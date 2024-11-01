using LetterKnowledgeAssessment.Interfaces;
using LetterKnowledgeAssessment.Models;
using Microsoft.EntityFrameworkCore;

namespace LetterKnowledgeAssessment.Handlers
{
    public class ClassListHandler : IClassListHandler
    {
        private readonly IClassListRepository _classListRepository;
        public ClassListHandler(IClassListRepository classListRepository)
        {
            _classListRepository = classListRepository;
        }
        public List<ClassList> ClassListsByTeacher(Teacher teacher)
        {
            return _classListRepository.ClassListsByTeacherId(teacher.Id).ToList();
        }
        public ClassList ClassListById(string id)
        {
            return _classListRepository.ClassListById(id);
        }
        public ClassList AddClassList(ClassList classList, Teacher teacher)
        {
            classList.Teacher = teacher;
            try
            {
                _classListRepository.AddClassList(classList);
            }
            catch (DbUpdateException)
            {
                return null;
            }
            return classList;
        }


        public ClassList UpdateClassList(ClassList classList)
        {
            try
            {
                _classListRepository.UpdateClassList(classList);
            }
            catch (DbUpdateException)
            {
                return null;
            }
            return classList;
        }

        public void RemoveClassList(ClassList classList)
        {
            _classListRepository.RemoveClassList(classList);
        }
    }
}
