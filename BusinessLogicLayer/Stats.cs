using BusinessLogicLayer.Models;
using DataLogicLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    [Serializable]
    public class Stats
    {
        [NonSerialized]
        private int categoryIndex;
        private static string path = "category.xml";

        public static List<string> Categories { get; set; }

        public int CategoryIndex { get => categoryIndex; 
            set {
                if(value > Stats.Categories.Count || value < 0)
                    throw new ArgumentException();
                else categoryIndex = value;
            }
        }
        public List<bool> Answers { get; set; }

        public Stats(int categoryIndex, List<Quiz> quizs, List<bool> answers)
        {
            if(quizs == null || answers == null) throw new ArgumentException();
            if(quizs.Count != answers.Count) throw new ArgumentException();
            this.CategoryIndex = categoryIndex;
            this.Answers = new List<bool>(answers);
        }

        static Stats()
        {
            if (File.Exists(path))
            {
                Categories = FileXmlSerrealization.Read<List<string>>(path);
            }
            else
            {
                Categories = new List<string>();
                FileXmlSerrealization.Write<List<string>>(path, Categories);
            }
        }

        public Stats()
        {
            this.Answers = new List<bool>();
        }

        public static void SaveCategories()
        {
            FileXmlSerrealization.Write(path, Categories);
        }
    }
}
