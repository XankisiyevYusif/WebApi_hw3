namespace WebApplication2.Services
{
    public class ArrayService
    {
        private List<string> words;

        public ArrayService(string filepath)
        {
            words = File.ReadAllLines(filepath).ToList();
        }

        public string GetWord()
        {
            Random random = new Random();   
            int index = random.Next(words.Count);

            return words[index];
        }
    }
}
