using LetterKnowledgeAssessment.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LetterKnowledgeAssessment.Areas.ClassManagement.Pages.Pupils
{
    public class RemoveModel : PageModel
    {
        private readonly IPupilHandler _pupilHandler;

        public RemoveModel(IPupilHandler pupilHandler)
        {
            _pupilHandler = pupilHandler;
        }

        public IActionResult OnPost(string pupilId)
        {
            var pupil = _pupilHandler.GetPupilById(pupilId);
            _pupilHandler.RemovePupil(pupil);
            return Redirect("/Overview/Pupils");
        }
    }
}
