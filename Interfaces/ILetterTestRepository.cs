using LetterKnowledgeAssessment.Models;

namespace LetterKnowledgeAssessment.Interfaces
{
    public interface ILetterTestRepository
    {
        IQueryable<LetterSoundKnowledgeTestResult> TestResultsByPupilId(string pupilId);
        LetterSoundKnowledgeTestResult TestResultById(string testId);
        void AddTestResult(LetterSoundKnowledgeTestResult testResult);
        void UpdateTestResult(LetterSoundKnowledgeTestResult testResult);
        void RemoveTestResult(LetterSoundKnowledge testResult);
    }
}
