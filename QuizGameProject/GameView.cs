using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer;
using BusinessLogicLayer.Models;
using ConsoleUI;

namespace QuizGameProject
{
    public class GameView
    {
        private static QuizService quizService;

        static GameView()
        {
            quizService = new QuizService();
        }

        public static Stats Start()
        {
            if(Stats.Categories.Count == 0)
            {
                Console.WriteLine("No quizes are added yet");
                Console.ReadKey();
                return null;
            }
            var categories = new List<string>(Stats.Categories);
            categories.Add("Mixed categories");
            categories.Add("Exit");
            int option = View.Menu(categories.ToArray(), "Choose category");
            List<Quiz> quizs = new List<Quiz>();

            if (option == categories.Count - 1)
                return null;
            else if (option == categories.Count - 2)
                quizs = quizService.GetAll();
            else
                quizs = quizService.GetAll().Where(q => q.CategoryIndex == option).ToList(); ;
            quizs = new List<Quiz>(MixQuizs(quizs, 10));
            Stats stats = new Stats();
            stats.CategoryIndex = option;

            foreach(var quiz in quizs)
            {
                bool[] answers = GetAnswers(quiz.Question, quiz.Options);
                stats.Answers.Add(IsCorrect(quiz.CorrectAnswers, answers));
            }
            return stats;
        }

        private static List<Quiz> MixQuizs(List<Quiz> quizs, int count)
        {
            Random random = new Random();
            List<int> nums = new List<int>();
            for (int i = 0; i < quizs.Count; i++)
                nums.Add(i);
            List<Quiz> newQuizs = new List<Quiz>();
            int length = Math.Min(quizs.Count, count);
            for(int i = 0; i < length; i++)
            {
                int index = nums[random.Next(0, nums.Count)];
                nums.Remove(index);
                newQuizs.Add(quizs[index]);
            }
            return newQuizs;
        }

        private static bool IsCorrect(List<int> answers, bool[] variants)
        {
            for (int i = 0; i < variants.Length; i++)
                if (answers.Contains(i) != variants[i]) return false;
            return true;
        }

        private static bool[] GetAnswers(string question, string[] options)
        {
            bool[] answers = new bool[3];
            int index = 0;
            Console.Clear();
            int posY = Console.CursorTop;
            while (true)
            {
                Console.CursorTop = posY;
                Console.CursorLeft = 0;
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("<Space> - choose answer; <Enter> - submit answer");
                Console.ResetColor();
                Console.WriteLine(question + '?');
                for(int i = 0; i < options.Length; i++)
                {
                    if (answers[i])
                        Console.ForegroundColor = ConsoleColor.Green;
                    if (i == index)
                        Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"{(char)('a' + i)}) {options[i]}");
                    Console.ResetColor();
                }
                var key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.DownArrow:
                        index = Math.Min(index + 1, options.Length);
                        break;
                    case ConsoleKey.UpArrow:
                        index = Math.Max(0, index - 1);
                        break;
                    case ConsoleKey.Spacebar:
                        answers[index] = !answers[index];
                        break;
                    case ConsoleKey.Enter:
                        if (!answers.Contains(true))
                            answers[index] = true;
                        return answers;
                }
            }
        }

        public static void ChangeQuiz()
        {
            var categories = new List<string>(Stats.Categories);
            categories.Add("Exit");
            int option = View.Menu(categories.ToArray(), "Choose category");
            if (option == categories.Count - 1)
                return;
            var quizs = quizService.GetAll().Where(q => q.CategoryIndex == option).ToList();
            Console.Clear();
            Console.WriteLine("Category: " + Stats.Categories[option]);
            int quizOption = View.Menu(quizs.Select(q => q.Question).ToArray(), "Choose question");
            Console.Clear();
            var newQuiz = CreateNewQuiz(option);
            var oldQuiz = quizs[quizOption];
            quizService.Update(oldQuiz, newQuiz);
            Console.WriteLine("Quiz has been changed!");
        }

        public static void CreateQuiz()
        {
            string category = Application.GetValidValue("Write a category: ", @"^\w+$");
            if(!Stats.Categories.Contains(category))
            {
                Stats.Categories.Add(category);
            }
            for(int i = 0; i < QuizService.QuizCount; i++)
            {
                quizService.Add(CreateNewQuiz(
                    Stats.Categories.FindIndex(c => c == category)));
            }
            Stats.SaveCategories();
            Console.WriteLine("Done!!!");
        }

        private static Quiz CreateNewQuiz(int categoryIndex)
        {
            string question = Application.GetValidValue("Write a question: ", @"^.+$");
            string[] options = new string[3];
            List<int> answers = new List<int>();
            for (int j = 0; j < 3; j++)
            {
                options[j] = Application.GetValidValue($"Write option {j + 1}: ", @"^.+$");
                if (View.Checker("the correct answer"))
                    answers.Add(j);
            }
            Console.WriteLine("-----------------------------");
            return new Quiz(categoryIndex, question, options, answers);
        }
    }
}
