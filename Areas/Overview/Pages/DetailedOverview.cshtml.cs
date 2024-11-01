using LetterKnowledgeAssessment.Interfaces;
using LetterKnowledgeAssessment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LetterKnowledgeAssessment.Areas.Overview.Pages
{
    [Authorize]
    public class DetailedOverviewModel : PageModel
    {
        private readonly IPupilHandler _pupilHandler;
        private readonly ILetterTestHandler _letterTestHandler;
        private readonly JsonSerializerSettings _serializerSettings;

        public DetailedOverviewModel(IPupilHandler puilHandler, ILetterTestHandler letterTestHandler)
        {
            _pupilHandler = puilHandler;
            _letterTestHandler = letterTestHandler;
            _serializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,

            };
        }
        public Pupil? Pupil { get; set; }
        public List<LetterSoundKnowledgeTestResult> TestResults { get; set; }
        public string TestResultsUpperSerialized { get; set; }
        public string TestResultsLowerSerialized { get; set; }
        public double AvgUpperCaseSounds { get; set; }
        public double AvgLowerCaseSounds { get; set; }

        public IActionResult OnGet(string pupilId)
        {
            if (string.IsNullOrEmpty(pupilId))
            {
                return NotFound();
            }
            Pupil = _pupilHandler.GetPupilById(pupilId);

            if (Pupil == null)
            {
                return NotFound();
            }
            TestResults = _letterTestHandler.TestResultsByPupilId(Pupil.PupilId.ToString());
            TestResultsUpperSerialized = JsonConvert.SerializeObject(TestResults.Where(t => t.IsUpperCase).ToList(), _serializerSettings);
            TestResultsLowerSerialized = JsonConvert.SerializeObject(TestResults.Where(t => !t.IsUpperCase).ToList(), _serializerSettings);
            var averages = _letterTestHandler.SoundKnowledgeAveragesBasedOnTime(DateTime.Now.Month);
            AvgUpperCaseSounds = averages["upperCaseAvg"];
            AvgLowerCaseSounds = averages["lowerCaseAvg"];

            return Page();
        }
    }
}
