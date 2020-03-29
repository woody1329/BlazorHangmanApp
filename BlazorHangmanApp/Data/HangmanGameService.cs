using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//Created by Elwood Fitton 
namespace BlazorHangmanApp.Data
{
    public class HangmanGameService
    {
        public string Word;
        public string lettersLeftInWord;
        public int WordLength;
        public int guessesLeft = 6;
        public List<char> missedLetters = new List<char>();
        public string HungWord;
        //public char currentLetter;
        public bool gameOver = false;
        public IWebHostEnvironment WebHostEnvironment;
        public char[] alphabet = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        public Dictionary<char, string> alphabetColorDict = new Dictionary<char, string>(){

            {'a',"blue"},
            {'b',"blue"},
            {'c',"blue"},
            {'d',"blue"},
            {'e',"blue"},
            {'f',"blue"},
            {'g',"blue"},
            {'h',"blue"},
            {'i',"blue"},
            {'j',"blue"},
            {'k',"blue"},
            {'l',"blue"},
            {'m',"blue"},
            {'n',"blue"},
            {'o',"blue"},
            {'p',"blue"},
            {'q',"blue"},
            {'r',"blue"},
            {'s',"blue"},
            {'t',"blue"},
            {'u',"blue"},
            {'v',"blue"},
            {'w',"blue"},
            {'x',"blue"},
            {'y',"blue"},
            {'z',"blue"},
        };

        
        public HangmanGameService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
            string[] words = File.ReadAllLines(TextFileName);
            Random rnd = new Random();
            int wordIndex = (int)(rnd.NextDouble() * words.Length);
            Word = words[wordIndex];
            lettersLeftInWord = Word;
            WordLength = Word.Length;
            HungWord = CreateRepeatedString(WordLength);

        }

        public string TextFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "words.txt"); }
        }
        public Task<string> GetRandomWordAsync()
        {
            string[] words = File.ReadAllLines(TextFileName);
            Random rnd = new Random();
            int wordIndex = (int)(rnd.NextDouble() * words.Length);
             
            return Task.FromResult(words[wordIndex]);
        }

        public string CreateRepeatedString(int wordlength)
        {
            string s = "_ ";
            return new StringBuilder(s.Length * wordlength)
                                    .Insert(0, s, wordlength)
                                    .ToString();
        }
        public bool TestSubmittedLetter(char letter)
        {
            return true;
        }
        public void PopulateWordFields()
        {
            lettersLeftInWord = Word;
            //WordLength = Word.Length;
            //HungWord = CreateRepeatedString(WordLength);
            //return Word;
        }
        public Task<string> ProcessCurrentLetterAsync(char currentLetter, string word)
        {
            CheckForMultipleLetters(currentLetter, word);
            if (!missedLetters.Contains(currentLetter) && !word.Contains(currentLetter))
            {
                alphabetColorDict[currentLetter] = "red";
                missedLetters.Add(currentLetter);
                guessesLeft--;
                Console.WriteLine($"You have {guessesLeft} guesses left.");
            }
            else { if (!(alphabetColorDict[currentLetter] == "red")) alphabetColorDict[currentLetter] = "green";
      
            }
            return Task.FromResult(HungWord);

        }
        private int CheckForMultipleLetters(char currentLetter, string word)
        {
            int i = 0;
            while (LetterInWord(currentLetter, lettersLeftInWord))
            {
                i++;
                if (i == 1) System.Console.WriteLine("Letter is in word");
                else Console.WriteLine("Letter is in word again, luck is on your side!");
                UpdateLettersLeftInWord(currentLetter);
                //UpdateAlphabetLetters(currentLetter);
            }
            UpdateHungWord(IndexesOfLetters(word, currentLetter), currentLetter);
            return i;
        }
        public List<int> IndexesOfLetters(string w, char v)
        {
            List<int> letterIndexes = new List<int>();
            int charCounter = 0;
            foreach (char a in w)
            {
                if (a.Equals(v)) letterIndexes.Add(charCounter);

                charCounter++;
            }

            //UpdateW(v);
            return letterIndexes;
        }
        public bool LetterInWord(char v, string word)
        {
            return word.Contains(v);
        }

        private void UpdateLettersLeftInWord(char v)
        {
            this.lettersLeftInWord = this.lettersLeftInWord.Remove(this.lettersLeftInWord.IndexOf(v), 1);
            Console.WriteLine(lettersLeftInWord);
        }

        public void UpdateHungWord(List<int> letterIndexes, char letter)
        {
            foreach (int i in letterIndexes)
            {
                HungWord = HungWord.Remove(i == 0 ? 0 : i * 2, 1)
                                             .Insert(i == 0 ? 0 : i * 2, letter.ToString());
                if (!HungWord.Contains("_"))
                {
                    guessesLeft = -1;
                    gameOver = true;
                }
            }
        }

    }
}
