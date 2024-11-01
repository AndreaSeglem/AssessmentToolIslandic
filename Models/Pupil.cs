namespace LetterKnowledgeAssessment.Models
{
    public class Pupil
    {
        public Guid PupilId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int KnowledgeLevel { get; set; }
        public virtual ICollection<LetterSoundKnowledgeTestResult> LetterSoundKnowledgeTestResults { get; set; }
        public ClassList ClassList { get; set; }
    }
}
