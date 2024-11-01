using LetterKnowledgeAssessment.Interfaces;
using LetterKnowledgeAssessment.Models;
using Microsoft.EntityFrameworkCore;

namespace LetterKnowledgeAssessment.Handlers
{
    public class PupilHandler : IPupilHandler
    {
        private readonly IPupilRepository _pupilRepository;

        public PupilHandler(IPupilRepository pupilRepository)
        {
            _pupilRepository = pupilRepository;
        }

        public List<Pupil> PupilsByClassListId(string id)
        {
            return _pupilRepository.PupilsByClassListId(id).ToList();
        }

        public List<Pupil> PupilsByClassListIdPaginated(string id, int currentPage, int pageSize)
        {
            var pupils = PupilsByClassListId(id);
            return pupils.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }
        public int PupilCountByClassListId(string id)
        {
            return PupilsByClassListId(id).Count();
        }

        public Pupil GetPupilById(string id) 
        {
            return _pupilRepository.GetPupilById(id);
        }
        public Pupil AddPupil(Pupil pupil, ClassList classList)
        {
            pupil.ClassList = classList;
            try
            {
                _pupilRepository.AddPupil(pupil);
            }
            catch (DbUpdateException)
            {
                return null;
            }
            return pupil;
        }

        public List<Pupil> AddPupilList(List<Pupil> pupils, ClassList classList)
        {
            foreach(Pupil pupil in pupils)
            {
                pupil.ClassList = classList;
                pupil.PupilId = Guid.NewGuid();
                try
                {
                    _pupilRepository.AddPupil(pupil);
                }
                catch (DbUpdateException)
                {
                    return null;
                }
            }
            return pupils;
        }

        public Pupil UpdatePupil(Pupil pupil)
        {
            try
            {
                _pupilRepository.UpdatePupil(pupil);
            }
            catch (DbUpdateException)
            {
                return null;
            }
            return pupil;
        }

        public void RemovePupil(Pupil pupil)
        {
            _pupilRepository.RemovePupil(pupil);
        }
    }
}
