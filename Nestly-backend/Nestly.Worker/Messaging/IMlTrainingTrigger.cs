namespace Nestly.Worker.Messaging
{
    public interface IMlTrainingTrigger
    {
        Task<bool> TriggerAsync(CancellationToken ct);
    }
}
