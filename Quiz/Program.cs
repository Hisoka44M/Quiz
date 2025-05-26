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

    enum QuestionType
    {
        Easy,
        Hard
    }

    class Question
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

    static class Quiz
    {
        private static List<Question> _questions = new List<Question>();
        private static Random _random = new Random();

        public static void GenerateSampleQuestions()
        {
            _questions.Add(new Question("Сколько будет 2 + 2?", new List<string> { "3", "4", "5" }, 1, QuestionType.Easy));
            _questions.Add(new Question("Столица Франции?", new List<string> { "Берлин", "Париж", "Рим" }, 1, QuestionType.Easy));
            _questions.Add(new Question("Кто написал '1984'?", new List<string> { "Оруэлл", "Достоевский", "Хемингуэй" }, 0, QuestionType.Hard));
            _questions.Add(new Question("Корень из 144?", new List<string> { "10", "12", "14" }, 1, QuestionType.Easy));
            _questions.Add(new Question("Интеграл x dx?", new List<string> { "x", "x^2 / 2", "ln(x)" }, 1, QuestionType.Hard));
            _questions.Add(new Question("5 * 6 = ?", new List<string> { "25", "30", "35" }, 1, QuestionType.Easy));
            _questions.Add(new Question("Глубочайший океан?", new List<string> { "Атлантический", "Тихий", "Индийский" }, 1, QuestionType.Hard));
            _questions.Add(new Question("Солнце встаёт на...?", new List<string> { "Западе", "Севере", "Востоке" }, 2, QuestionType.Easy));
            _questions.Add(new Question("Скорость света?", new List<string> { "150 000", "300 000", "1 000 000" }, 1, QuestionType.Hard));
            _questions.Add(new Question("Формула воды?", new List<string> { "H2O", "CO2", "O2" }, 0, QuestionType.Easy));
        }

        public static void AddQuestion(Question question)
        {
            _questions.Add(question);
        }

        public static void UpdateCorrectAnswer(int questionIndex, int newCorrectIndex)
        {
            if (questionIndex >= 0 && questionIndex < _questions.Count)
            {
                Question q = _questions[questionIndex];
                if (newCorrectIndex >= 0 && newCorrectIndex < q.Answers.Count)
                {
                    q.CorrectAnswerIndex = newCorrectIndex;
                }
            }
        }

        public static void RunQuiz()
        {
            List<Question> easyQuestions = new List<Question>(); // сюда попадут только лёгкие
            List<Question> hardQuestions = new List<Question>(); // сюда — только сложные

            // Проходимся по всем вопросам и сортируем вручную по типу
            foreach (Question q in _questions)
            {
                if (q.Type == QuestionType.Easy)
                    easyQuestions.Add(q);
                else if (q.Type == QuestionType.Hard)
                    hardQuestions.Add(q);
            }

            // Перемешиваем каждый список отдельно
            Shuffle(easyQuestions);
            Shuffle(hardQuestions);

            // Берём только 7 лёгких и 3 сложных
            List<Question> selectedQuestions = new List<Question>();
            for (int i = 0; i < 7 && i < easyQuestions.Count; i++)
                selectedQuestions.Add(easyQuestions[i]);

            for (int i = 0; i < 3 && i < hardQuestions.Count; i++)
                selectedQuestions.Add(hardQuestions[i]);

            // Перемешиваем финальный список вопросов
            Shuffle(selectedQuestions);

            int score = 0;

            // Запускаем саму викторину
            foreach (Question q in selectedQuestions)
            {
                Console.Clear();

                // Устанавливаем цвет в зависимости от сложности
                Console.ForegroundColor = q.Type == QuestionType.Easy ? ConsoleColor.Green : ConsoleColor.Red;

                // Показываем вопрос
                Console.WriteLine(q.Text);

                // Показываем варианты
                for (int i = 0; i < q.Answers.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {q.Answers[i]}");
                }

                Console.ResetColor();

                int userAnswer;
                // Проверяем корректность ввода пользователя
                while (!int.TryParse(Console.ReadLine(), out userAnswer) || userAnswer < 1 || userAnswer > q.Answers.Count)
                {
                    Console.WriteLine("Введите номер ответа от 1 до " + q.Answers.Count);
                }

                // Если ответ правильный — начисляем баллы
                if (userAnswer - 1 == q.CorrectAnswerIndex)
                {
                    score += q.Type == QuestionType.Easy ? 1 : 5;
                }
            }

            // Показываем финальный результат
            Console.Clear();
            Console.WriteLine($"Вы завершили викторину! Набрано баллов: {score}");
        }

        // Метод перемешивания списка вручную (аналог .OrderBy(...))
        private static void Shuffle(List<Question> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int j = _random.Next(i, list.Count);
                Question temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }
    }
}
