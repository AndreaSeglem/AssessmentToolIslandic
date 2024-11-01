using System.ComponentModel.DataAnnotations;

namespace LetterKnowledgeAssessment.Models
{
    public class LetterSoundKnowledgeTestResult
    {
        public Guid Id { get; set; }   
        public DateTimeOffset Date { get; set; }
        public bool IsUpperCase { get; set; }
        public List<LetterSoundKnowledge>? LetterTestResult { get; set; }
        public Pupil Pupil { get; set; }
    }
}
