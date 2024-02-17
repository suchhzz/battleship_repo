using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Battleship
{
    internal class Program
    {
        static void ShowAllPlayground(Playground pg, Gameplay gmpl) // вывод всего поля на экран
        {
            Console.Clear();
            for (int i = 0; i < gmpl.gameplay.GetLength(0); i++)
            {
                for (int j = 0; j < gmpl.gameplay.GetLength(1); j++)
                {
                    Console.Write(gmpl.gameplay[i, j] + " ");
                }
                Console.WriteLine();
            }

            Console.WriteLine("\n");

            for (int i = 0; i < pg.playground.GetLength(0); i++)
            {
                for(int j = 0; j  < pg.playground.GetLength(1); j++)
                {
                    Console.Write(pg.playground[i,j] + " ");
                }
                Console.WriteLine();
            }
        }

        static void mainRules() // правила
        {
            Console.WriteLine("\tRULES\n# - your ships\n@ - destroyed ships\n- - empty slot\n▄ - miss\n+ - enemy hit");
            Console.WriteLine("\n CONTROLS\nW - up\nS - down\nA - left\nD - right\n\nR - rotate\nENTER - place/shoot");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nREADY? (enter to start)");
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void Main(string[] args)
        {
            mainRules();          // выводим правила игры
            Thread.Sleep(2000);
            Console.ReadLine();

            Playground pg = new Playground();

           pg.ShipChoose();      // игрок располагает свои корабли на поле

            EnemyPlayground enemy = new EnemyPlayground();

           enemy.EnemyChoose();  // противник располагает свои корабли на поле

            Gameplay gameplay = new Gameplay();

            EnemyGameplay enemyGameplay = new EnemyGameplay();

            ShowAllPlayground(pg, gameplay); // вывод полей на экран

            bool playerWin = false;

            bool win = false;

            while (!win)
            {
                bool playerMove = true;

                while (playerMove) // ходы игрока
                {
                    playerMove = gameplay.Control(pg, enemy, enemyGameplay);

                    if (gameplay.shipsRemaining <= 0)
                    {
                        win = true;
                        playerWin = true;
                    }
                }

                if (win)
                {
                    break;
                }
                
                bool enemyMove = true;

                while (enemyMove) // ходы соперника
                {
                    enemyMove = enemyGameplay.enemyMove(pg, gameplay);

                    if (enemyGameplay.shipsRemaining == 0)
                    {
                        win = true;
                    }
                }

            }
            if (playerWin) // проверка победителя
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("YOU WON!!!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ENEMY WON!!!");
            }
           
        }
    }
}
