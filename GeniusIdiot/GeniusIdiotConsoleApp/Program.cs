using System;
using System.IO;

namespace GeniusIdiotConsoleApp
{
    internal class Program
    {

        static void Main(string[] args)
        {
            bool isYes = true;
            while (isYes)
            {
                string userName = GetUserName();

                int countQuestions = 5;
                string[] questions = GetQuestions(countQuestions);
                int[] answers = GetRightAnswers(countQuestions);
                int[] mixedIndexes = MixIndexes(countQuestions);

                int countRightAnswers = 0;

                for (int i = 0; i < countQuestions; i++)
                {
                    Console.WriteLine("Вопрос №" + (i + 1));
                    Console.WriteLine(questions[mixedIndexes[i]]);

                    int userAnswer = GetUserAnswerNumber();
                    int rightAnswer = answers[mixedIndexes[i]];
                    if (userAnswer == rightAnswer)
                        countRightAnswers++;
                }

                string userDiagnose = GetUserDiagnose(countQuestions, countRightAnswers);

                Console.WriteLine($"Количество правильных ответов: {countRightAnswers}");
                Console.WriteLine($"{userName}, ваш диагноз: {userDiagnose}");

                string userResultTest = $"{userName},{countRightAnswers},{userDiagnose}";
                string dirName = @".\test results.txt";

                WritingResultToFile(userResultTest, dirName);

                isYes = GetUserChoice("Вы хотите посмотреть таблицу результатов теста?");
                if (isYes)
                    GetResultFromFile(dirName);

                isYes = GetUserChoice("Вы хотите повторить тест? Ответ: да или нет");
            }
        }

        static string GetUserName()
        {
            Console.WriteLine("Здравствуйте! Введите ваши фамилию, имя и отчество:");
            int userNameLenght = 40;
            string userName;
            do
            {
                userName = Console.ReadLine();
                if (userName.Length > userNameLenght)
                    Console.WriteLine($"Вы ввели слишком много знаков, пожалуйста, сократите ФИО до {userNameLenght} знаков.");

            } while (userName.Length > userNameLenght);

            return userName;
        }

        static string[] GetQuestions(int countQuestions)
        {
            string[] questions = new string[countQuestions];
            questions[0] = "Сколько будет два плюс два умноженное на два?";
            questions[1] = "Бревно нужно распилить на 10 частей. Сколько распилов нужно сделать?";
            questions[2] = "На двух руках 10 пальцев. Сколько пальцев на 5 руках?";
            questions[3] = "Укол делают каждые полчаса. Сколько нужно минут, чтобы сделать три укола?";
            questions[4] = "Пять свечей горело, две потухли. Сколько свечей осталось?";
            return questions;
        }

        static int[] GetRightAnswers(int countQuestions)
        {
            int[] answers = new int[countQuestions];
            answers[0] = 6;
            answers[1] = 9;
            answers[2] = 25;
            answers[3] = 60;
            answers[4] = 2;
            return answers;
        }
        static int[] MixIndexes(int countQuestions)
        {
            int[] indexes = new int[countQuestions];
            for (int i = 0; i < countQuestions; i++)
            {
                indexes[i] = i;
            }
            Random random = new Random();
            for (int i = 0; i < countQuestions; i++)
            {
                int j = random.Next(i + 1);
                int temp = indexes[j];
                indexes[j] = indexes[i];
                indexes[i] = temp;
            }
            return indexes;
        }
        static int GetUserAnswerNumber()
        {
            bool isNumber = false;
            int userAnswerNumber = 0;
            do
            {
                string userAnswer = Console.ReadLine();
                if (userAnswer.Length > 10)
                {
                    Console.WriteLine("Вы ввели слишком длинный ответ! Ответом считается целое число до 10 разрядов.");
                    continue;
                }
                isNumber = int.TryParse(userAnswer, out userAnswerNumber);
                if (!isNumber)
                {
                    Console.WriteLine("Пожалуйста, введите целое число!");
                }
                else
                    userAnswerNumber = Convert.ToInt32(userAnswer);

            } while (!isNumber);
            return userAnswerNumber;
        }

        static string GetUserDiagnose(int countQuestions, int countRightAnswers)
        {
            string userDiagnose = "";
            int numberDiagnoses = 6;
            double percenteRightAnswers = countRightAnswers * 100 / countQuestions;
            double graduationStep = 100 / (double)numberDiagnoses;

            string[] diagnoses = new string[numberDiagnoses];
            diagnoses[0] = "Идиот";
            diagnoses[1] = "Кретин";
            diagnoses[2] = "Дурак";
            diagnoses[3] = "Нормальный";
            diagnoses[4] = "Талант";
            diagnoses[5] = "Гений";

            for (int i = 0; i < numberDiagnoses; i++)
            {
                if (percenteRightAnswers <= graduationStep * (i + 1))
                {
                    userDiagnose = diagnoses[i];
                    break;
                }
            }
            return userDiagnose;
        }

        static bool GetUserChoice(string message)
        {
            Console.WriteLine(message);
            string userUnswer = Console.ReadLine().ToLower();

            while (userUnswer != "да" && userUnswer != "нет")
            {
                Console.WriteLine("Вы ввели неверный ответ. Введите да или нет");
                userUnswer = Console.ReadLine().ToLower();
            }
            if (userUnswer == "да")
                return true;
            else
                return false;
        }

        static void WritingResultToFile(string userResultTest, string dirName)
        {
            FileInfo file1 = new FileInfo(dirName);
            if (!file1.Exists)
            {
                file1.Create();
                using (StreamWriter sw = new StreamWriter(dirName, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(userResultTest);
                }
            }
            else
            {
                // Можно только вот этот кусок оставить
                using (StreamWriter sw = new StreamWriter(dirName, true, System.Text.Encoding.Default))
                {
                    sw.WriteLine(userResultTest);
                }
            }
        }

        // Неверное название функции. Что возвращается не соответствует названию функции
        // никаких сокращений пожалуйста
        static void GetResultFromFile(string dirName)
        {
            Console.WriteLine();
            Console.WriteLine($"| {"ФИО",40} | {"Кол-во правильных ответов",30} | {"Диагноз",15} |");
            using (StreamReader sr = new StreamReader(dirName))
            {
                string line = "";
                while (true)
                {
                    line = sr.ReadLine();
                    if (line != null)
                    {
                        string[] line1 = line.Split(',');
                        Console.WriteLine($"| {line1[0],40} | {line1[1],30} | {line1[2],15} |");
                    }
                    else
                        break;
                }
            }
        }
    }
}
