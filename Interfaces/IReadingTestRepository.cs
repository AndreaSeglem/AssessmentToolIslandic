using LetterKnowledgeAssessment.Models;
namespace LetterKnowledgeAssessment.Interfaces
{
    public interface IReadingTestRepository
    {
        void SaveTest(ReadingTest test);
    }
}