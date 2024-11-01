namespace LetterKnowledgeAssessment.Models
{
    public class LetterSoundKnowledge
    {
        public Guid Id { get; set; }
        public string Letter { get; set; }
        public LetterKnowledgeLevel KnowledgeLevel { get; set; }
        public LetterSoundKnowledgeTestResult TestResult { get; set; }
    }
}
