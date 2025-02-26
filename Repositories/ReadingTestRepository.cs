using LetterKnowledgeAssessment.Interfaces;
using LetterKnowledgeAssessment.Models;
using LetterKnowledgeAssessment.Data;

namespace LetterKnowledgeAssessment.Repositories
{
    public class ReadingTestRepository : IReadingTestRepository
    {
        private readonly ApplicationDbContext _context;

        public ReadingTestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void SaveTest(ReadingTest test)
        {
            _context.ReadingTests.Add(test);
            _context.SaveChanges();
        }
    }
}