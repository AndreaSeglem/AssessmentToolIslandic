using System;

namespace LetterKnowledgeAssessment.Models
{
    public class ReadingTestWord
    {
        public Guid Id { get; set; }
        public string Word { get; set; } // Ordet som ble testet
        public bool IsReadCorrectly { get; set; } // Om eleven leste ordet riktig
        public ReadingTest ReadingTest { get; set; } // Tilhørende lesetest
    }
}
