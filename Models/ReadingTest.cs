using System;
using System.Collections.Generic;

namespace LetterKnowledgeAssessment.Models
{
    public class ReadingTest
    {
        public Guid Id { get; set; }  // Unik ID for testen
        public Guid PupilId { get; set; }
        public Pupil Pupil { get; set; } // Referanse til eleven som tok testen
        public DateTimeOffset Date { get; set; }
        public bool IsTestPassed { get; set; } // Om eleven bestod testen eller ikke
        public List<ReadingTestWord> WordResults { get; set; } // Liste over ordene i testen
    }
}
