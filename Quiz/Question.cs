using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz
{
    public class Question
    {
        public string Text { get; set; }
        public List<string> Answers { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public QuestionType Type { get; set; }

        public Question(string text, List<string> answers, int correctAnswerIndex, QuestionType type)
        {
            if (answers.Count < 3 || answers.Count > 5)
                throw new ArgumentException("Количество вариантов ответа должно быть от 3 до 5.");

            if (correctAnswerIndex < 0 || correctAnswerIndex >= answers.Count)
                throw new ArgumentException("Неверный индекс правильного ответа.");

            Text = text;
            Answers = answers;
            CorrectAnswerIndex = correctAnswerIndex;
            Type = type;
        }
    }
}
