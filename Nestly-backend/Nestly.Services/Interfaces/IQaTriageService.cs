namespace Nestly.Services.Interfaces
{
    public record QaUrgencyPrediction(bool IsUrgent, float Confidence);

    /// <summary>
    /// NLP text classifier (bag-of-n-grams + logistic regression, trained
    /// in Nestly.MLTraining on a synthetic Bosnian-language dataset of
    /// urgent vs. routine parent questions) that flags questions in the
    /// doctor's Q&amp;A inbox that likely need attention first. See
    /// who-standards-dataset.md for the labeling methodology.
    /// </summary>
    public interface IQaTriageService
    {
        QaUrgencyPrediction PredictUrgency(string questionText);
    }
}
