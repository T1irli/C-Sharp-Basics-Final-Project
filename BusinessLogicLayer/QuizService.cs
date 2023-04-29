using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataLogicLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class QuizService : IService<Quiz>
    {
        private List<Quiz> quizs;
        private string path = "quizs.xml";

        public static int QuizCount = 3;

        public QuizService()
        {
            if (File.Exists(path))
            {
                quizs = FileXmlSerrealization.Read<List<Quiz>>(path);
            }
            else
            {
                quizs = new List<Quiz>();
                FileXmlSerrealization.Write<List<Quiz>>(path, quizs);
            }
        }

        public void Add(Quiz item)
        {
            quizs.Add(item);
            FileXmlSerrealization.Write<List<Quiz>>(path, quizs);
        }

        public List<Quiz> GetAll()
        {
            return quizs;
        }

        public void Remove(Quiz item)
        {
            quizs.Remove(item);
            FileXmlSerrealization.Write<List<Quiz>>(path, quizs);
        }

        public void Update(Quiz oldItem, Quiz newItem)
        {
            quizs[quizs.FindIndex(q => q.Equals(oldItem))] = newItem;
            FileXmlSerrealization.Write<List<Quiz>>(path, quizs);
        }
    }
}
