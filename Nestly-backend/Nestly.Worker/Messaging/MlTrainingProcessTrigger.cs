using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Nestly.Worker.Messaging
{
    // Runs the existing offline training pipeline (Nestly.MLTraining -
    // today generates a fresh synthetic dataset and retrains the five
    // deviation-detection models against it, see that project's Program.cs)
    // as an external "dotnet run" process, the same way a developer would
    // run it by hand. This is intentionally a thin process wrapper rather
    // than pulling ML.NET training into the always-on worker: training is
    // CPU/memory heavy and occasional, so isolating it in its own process
    // keeps a slow or misbehaving run from affecting the worker's other
    // background jobs (reminders, deviation checks, RabbitMQ consumption).
    public class MlTrainingProcessTrigger : IMlTrainingTrigger
    {
        private readonly IConfiguration _config;
        private readonly ILogger<MlTrainingProcessTrigger> _logger;

        public MlTrainingProcessTrigger(IConfiguration config, ILogger<MlTrainingProcessTrigger> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<bool> TriggerAsync(CancellationToken ct)
        {
            var backendRoot = _config["MlTraining:BackendRootPath"];

            if (string.IsNullOrWhiteSpace(backendRoot))
            {
                backendRoot = ComputeDefaultBackendRoot();
            }

            if (!Directory.Exists(Path.Combine(backendRoot, "Nestly.MLTraining")))
            {
                _logger.LogError(
                    "Cannot locate Nestly.MLTraining under resolved backend root '{BackendRoot}'. " +
                    "Set MlTraining:BackendRootPath in configuration if the worker runs from a different layout.",
                    backendRoot);
                return false;
            }

            _logger.LogInformation("Starting scheduled ML retraining run in {BackendRoot}.", backendRoot);

            var psi = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "run --project Nestly.MLTraining -c Release",
                WorkingDirectory = backendRoot,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            try
            {
                using var process = Process.Start(psi);

                if (process is null)
                {
                    _logger.LogError("Failed to start the ML retraining process.");
                    return false;
                }

                var stdOutTask = process.StandardOutput.ReadToEndAsync(ct);
                var stdErrTask = process.StandardError.ReadToEndAsync(ct);

                await process.WaitForExitAsync(ct);

                var stdOut = await stdOutTask;
                var stdErr = await stdErrTask;

                if (process.ExitCode == 0)
                {
                    _logger.LogInformation("ML retraining run completed successfully.\n{Output}", stdOut);
                    return true;
                }

                _logger.LogError(
                    "ML retraining run exited with code {ExitCode}.\nStdOut: {StdOut}\nStdErr: {StdErr}",
                    process.ExitCode, stdOut, stdErr);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ML retraining run failed to execute.");
                return false;
            }
        }

        private static string ComputeDefaultBackendRoot()
        {
            var baseDir = AppContext.BaseDirectory;
            return Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", ".."));
        }
    }
}
