using LetterKnowledgeAssessment.Models;

namespace LetterKnowledgeAssessment.Interfaces
{
    public interface IClassListHandler
    {
        List<ClassList> ClassListsByTeacher(Teacher teacher);
        ClassList ClassListById(string id);
        ClassList AddClassList(ClassList classList, Teacher teacher);
        ClassList UpdateClassList(ClassList classList);
        void RemoveClassList(ClassList classList);
    }
}
