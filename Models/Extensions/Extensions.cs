namespace LetterKnowledgeAssessment.Models.Extensions
{
    public static class Extensions
    {
        private static Random rng = new Random();
        public static IEnumerable<T> Shuffle<T>(this IList<T> list)
        {
            var elements = list.ToArray();
            for (var i = elements.Length - 1; i >= 0; i--)
            {
                var swapIndex = rng.Next(i + 1);
                yield return elements[swapIndex];
                elements[swapIndex] = elements[i];
            }

        }
    }
}
