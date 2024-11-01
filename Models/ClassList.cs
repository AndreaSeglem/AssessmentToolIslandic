namespace LetterKnowledgeAssessment.Models
{
    public class ClassList
    {
        public Guid Id { get; set; }
        public string Grade { get; set; }
        public Teacher Teacher { get; set; }
        public List<Pupil>? Pupils { get; set; }
    }
}
