namespace Quiz
{
    internal class Program
    { //55
        static void Main()
        {
            Quiz.GenerateSampleQuestions(); // базовые вопросы

            Console.WriteLine("Хотите добавить свой вопрос? (да/нет)");
            string answer = Console.ReadLine().Trim().ToLower();

            if (answer == "да")
            {
                AddQuestionFromUser();
            }

            Quiz.RunQuiz();
        }

        static void AddQuestionFromUser()
        {
            Console.WriteLine("Введите текст вопроса:");
            string text = Console.ReadLine();

            List<string> options = new List<string>();

            // Пользователь вводит от 3 до 5 вариантов
            for (int i = 1; i <= 5; i++)
            {
                Console.WriteLine($"Введите вариант ответа #{i} (или нажмите Enter, чтобы остановиться):");
                string option = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(option))
                {
                    if (i < 4)
                    {
                        Console.WriteLine("Нужно минимум 3 варианта!");
                        i--; // повторно просим ввести
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

                options.Add(option);

                if (i >= 3 && i < 5)
                {
                    Console.WriteLine("Добавить ещё один вариант? (да/нет)");
                    string more = Console.ReadLine().Trim().ToLower();
                    if (more != "да")
                        break;
                }
            }

            Console.WriteLine("Введите номер правильного ответа (с 1):");
            int correctIndex;
            while (!int.TryParse(Console.ReadLine(), out correctIndex) || correctIndex < 1 || correctIndex > options.Count)
            {
                Console.WriteLine("Некорректный номер. Введите снова:");
            }

            Console.WriteLine("Выберите сложность вопроса: 1 — Лёгкий, 2 — Сложный");
            int typeChoice;
            while (!int.TryParse(Console.ReadLine(), out typeChoice) || (typeChoice != 1 && typeChoice != 2))
            {
                Console.WriteLine("Введите 1 или 2:");
            }

            QuestionType type = typeChoice == 1 ? QuestionType.Easy : QuestionType.Hard;

            Question q = new Question(text, options, correctIndex - 1, type);
            Quiz.AddQuestion(q);

            Console.WriteLine("Вопрос добавлен!");
        }
    }

    
}
