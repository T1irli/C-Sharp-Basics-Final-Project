using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Models
{
    [Serializable]
    public class Quiz
    {
        [NonSerialized]
        private int id;
        private string question;
        private int categoryIndex;
        private List<int> correctAnswers;

        public string Question { get => question; 
            set {
                if (String.IsNullOrWhiteSpace(value))
                    throw new ArgumentException();
                else question = value;
            }
        }
        public int CategoryIndex { get => categoryIndex; 
            set {
                if(value > Stats.Categories.Count || value < 0)
                    throw new ArgumentException();
                else categoryIndex = value;
            }
        }
        public string[] Options { get; set; }
        public List<int> CorrectAnswers { get => correctAnswers; 
            set {
                if(value.Count > 2)
                    throw new ArgumentException();
                foreach(var item in value)
                {
                    if(item > 2 || item < 0)
                        throw new ArgumentException();
                }
                correctAnswers = value;
            } 
        }

        public Quiz(int categoryIndex, string question, string[] options, List<int> correctAnswers)
        {
            if(options == null || correctAnswers == null || options.Length != 3) throw new ArgumentException();
            this.id = GetHashCode();
            this.CategoryIndex = categoryIndex;
            this.Question = question;
            this.Options = new string[3];
            for (int i = 0; i < options.Length; i++)
            {
                this.Options[i] = options[i];
            }
            this.CorrectAnswers = new List<int>(correctAnswers);
        }

        public Quiz()
        {
            this.id = GetHashCode();
            this.Options = new string[3];
        }

        public override string ToString()
        {
            string result = $"{this.Question}?\n";
            for(int i = 0; i < this.Options.Length; i++)
            {
                result += $"{(char)('a' + i)}) {this.Options[i]}\n";
            }
            return result;
        }

        public override bool Equals(object obj)
        {
            return this.id == (obj as Quiz).id;
        }
    }
}
