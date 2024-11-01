using Microsoft.AspNetCore.Identity;

namespace LetterKnowledgeAssessment.Models
{
    public class Teacher : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<ClassList> ClassLists { get; set; }
    }
}
