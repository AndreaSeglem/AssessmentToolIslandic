using LetterKnowledgeAssessment.Data;
using LetterKnowledgeAssessment.Interfaces;
using LetterKnowledgeAssessment.Models;
using Microsoft.EntityFrameworkCore;

namespace LetterKnowledgeAssessment.Repositories
{
    public class ClassListRepository : IClassListRepository
    {
        private readonly ApplicationDbContext _context;

        public ClassListRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<ClassList> ClassListsByTeacherId(string id)
        {
            return _context.ClassLists.Include(c => c.Pupils).Where(c => c.Teacher.Id.Equals(id));
        }

        public ClassList ClassListById(string id)
        {
            return _context.ClassLists.Include(c => c.Pupils).Where(c => c.Id.ToString().Equals(id)).FirstOrDefault();
        }

        public void AddClassList(ClassList classList)
        {
            _context.ClassLists.Add(classList);
            _context.SaveChanges();
        }

        public void UpdateClassList(ClassList classList)
        {
            _context.ClassLists.Update(classList);
            _context.SaveChanges();
        }

        public void RemoveClassList(ClassList classList)
        {
            _context.ClassLists.Remove(classList);
            _context.SaveChanges();
        }
    }
}
