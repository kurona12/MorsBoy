using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace morsbou
{
    class Program
    {
        static char[,] playerField = new char[10, 10];
        static char[,] botField = new char[10, 10];
        static Random random = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine("Морской бой против компьютера");
            while (true)
            {
                Console.WriteLine("Выберите опцию:");
                Console.WriteLine("1 - Начать игру");
                Console.WriteLine("2 - Выход");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        InitializeFields();
                        PlacePlayerShips();
                        PlaceBotShips();
                        PlayGame();
                        break;
                    case "2":
                        Console.WriteLine("Спасибо за игру! До свидания.");
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Пожалуйста, попробуйте снова.");
                        break;
                }
            }
        }

        static void InitializeFields()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    playerField[i, j] = 'O'; // Открытая клетка
                    botField[i, j] = 'O';    // Открытая клетка
                }
            }
        }

        static void PlacePlayerShips()
        {
            Console.WriteLine("Расставьте свои корабли! У вас есть:");
            Console.WriteLine("1 - 4-палубный (1)");
            Console.WriteLine("2 - 3-палубных (2)");
            Console.WriteLine("3 - 2-палубных (3)");
            Console.WriteLine("4 - 1-палубных (4)");

            PlaceShipForPlayer(1, 4); // Одby 4-палубный
            PlaceShipForPlayer(2, 3); // Два 3-палубных
            PlaceShipForPlayer(3, 2); // Три 2-палубных
            PlaceShipForPlayer(4, 1); // Четыре 1-палубных
        }

        static void PlaceShipForPlayer(int count, int size)
        {
            for (int i = 0; i < count; i++)
            {
                bool placed = false;
                while (!placed)
                {
                    Console.Clear();
                    DisplayField(playerField);
                    Console.WriteLine($"Введите координаты для расстановки {size}-палубного корабля (например, A0, или J9):");
                    string input = Console.ReadLine().ToUpper();

                    int row, col;
                    if (input.Length >= 2 &&
                        char.IsLetter(input[0]) &&
                        char.IsDigit(input[1]) &&
                        input[0] >= 'A' && input[0] <= 'J' &&
                        input[1] >= '0' && input[1] <= '9' &&
                        (input.Length == 2 || input.Length == 3 && input[2] == '0'))
                    {
                        col = input[0] - 'A';
                        row = int.Parse(input[1].ToString());

                        bool horizontal = false;

                        Console.WriteLine("Введите 1 для горизонтального или 2 для вертикального размещения:");
                        char orientation = Console.ReadLine()[0];

                        if (orientation == '1')
                            horizontal = true;
                        else if (orientation == '2')
                            horizontal = false;
                        else
                        {
                            Console.WriteLine("Неверная ориентация! Попробуйте снова.");
                            continue;
                        }

                        if (CanPlaceShip(row, col, size, horizontal, playerField))
                        {
                            for (int j = 0; j < size; j++)
                            {
                                if (horizontal)
                                    playerField[row, col + j] = '%';
                                else
                                    playerField[row + j, col] = '%';
                            }
                            placed = true;
                        }
                        else
                        {
                            Console.WriteLine("Не удалось разместить корабль. Пожалуйста, попробуйте снова.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Неверные координаты! Попробуйте снова.");
                    }
                }
            }
        }

        static bool CanPlaceShip(int row, int col, int size, bool horizontal, char[,] field)
        {
            if (horizontal)
            {
                if (col + size > 10) return false;
                for (int j = 0; j < size; j++)
                {
                    if (field[row, col + j] != 'O') return false;
                }
                for (int j = -1; j <= size; j++)
                {
                    if (col + j >= 0 && col + j < 10)
                    {
                        if (row > 0 && field[row - 1, col + j] == '%') return false;
                        if (row < 9 && field[row + 1, col + j] == '%') return false;
                    }
                }
                if (col > 0 && field[row, col - 1] == '%') return false;
                if (col + size < 10 && field[row, col + size] == '%') return false;
            }
            else
            {
                if (row + size > 10) return false;
                for (int j = 0; j < size; j++)
                {
                    if (field[row + j, col] != 'O') return false;
                }
                for (int j = -1; j <= size; j++)
                {
                    if (row + j >= 0 && row + j < 10)
                    {
                        if (col > 0 && field[row + j, col - 1] == '%') return false;
                        if (col < 9 && field[row + j, col + 1] == '%') return false;
                    }
                }
                if (row > 0 && field[row - 1, col] == '%') return false;
                if (row + size < 10 && field[row + size, col] == '%') return false;
            }

            return true;
        }

        static void PlaceBotShips()
        {
            PlaceShip(1, 4);
            PlaceShip(2, 3);
            PlaceShip(3, 2);
            PlaceShip(4, 1);
        }

        static void PlaceShip(int count, int size)
        {
            for (int i = 0; i < count; i++)
            {
                bool placed = false;
                while (!placed)
                {
                    int row = random.Next(0, 10);
                    int col = random.Next(0, 10);
                    bool horizontal = random.Next(0, 2) == 0;

                    if (CanPlaceShip(row, col, size, horizontal, botField))
                    {
                        for (int j = 0; j < size; j++)
                        {
                            if (horizontal)
                                botField[row, col + j] = '%';
                            else
                                botField[row + j, col] = '%';
                        }
                        placed = true;
                    }
                }
            }
        }

        static void PlayGame()
        {
            bool gameContinues = true;
            while (gameContinues)
            {
                Console.Clear();
                DisplayFields();
                Console.WriteLine("Ваш ход! Введите координаты для выстрела (например, A0):");
                string input = Console.ReadLine().ToUpper();

                int row, col;

                if (input.Length == 2 &&
                    char.IsLetter(input[0]) && char.IsDigit(input[1]) &&
                    input[0] >= 'A' && input[0] <= 'J' &&
                    input[1] >= '0' && input[1] <= '9')
                {
                    col = input[0] - 'A';
                    row = int.Parse(input[1].ToString());
                }
                else
                {
                    Console.WriteLine("Неверные координаты! Попробуйте снова.");
                    continue;
                }

                if (botField[row, col] == '%')
                {
                    botField[row, col] = 'X';
                    Console.WriteLine("Попадание!");
                }
                else if (botField[row, col] == 'O')
                {
                    botField[row, col] = '#';
                    Console.WriteLine("Промах!");
                }
                else
                {
                    Console.WriteLine("Вы уже стреляли в эту клетку! Попробуйте снова.");
                    continue;
                }

                if (CheckVictory(botField))
                {
                    Console.WriteLine("Вы победили!");
                    gameContinues = false;
                }
                else
                {
                    BotTurn();
                    if (CheckVictory(playerField))
                    {
                        Console.WriteLine("Компьютер победил!");
                        gameContinues = false;
                    }
                }
            }
            Console.WriteLine("Игра окончена. Нажмите любую клавишу, чтобы выйти.");
            Console.ReadKey();
        }

        static void BotTurn()
        {
            bool shot = false;
            while (!shot)
            {
                int row = random.Next(0, 10);
                int col = random.Next(0, 10);
                if (playerField[row, col] != 'X' && playerField[row, col] != '#') // проверяем уже ли стреляли
                {
                    if (playerField[row, col] == '%')
                    {
                        playerField[row, col] = 'X';
                    }
                    else
                    {
                        playerField[row, col] = '#'; // Заменено с 'M' на '#'
                    }
                    shot = true;
                }
            }
        }

        static void DisplayFields()
        {
            Console.WriteLine("Поле компьютера:         Ваше поле:");
            Console.WriteLine("   A B C D E F G H I J      A B C D E F G H I J");
            for (int i = 0; i < 10; i++)
            {
                Console.Write($"{i} ");
                for (int j = 0; j < 10; j++)
                {
                    if (botField[i, j] == '%')
                        Console.Write("O ");
                    else
                        Console.Write(botField[i, j] + " ");
                }
                Console.Write("    ");
                Console.Write($"{i} ");
                for (int j = 0; j < 10; j++)
                {
                    Console.Write(playerField[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        static void DisplayField(char[,] field)
        {
            Console.WriteLine("Текущее состояние вашего поля:");
            Console.WriteLine("  A B C D E F G H I J");
            for (int i = 0; i < 10; i++)
            {
                Console.Write($"{i} ");
                for (int j = 0; j < 10; j++)
                {
                    Console.Write(field[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        static bool CheckVictory(char[,] field)
        {
            foreach (char cell in field)
            {
                if (cell == '%') return false;
            }
            return true;
        }
    }
}