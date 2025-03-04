using LetterKnowledgeAssessment.Data;
using LetterKnowledgeAssessment.Interfaces;
using LetterKnowledgeAssessment.Models;
using Microsoft.EntityFrameworkCore;

namespace LetterKnowledgeAssessment.Repositories
{
    public class PupilRepository : IPupilRepository
    {
        private readonly ApplicationDbContext _context;

        public PupilRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Pupil> PupilsByClassListId(string id) 
        {
            var pupils = _context.Pupils
            .Include(p => p.LetterSoundKnowledgeTestResults)
            .Include(p => p.ReadingTests) // Viktig!
            .Where(p => p.ClassList.Id.ToString().Equals(id));

            Console.WriteLine($"Pupils fetched for class {id}: {pupils.Count()}");
    
            foreach (var pupil in pupils)
            {
                Console.WriteLine($"Pupil: {pupil.FirstName} {pupil.LastName}, ReadingTests: {pupil.ReadingTests.Count}");
            }

            return pupils;
        }

        public Pupil GetPupilById(string id)
        {
            return _context.Pupils.Include(p => p.LetterSoundKnowledgeTestResults).Include(p => p.ReadingTests).Where(p => p.PupilId.ToString().Equals(id)).FirstOrDefault();
        }
        public void AddPupil(Pupil pupil)
        {
            _context.Add(pupil);
            _context.SaveChanges();
        }
        public void UpdatePupil(Pupil pupil)
        {
            _context.Update(pupil);
            _context.SaveChanges();
        }
        public void RemovePupil(Pupil pupil)
        {
            _context.Remove(pupil);
            _context.SaveChanges();
        }
    }
}
