using System;
using System.Text;


namespace RandomTextGenerator
{
    public class RandomText
    {
        private Random random;
        public RandomText()
        {
            this.random = new Random();
        }
        public string GetRandomSentence(int wordCount)
        {
	    /*
            string[] words = { "an", "automobile", "or", "motor", "car", "is", "a", "wheeled", "motor", "vehicle", "used", "for", "transporting", "passengers", "which", "also", "carries", "its", "own", "engine", "or" };
	    */
	    string[] sg_nouns = {"box", "block", "mug", "cup", "knife", "plate", "one"};
            string[] pl_nouns = {"blocks", "boxes", "mugs","cups","knives", "plates", "ones"};
            string[] sg_determiners = { "this", "that" };
            string[] sg_determiners = { "these", "those"};
            string[] adjectives = { "yellow", "red", "blue", "purple", "green", "orange", "white",
		"gray", "black", "pink", "brown" };
            string[] prepositions = { "on the left of", "on the right of", "to the left of",
		"to the right of", "left of", "right of", "above", "below", "behind", "in", "on",
                "beside", "before", "around",  "on top of", "in front of", "in back of", "on the front of",
		"on the back of"};
            string[] det_adj = { };
            string[] adj_no_det = { };
            List<string> det_no_adj = new List<string>();
            List<string> det_adj = new List<string>();
            List<string> adj_no_det = new List<string>();
	    /*
	    
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < wordCount; i++)
            {
                // Select a random word from the array
                builder.Append(words[random.Next(words.Length)]).Append(" ");
            }

            string sentence = builder.ToString().Trim() + ". ";

            // Set the first letter of the first word in the sentenece to uppercase
            sentence = char.ToUpper(sentence[0]) + sentence.Substring(1);

            builder = new StringBuilder();
            builder.Append(sentence);
	    */
            return builder.ToString();
        }
        public static void Main(string[] args)
        {
            RandomText r = new RandomText();
            Console.WriteLine(r.GetRandomSentence(5));
            Console.WriteLine(r.GetRandomSentence(10));
            Console.WriteLine(r.GetRandomSentence(15));
        }
    }
}
