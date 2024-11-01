using LetterKnowledgeAssessment.Data;
using LetterKnowledgeAssessment.Interfaces;
using LetterKnowledgeAssessment.Models;
using Microsoft.EntityFrameworkCore;

namespace LetterKnowledgeAssessment.Repositories
{
    public class LetterTestRepository : ILetterTestRepository
    {
        private readonly ApplicationDbContext _context;

        public LetterTestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<LetterSoundKnowledgeTestResult> TestResultsByPupilId(string pupilId)
        {
            var result = _context.LetterKnowledgeTestResults.Include(t => t.LetterTestResult).Where(t => t.Pupil.PupilId.ToString().Equals(pupilId)).OrderByDescending(testResult => testResult.Date);
            return result;
        }

        public LetterSoundKnowledgeTestResult TestResultById(string testId)
        {
            return _context.LetterKnowledgeTestResults.Include(t => t.LetterTestResult).Where(t => t.Id.ToString().Equals(testId)).FirstOrDefault();
        }


        public void AddTestResult(LetterSoundKnowledgeTestResult testResult)
        {
            _context.Add(testResult);
            _context.SaveChanges();
        }

        public void UpdateTestResult(LetterSoundKnowledgeTestResult testResult)
        {
            _context.Update(testResult);
            _context.SaveChanges();
        }

        public void RemoveTestResult(LetterSoundKnowledge testResult)
        {
            _context.Remove(testResult);
            _context.SaveChanges();
        }

    }
}
