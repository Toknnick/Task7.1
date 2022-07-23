using System;
using System.Collections.Generic;
using System.Linq;

namespace Task7._1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DatabaseOfJail databaseOfJail = new DatabaseOfJail();
            databaseOfJail.Work();
        }
    }

    class DatabaseOfJail
    {
        public void Work()
        {
            Jail jail = new Jail();
            bool isWork = true;
            bool isShowPrisonersByCertainParameters = false;
            Console.WriteLine("База данных тюрьмы.");

            while (isWork)
            {
                Console.WriteLine("1.Вывести всех заключенных. \n2.Вывести определенных заключенных. \n3.Выход. \nВыберите вариант:");
                string userInput = Console.ReadLine();
                Console.Clear();

                switch (userInput)
                {
                    case "1":
                        jail.ShowAllPrisioners();
                        break;
                    case "2":
                        jail.ShowPrisonersByCertainParameters(ref isShowPrisonersByCertainParameters);
                        break;
                    case "3":
                        isWork = false;
                        break;
                }

                WriteMessage();
            }
        }

        private void WriteMessage()
        {
            Console.WriteLine("Для продолжения нажмите любую клавишу:");
            Console.ReadKey();
            Console.Clear();
        }

    }

    class Jail
    {
        private List<Prisioner> _prisioners = new List<Prisioner>();

        public Jail()
        {
            AddPrisioners();
        }

        public void ShowAllPrisioners()
        {
            if (_prisioners.Count > 0)
            {
                foreach (var prisioner in _prisioners)
                {
                    prisioner.ShowInfo();
                }
            }
            else
            {
                WriteError("Заключенных с такими параметрами не существуют");
            }
        }

        public void ShowPrisonersByCertainParameters(ref bool isShowPrisonersByCertainParameters)
        {
            string userInput = "";
            IEnumerable<Prisioner> prisonersByCertainParameters;

            if (IsChoosedParameters(ref userInput, out int growth, out int weight, out NationalitiesOfPrisioners nationalityOfPrisioner, isShowPrisonersByCertainParameters))
            {
                isShowPrisonersByCertainParameters = true;
                Console.Clear();

                if (userInput == "true")
                {
                    prisonersByCertainParameters = _prisioners.Where(prisioner => prisioner.Growth >= growth).
                                                               Where(prisioner => prisioner.Weight >= weight).Where((prisioner => prisioner.Nationality == nationalityOfPrisioner));
                }
                else
                {
                    prisonersByCertainParameters = _prisioners.Where(prisioner => prisioner.Growth == growth).
                                                               Where(prisioner => prisioner.Weight == weight).Where((prisioner => prisioner.Nationality == nationalityOfPrisioner));
                }

                _prisioners = prisonersByCertainParameters.ToList();

                if (_prisioners.Count <= 0)
                {
                    WriteError("Заключенных с такими параметрами не существуют");
                }

                foreach (Prisioner prisioner in prisonersByCertainParameters)
                {
                    prisioner.ShowInfo();
                }
            }
        }

        private bool IsChoosedParameters(ref string userInput, out int growth, out int weight, out NationalitiesOfPrisioners nationalityOfPrisioner, bool isShowPrisonersByCertainParameters)
        {
            growth = 0;
            weight = 0;
            nationalityOfPrisioner = default;
            bool isChoosedParameters = false;

            if (isShowPrisonersByCertainParameters == false)
            {
                Console.WriteLine("Введите рост заключенного: ");

                if (int.TryParse(Console.ReadLine(), out growth))
                {
                    Console.Clear();
                    Console.WriteLine("Введите вес заключенного: ");

                    if (int.TryParse(Console.ReadLine(), out weight))
                    {
                        Console.Clear();
                        Console.WriteLine("Введите номер национальности заключенного:");
                        ShowNationalitiesOfPrisioners();

                        if (int.TryParse(Console.ReadLine(), out int numberOfNationalityOfPrisioner))
                        {
                            if (numberOfNationalityOfPrisioner < Enum.GetNames(typeof(NationalitiesOfPrisioners)).Length && numberOfNationalityOfPrisioner > 0)
                            {
                                Console.Clear();
                                Console.WriteLine("Будут ли выводиться заключенные больше данных параметров? \ntrue / false");
                                userInput = Console.ReadLine();
                                numberOfNationalityOfPrisioner -= 1;
                                nationalityOfPrisioner = (NationalitiesOfPrisioners)numberOfNationalityOfPrisioner;
                                isChoosedParameters = true;
                            }
                            else
                            {
                                WriteError();
                            }
                        }
                        else
                        {
                            WriteError();
                        }
                    }
                    else
                    {
                        WriteError();
                    }
                }
                else
                {
                    WriteError();
                }
            }
            else
            {
                WriteError("Вы уже выбрали параметры для поиска!");
                ShowAllPrisioners();
            }

            return isChoosedParameters;
        }

        private void ShowNationalitiesOfPrisioners()
        {
            for (int i = 0; i < Enum.GetValues(typeof(NationalitiesOfPrisioners)).Length; i++)
            {
                Console.WriteLine($"{i + 1}.{(NationalitiesOfPrisioners)i}");
            }
        }

        private void AddPrisioners()
        {
            Random random = new Random();
            int minCountOfPrisioners = 10;
            int maxCountOfPrisioners = 20;
            int minValueOfGrowth = 120;
            int maxValueOfGrowth = 200;
            int minValueOfWeight = 40;
            int maxValueOfWeight = 100;
            int CountOfPrisioners = random.Next(minCountOfPrisioners, maxCountOfPrisioners);

            for (int i = 0; i < CountOfPrisioners; i++)
            {
                int growth = random.Next(minValueOfGrowth, maxValueOfGrowth);
                int weight = random.Next(minValueOfWeight, maxValueOfWeight);
                NationalitiesOfPrisioners nationality = (NationalitiesOfPrisioners)random.Next(Enum.GetNames(typeof(NationalitiesOfPrisioners)).Length);
                _prisioners.Add(new Prisioner((NamesOfPrisioners)i, growth, weight, nationality));
            }
        }

        private void WriteError(string text = "Данные некорректные! Попробуйте еще раз.")
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ForegroundColor = defaultColor;
        }
    }

    class Prisioner
    {
        public NamesOfPrisioners Name { get; private set; }
        public int Growth { get; private set; }
        public int Weight { get; private set; }
        public NationalitiesOfPrisioners Nationality { get; private set; }

        public Prisioner(NamesOfPrisioners name, int growth, int weight, NationalitiesOfPrisioners nationality)
        {
            Name = name;
            Growth = growth;
            Weight = weight;
            Nationality = nationality;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Имя: {Name}. Рост: {Growth}. Вес: {Weight}. Национальность: {Nationality}.");
        }
    }

    enum NationalitiesOfPrisioners
    {
        English,
        Japanese,
        Italian,
        Russian,
        Spanish,
        Polish,
        Maxican,
        Turkish,
        American
    }

    enum NamesOfPrisioners
    {
        Jose,
        Carl,
        John,
        James,
        Randy,
        Scott,
        Shawn,
        Keith,
        Curtis,
        Rafael,
        George,
        Martin,
        Robert,
        Michae,
        Justin,
        Thomas,
        Matthew,
        Raymond,
        Patrick,
        Clarence
    }
}
