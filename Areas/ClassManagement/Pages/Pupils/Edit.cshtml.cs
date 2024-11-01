using LetterKnowledgeAssessment.Interfaces;
using LetterKnowledgeAssessment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LetterKnowledgeAssessment.Areas.ClassManagement.Pages.Pupils
{
    public class EditModel : PageModel
    {
        private readonly IPupilHandler _pupilHandler;

        public EditModel(IPupilHandler pupilHandler)
        {
            _pupilHandler = pupilHandler;
        }

        public async Task<IActionResult> OnPost(string pupilId, [Bind("FirstName", "LastName", "BirthDate")] Pupil pupil)
        {
            var editTarget = _pupilHandler.GetPupilById(pupilId);
            editTarget.FirstName = pupil.FirstName ?? editTarget.FirstName;
            editTarget.LastName = pupil.LastName ?? editTarget.LastName;
            editTarget.BirthDate = pupil.BirthDate;
            _pupilHandler.UpdatePupil(editTarget);
            return Redirect($"~/Overview/DetailedOverview?pupilid={editTarget.PupilId}");
        }
    }
}
