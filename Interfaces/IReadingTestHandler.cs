namespace LetterKnowledgeAssessment.Interfaces
{
    public interface IReadingTestHandler
    {
        void StartTest(string pupilId);
        List<string> GetWordList();
    }
}