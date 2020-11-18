using System;
using System.IO;

namespace GDI_Task_05
{
    class Program
    {

        static string[] newFile = File.ReadAllLines($"maps/level02.txt");//Файл с лабиринтом
        static char[,] map = new char[newFile.Length, newFile[0].Length];//Массив символов/лабиринт

        static int HeroX;//координаты Игрока
        static int HeroY;

        static int E_enX;//Координаты врага Е
        static int E_enY;

        static int V_enX;//Координаты врага V
        static int V_enY;

        static int O_enX;//Координаты врага О
        static int O_enY;

        static int Fx;//Координаты финиша
        static int Fy;


        static ConsoleKeyInfo key;
        static Random rnd = new Random();
        static int Helth = 10;//Жизни
        static bool mist = false;//Проверка на попытку войти в стену

        static char[,] ReadMap()//Чтение лабиринта в массив
        {
            HeroX = 0;
            HeroY = 0;

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = newFile[i][j];

                    if (map[i, j] == '♠')
                    {
                        HeroX = i;
                        HeroY = j;
                    }else if (map[i, j] == 'F')//Сохраняем координаты игрока, врагов и финиша
                    {
                        Fx = i;
                        Fy = j;
                    }
                    else if (map[i, j] == 'E')
                    {
                        E_enX = i;
                        E_enY = j;
                    }
                    else if (map[i, j] == 'V')
                    {
                        V_enX = i;
                        V_enY = j;
                    }
                    else if (map[i, j] == 'O')
                    {
                        O_enX = i;
                        O_enY = j;
                    }

                }
            }
            return map;
        }


        static void DrawMap(char[,] map)//Функция вывода лабиринта в консоль
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }
        }

        static void En_way(char n, ref int x, ref int y)//Расчёт хода врагов
        {
            int w;

        Sicl:
            w = rnd.Next(1, 4);
            if (x == 1) w = 4;
            if (x == map.GetLength(0) - 1) w = 2;
            switch (w) {
                case 1:
                    if (map[x, y - 1] != '█')
                    {
                        map[x, y - 1] = n;
                        map[x, y] = ' ';
                        y--;
                    }
                    else goto Sicl;
                        break;

                case 2:
                    if (map[x-1, y] != '█')
                    {
                        map[x-1, y] = n;
                        map[x, y] = ' ';
                        x--;
                    }
                    else goto Sicl;
                    break;

                case 3:
                    if (map[x, y+1] != '█')
                    {
                        map[x, y+1] = n;
                        map[x, y] = ' ';
                        y++;
                    }
                    else goto Sicl;
                    break;

                case 4:
                    if (map[x+1, y] != '█')
                    {
                        map[x+1, y] = n;
                        map[x, y] = ' ';
                        x++;
                    }
                    else goto Sicl;
                    break;
            }Console.WriteLine(Convert.ToString(V_enX));
        }

        static void HP_Mistakes() //Правила здоровье и ошибки
        {
            Console.Write("HP:[");
            for(int i = 0; i < Helth; i++)
            {
                Console.Write('#');
            }
            if(Helth < 10)
            {
                for(int i = 0; i < 10 - Helth; i++)
                {
                    Console.Write('_');
                }
            }
            Console.Write("]\n");
            Console.WriteLine("E,V и O - враги, опасайтесь их.");
            Console.WriteLine("F - Выход из лабиринта.");
            Console.WriteLine("♠ - Вы.");
            Console.WriteLine("Удачи)");
            if (mist == true)
            {
                Console.WriteLine("Тут стена, сюда нельзя!\n");
                mist = false;
            }
            else Console.Write("\n\n");
        }

        static void Walk()
        {
            do
            {
                HP_Mistakes();
                DrawMap(map);

                key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.LeftArrow)//Чтение стрелочек
                {
                    if ((map[HeroX, HeroY - 1] != '█') && (HeroY != 0))
                    {
                        map[HeroX, HeroY - 1] = '♠';
                        map[HeroX, HeroY] = '*';
                        HeroY--;
                    }
                    else mist = true;
                }
                else if (key.Key == ConsoleKey.RightArrow)
                {
                    if ((map[HeroX, HeroY + 1] != '█') && (HeroY != map.GetLength(1)))
                    {
                        map[HeroX, HeroY + 1] = '♠';
                        map[HeroX, HeroY] = '*';
                        HeroY++;
                    }
                    else mist = true;
                }
                else if (key.Key == ConsoleKey.UpArrow)
                {
                    if ((map[HeroX - 1, HeroY] != '█') && (HeroX != 0))
                    {
                        map[HeroX - 1, HeroY] = '♠';
                        map[HeroX, HeroY] = '*';
                        HeroX--;
                    }
                    else mist = true;
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    if ((map[HeroX + 1, HeroY] != '█') && (HeroX != map.GetLength(0)))
                    {
                        map[HeroX + 1, HeroY] = '♠';
                        map[HeroX, HeroY] = '*';
                        HeroX++;
                    }
                    else mist = true;
                }
                if ((HeroX == E_enX) && (HeroY == E_enY) || (HeroX == V_enX) && (HeroY == V_enY) || (HeroX == O_enX) && (HeroY == V_enY)) Helth--;

                En_way('E', ref E_enX, ref E_enY);//Ход врагов
                En_way('V', ref V_enX, ref V_enY);
                En_way('O', ref O_enX, ref O_enY);

                if ((HeroX == E_enX) && (HeroY == E_enY) || (HeroX == V_enX) && (HeroY == V_enY) || (HeroX == O_enX) && (HeroY == V_enY)) Helth--;//проверка на урон от монстров

                Console.Clear();

            } while ((map[HeroX, HeroY] != map[Fx, Fy]) && (Helth > 0));

            if (map[HeroX, HeroY] == map[Fx, Fy])//победа
            {

                Console.WriteLine(@"╔╗──╔╗╔══╗╔═══╗╔════╗╔═══╗╔═══╗╔╗──╔╗
║╚╗╔╝║╚╣─╝║╔═╗║║╔╗╔╗║║╔═╗║║╔═╗║║╚╗╔╝║
╚╗║║╔╝─║║─║║─╚╝╚╝║║╚╝║║─║║║╚═╝║╚╗╚╝╔╝
─║╚╝║──║║─║║─╔╗──║║──║║─║║║╔╗╔╝─╚╗╔╝─
─╚╗╔╝─╔╣─╗║╚═╝║──║║──║╚═╝║║║║╚╗──║║──
──╚╝──╚══╝╚═══╝──╚╝──╚═══╝╚╝╚═╝──╚╝──");
                Console.WriteLine(@"______________¶¶¶¶¶¶¶¶¶¶¶
______________¶¶¶¶¶¶¶¶¶¶¶
______________¶¶111111¶¶¶___¶¶¶¶¶¶¶¶¶¶¶
______________¶¶111111¶¶¶___¶¶¶¶¶¶¶¶¶¶¶
______________¶¶111111¶¶¶___¶¶¶111111¶¶
______________¶¶111111¶¶¶___¶¶¶111111¶¶
______________¶¶111111¶¶¶___¶¶111111¶¶¶
______________¶¶¶111111¶¶__¶¶¶111111¶¶
______________¶¶¶111111¶¶__¶¶¶111111¶¶
______________¶¶¶111111¶¶__¶¶¶111111¶¶
______________¶¶¶111111¶¶__¶¶111111¶¶¶
______________¶¶¶111111¶¶__¶¶111111¶¶¶
______________¶¶¶¶11111¶¶__¶¶111111¶¶
¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶11111¶¶¶¶¶¶111111¶¶
¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶111111¶¶¶¶111111¶¶¶
¶¶¶11111¶¶111111¶¶111111¶¶¶¶111111¶¶¶
¶¶¶11111¶¶111111¶¶111111¶¶¶¶111111¶¶¶
¶¶¶11111¶¶111111¶¶111111¶¶¶¶111111¶¶
¶¶¶11111¶¶111111¶¶¶11111111111111¶¶¶¶
¶¶¶11111¶¶111111¶¶¶1111111111¶¶¶¶¶¶¶¶¶¶
¶¶¶11111¶¶111111¶¶¶11¶¶¶¶¶¶¶¶¶¶¶¶11111¶¶
¶¶¶11111¶¶111111¶¶¶¶¶¶1111111111111111¶¶¶
¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶1¶111111111111111111¶¶
¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶111¶¶¶¶11¶¶¶¶¶1111111¶¶¶
¶¶¶11111111111111111111¶¶¶¶¶¶¶1111111111¶¶¶
¶¶¶1111111111111111111111¶¶¶¶11111111111¶¶¶
¶¶¶111111111111111111111¶¶¶11111111111111¶¶¶
¶¶¶11111111111111111111¶¶¶111111111111111¶¶¶
¶¶¶11111111111111111111¶¶¶1111111111111111¶¶
¶¶¶¶1111111111111111111111111111111111111¶¶¶
__¶¶¶11111111111111111111111111111111111¶¶¶
___¶¶¶¶11111111111111111111111111111111¶¶¶
____¶¶¶¶11111111111111111111111111111¶¶¶¶
______¶¶¶111111111111111111111111111¶¶¶
_______¶¶¶¶11111111111111111111111¶¶¶¶
________¶¶¶¶111111111111111111111¶¶¶
__________¶¶¶1111111111111111111¶¶¶
___________¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶¶");
            }
            else
            {//поражение
                Console.WriteLine(@"▀▄░▄▀ ▄▀▄ █░█     █▀▄ █▀▀ ▄▀▄ █▀▄ 
░░█░░ █░█ █░█     █░█ █▀▀ █▀█ █░█ 
░░▀░░ ░▀░ ░▀░     ▀▀░ ▀▀▀ ▀░▀ ▀▀░");
                Console.WriteLine(@"████████████████████████████████████████
██▀░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░▀██
██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██
██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██
██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██
██░░▄█▀▀▀▄░░░░░░░░░░░░░░░░░░░███▀▀▀█▄░██
██▄█░▄▄░░▀██▄░░░░░░░░░░░░░░░█▀█░░██░▀███
████░▀▀░░░█░█░░░░░░░░░░░░░░░█░█▄░░░░░███
██▀█▄▄░░▄█▀█▀░░░░░░░░░░░░░░░▀█░▀█▄▄▄█▀██
██░░██▀▀▀▄██░░░▀█░░░░░░█▀░░░░▀▀▀░▀▀█░░██
██░▄▄▀▀▀██░░█░░░░░░░░░░░░░▄▄░░███░░░█░██
██░░▀█████▄▄█▄▄█▀█▄▄▄█▀█▄▄██░░░██▄░░▀███
██░░░░░░░░▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀█▄░░▀██░░░███
██░░░░░░░░░░░░░░░░░░░░░░░░░░█░░░██░░░███
██░░░░░░░░░░░░░░░░░░░░░░░░░░░█░░░▀░░░░██
██░░░░░░░░░░░░░░░░░░░░░░░░░░░██░░░░░░▄██
████████████████████████████████████████");
            }
        }


        static void Main(string[] args)
        {
            int origWidth, width;
            int origHeight, height;

            origWidth = Console.WindowWidth;//Настраиваем окно командной строки
            origHeight = Console.WindowHeight;

            width = origWidth / 2;
            height = origHeight * 2;

            Console.SetWindowSize(width, height);

            map = ReadMap();

            Walk();
            

            

        }
    }
}
