using LetterKnowledgeAssessment.Models;

namespace LetterKnowledgeAssessment.Interfaces
{
    public interface IClassListRepository
    {
        IQueryable<ClassList> ClassListsByTeacherId(string id);
        ClassList ClassListById(string id);
        void AddClassList(ClassList classList);
        void UpdateClassList(ClassList classList);
        void RemoveClassList(ClassList classList);
    }
}
