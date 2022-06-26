using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWorkAutoServes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Serves serves = new Serves();
            bool isWork = true;
            Console.WriteLine("\nУправление сервисом специализация которого продажа и установка шин.\n" +
                "Ваш слоган у нас есть все шины, а если нету то мы вам заплатим!");

            while (isWork)
            {
                Console.WriteLine($"Баланс сервиса {serves.Money}");
                Console.WriteLine(" 1 - Принять нового клиента. 2 - Шины а наличии. 3 - Закрыть программу.");
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        serves.ServeClient();
                        break;

                    case "2":
                        serves.ShowInfoCells();
                        break;

                    case "3":
                        isWork = false;
                        break;
                }
            }
        }
    }

    class Serves
    {
        private List<Cell> _cells = new List<Cell>();
        private int _money = 1000;

        public int Money => _money;

        public Serves()
        {
            FillCells();
        }

        public void ServeClient()
        {
            bool isDone = false;
            string name = "";
            int numberTire = 0;
            int discDiameter = 0;
            int width = 0;
            int profile = 0;
            int numberValidInputs = 4;
            string userInput = "";

            while (isDone == false)
            {
                int validInput = 0;

                Console.WriteLine("\nВнесите требуемые параметры клиента.");
                Console.WriteLine("Шины какой фирмы нужны клиенту");
                name = Console.ReadLine();
                Console.WriteLine("Какaя размерность нужна клиенту");
                userInput = Console.ReadLine();

                if (int.TryParse(userInput, out int discDiameterInput))
                {
                    discDiameter = discDiameterInput;
                    validInput++;
                }

                Console.WriteLine("Какая ширина нужна клиенту");
                userInput = Console.ReadLine();

                if (int.TryParse(userInput, out int widthInput))
                {
                    width = widthInput;
                    validInput++;
                }

                Console.WriteLine("Какая высота шины нужна клиенту");
                userInput = Console.ReadLine();

                if (int.TryParse(userInput, out int profileInput))
                {
                    profile = profileInput;
                    validInput++;
                }

                Console.WriteLine("Какое количество шин требуется.");
                userInput = Console.ReadLine();

                if (int.TryParse(userInput, out int number))
                {
                    numberTire = number;
                    validInput++;
                }


                if (validInput == numberValidInputs)
                {
                    isDone = true;
                }
                else
                {
                    Console.WriteLine("Данные введены некорректно.");
                }
            }

            Tire tireClient = new Tire(name, width, profile, discDiameter, 0);
            int indexTire = SearchTire(tireClient, numberTire);

            if (indexTire > -1)
            {
                CompleteDeal(indexTire, numberTire);
            }
            else
            {
                PayFine();
            }
        }

        public void ShowInfoCells()
        {
            foreach (Cell cell in _cells)
            {
                cell.Tire.ShowInfoTire();
                Console.Write($" : В наличии {cell.Number} штук");
            }
        }

        private void CompleteDeal(int indexTire, int numberTire)
        {
            Console.WriteLine("Есть в наличии");
            CalculateProfit(indexTire, numberTire);
            DeleteTire(indexTire, numberTire);
        }

        private void CalculateProfit(int indexTire, int numberTire)
        {
            int price = GetPrice(indexTire);
            int priceAll = price * numberTire;
            Console.WriteLine($"Клиенту к оплате {priceAll}");
            _money += priceAll;
        }

        private int GetPrice(int indexTire)
        {            
            Tire tireTemp = _cells[indexTire].Tire;
            return tireTemp.Price;
        }

        private void DeleteTire(int indexTire,int numberTire)
        {
            _cells[indexTire].DeleteNumberTire(numberTire); 
        }

        private void FillCells()
        {
            int defaultQuantity = 10;
            Tire tireOne = new Tire("Bridgestone", 195, 70, 14, 25);
            Tire tireTwo = new Tire("Michelin", 195, 70, 14, 25);           
            _cells.Add(new Cell(tireOne, defaultQuantity));
            _cells.Add(new Cell(tireTwo, defaultQuantity));
        }

        private int SearchTire(Tire tireClient, int numberClient)
        {
            int indexTire = 0;

            for (int i = 0; i < _cells.Count; i++)
            {
                if (_cells[i].Tire.Name == tireClient.Name && _cells[i].Tire.Width == tireClient.Width && _cells[i].Tire.Profile == tireClient.Profile && _cells[i].Tire.DiscDiameter == tireClient.DiscDiameter)
                {
                    if(_cells[i].Number >= numberClient)
                    {
                        indexTire = i;
                        return indexTire;
                    }
                }
            }

            indexTire = -1;
            return indexTire;
        }

        private void PayFine()
        {
            int fine = 100;
            Console.WriteLine(" Такой шины нету в наличии и сервис выплачивает штраф");
            _money -= fine;
        }
    }

    class Cell
    {
        public int Number { get; private set; }
        public Tire Tire { get; private set; }

        public Cell(Tire tire, int number)
        {
            Tire = tire;
            Number = number;
        }

        public void DeleteNumberTire(int numberDelete)
        {
            Number -= numberDelete;
        }
    }    

    class Tire
    {
        public string Name { get; private set; }
        public int DiscDiameter { get; private set; }
        public int Width { get; private set; }
        public int Profile { get; private set; }
        public int Price { get; private set; }

        public Tire(string name, int width, int profile, int discDiameter, int price)
        {
            Name = name;
            Width = width;
            Profile = profile;
            DiscDiameter = discDiameter;
            Price = price;
        }

        public void ShowInfoTire()
        {
            Console.Write($"\nШина {Name}: Размеры {Width}/{Profile}/{DiscDiameter} : Цена за штуку {Price}");
        }
    }
}
