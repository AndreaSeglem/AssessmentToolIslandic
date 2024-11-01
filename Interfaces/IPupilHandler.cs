using LetterKnowledgeAssessment.Models;

namespace LetterKnowledgeAssessment.Interfaces
{
    public interface IPupilHandler
    {
        List<Pupil> PupilsByClassListId(string id);
        List<Pupil> PupilsByClassListIdPaginated(string id, int currentPage, int pageSize);
        int PupilCountByClassListId(string id);
        Pupil GetPupilById(string id);
        Pupil AddPupil(Pupil pupil, ClassList classList);
        List<Pupil> AddPupilList(List<Pupil> pupils, ClassList classList);
        Pupil UpdatePupil(Pupil pupil);
        void RemovePupil(Pupil pupil);
    }
}
