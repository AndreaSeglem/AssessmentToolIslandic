using LetterKnowledgeAssessment.Interfaces;
using LetterKnowledgeAssessment.Models;

namespace LetterKnowledgeAssessment.Handlers
{
    public class ReadingTestHandler : IReadingTestHandler
    {
        private readonly IReadingTestRepository _readingTestRepository;

        public ReadingTestHandler(IReadingTestRepository readingTestRepository)
        {
            _readingTestRepository = readingTestRepository;
        }

        public void StartTest(string pupilId)
        {
            var test = new ReadingTest { PupilId = pupilId, StartTime = DateTime.UtcNow };
            _readingTestRepository.SaveTest(test);
        }
    }
}