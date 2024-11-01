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
            return _context.Pupils.Include(p => p.LetterSoundKnowledgeTestResults).Where(p => p.ClassList.Id.ToString().Equals(id));
        }
        public Pupil GetPupilById(string id)
        {
            return _context.Pupils.Include(p => p.LetterSoundKnowledgeTestResults).Where(p => p.PupilId.ToString().Equals(id)).FirstOrDefault();
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
