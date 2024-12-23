using Automat.Application.Interfaces;
using Automat.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automat.Infrastructure.Services
{
    public class AutomatService : IResponseAutomat
    {
        public ResponseEntity GetWordFrecuency(string word, string text)
        {
			try
			{
                //using the KPM error table and creating the automat: 
                string normalizedWord = word.ToLower();
                string normalizedText = text.ToLower();

                int[] failureFunction = BuildFailureFunction(normalizedWord);

                int wordIndex = 0; 
                int frequency = 0;

                for (int textIndex = 0; textIndex < normalizedText.Length; textIndex++)
                {
                    while (wordIndex > 0 && normalizedText[textIndex] != normalizedWord[wordIndex])
                    {
                        wordIndex = failureFunction[wordIndex - 1];
                    }

                    if (normalizedText[textIndex] == normalizedWord[wordIndex])
                    {
                        wordIndex++;
                    }
                    if (wordIndex == normalizedWord.Length)
                    {
                        frequency++;
                        wordIndex = failureFunction[wordIndex - 1];
                    }
                }

                return new ResponseEntity { Word = word, WordFrecuency = frequency };

            }
			catch (Exception)
			{
				throw new Exception("algo salio mal intenta de nuevo mas tarde");
			}
        }

        private static int[] BuildFailureFunction(string pattern)
		{
            int[] failureFunction = new int[pattern.Length];
            int j = 0;

            for (int i = 1; i < pattern.Length; i++)
            {
                while (j > 0 && pattern[i] != pattern[j])
                {
                    j = failureFunction[j - 1];
                }

                if (pattern[i] == pattern[j])
                {
                    j++;
                }

                failureFunction[i] = j;
            }

            return failureFunction;
        }
    }
}
