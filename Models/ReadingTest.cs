using System;
using System.Collections.Generic;

namespace LetterKnowledgeAssessment.Models
{
    public class ReadingTest
    {
        public Guid Id { get; set; }  // Unik ID for testen
        public Guid PupilId { get; set; }
        public Pupil Pupil { get; set; } // Referanse til eleven som tok testen
        public DateTime Date { get; set; } // Dato testen ble gjennomført
        public bool IsTestPassed { get; set; } // Om eleven bestod testen eller ikke
        public List<ReadingTestWord> WordResults { get; set; } // Liste over ordene i testen
    }
}
