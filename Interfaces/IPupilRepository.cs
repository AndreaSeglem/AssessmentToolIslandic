using LetterKnowledgeAssessment.Models;

namespace LetterKnowledgeAssessment.Interfaces
{
    public interface IPupilRepository
    {
        IQueryable<Pupil> PupilsByClassListId(string id);
        Pupil GetPupilById(string id);
        void AddPupil(Pupil pupil);
        void UpdatePupil(Pupil pupil);
        void RemovePupil(Pupil pupil);
    }
}
