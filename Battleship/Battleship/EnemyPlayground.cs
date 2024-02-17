using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    internal class EnemyPlayground  // поле противника, в котором он сам решает как расположить корабли
    {
        public EnemyPlayground() // создаем поле 10 на 10, заполняем пустыми клетками, выводим на экран
        {
            enemyPlayground = new char[10, 10];
            FillPlayground();
            ShowPlayground();
        }

        private void FillPlayground()
        {
            for (int i = 0; i < enemyPlayground.GetLength(0); i++)
            {
                for (int j = 0; j < enemyPlayground.GetLength(1); j++)
                {
                    enemyPlayground[i, j] = '-';
                }
            }
        }
        public void ShowPlayground()
        {
            Console.Clear();
            for (int i = 0; i < enemyPlayground.GetLength(0); i++)
            {
                for (int j = 0; j < enemyPlayground.GetLength(1); j++)
                {
                    Console.Write(enemyPlayground[i,j] + " ");
                }
                Console.WriteLine();
            }
        }

        public class Ship  // класс корабля, имеющий в себе тип (1-4 палубные)
        {
            public int type { get; set; }
            public Ship(int type = 0)
            {
                this.type = type;
            }
        }

        public Ship[] ships = { // массив кораблей с количеством x-палубных кораблей
            new Ship(4),

            new Ship(3),
            new Ship(3),

            new Ship(2),
            new Ship(2),
            new Ship(2),

            new Ship(1),
            new Ship(1),
            new Ship(1),
            new Ship(1) };

        private int _Xpos = 0;
        private int _Ypos = 0;

        public char[,] enemyPlayground ;

        private Random rnd = new Random();

        private void setPos(Ship ship, bool reverse) // метод расположения кораблей рандомным способом
        {
            bool check = false;

            reverse = false;       // булевая переменная, обозначающая перевернутый корабль (TRUE - перевернутый | FALSE - не перевернутый)

            int reverseRand = rnd.Next(0, 2); // рандом, определяющий будет ли корабль перевернутый

            if (reverseRand == 1)              
            {
                reverse = true;
            }

            while (!check)
            {

                if (!reverse)         // для не перевернутого корабля
                {
                    _Xpos = rnd.Next(0, enemyPlayground.GetLength(0) - ship.type);  // рандомно генерируем X, Y в пределах поля
                    _Ypos = rnd.Next(0, enemyPlayground.GetLength(0));


                    if (BorderCheck(ship, _Xpos, _Ypos, reverse))     // проверяем, касается ли корабль других кораблей
                    {
                        int end = _Xpos + ship.type;                  // конечная точка САМОГО КОРАБЛЯ

                        int current = _Xpos;                          // текущая точка Х, которой будем итерироваться

                        while (current != end)
                        {
                            enemyPlayground[_Ypos, current] = '#';    // располагаем корабль N-го типа в его пределах
                            current++;                               // Xpos в данном случае начальная точка корабля (левая крайняя) 
                        }                                            // end - конечная точка корабля (правая крайняя)
                        check = true; 
                    }
                }
                else                // для перевернутого корабля
                {

                    _Xpos = rnd.Next(0, enemyPlayground.GetLength(0));
                    _Ypos = rnd.Next(0, enemyPlayground.GetLength(0) - ship.type);

                    if (BorderCheck(ship, _Xpos, _Ypos, reverse))
                    {
                        int end = _Ypos + ship.type;

                        int current = _Ypos;

                        while (current != end)
                        {
                            enemyPlayground[current, _Xpos] = '#';
                            current++;
                        }
                        check = true;
                    }
                }
            }
        }

        private bool BorderCheck(Ship ship, int x, int y, bool reverse) // метод, проверяющий можно ли поставить корабль в данное место
        {                                                               // если корабль пересекается с другим кораблем - возвращаем FALSE
            ///////////////      правая нижняя точка границы корабля
            int secondPointX;
            int secondPointY;
            if (reverse)
            {
                secondPointX = x + 1;
                secondPointY = y + ship.type;
            }
            else
            {
                secondPointX = x + ship.type;
                secondPointY = y + 1;
            }
            /////////////////

            /////////////////         левая верхняя точка границы корабля
                int firstPointX = x - 1;
                int firstPointY = y - 1;
            /////////////////
               
                
            // поправляем точки если они выходят за границы поля
                if (firstPointX < 0)                                 
                {
                    firstPointX++;
                }
                if (firstPointY < 0)
                {
                    firstPointY++;
                }
                if (secondPointX > enemyPlayground.GetLength(0) - 1)
                {
                    secondPointX--;
                }
                if (secondPointY > enemyPlayground.GetLength(0) - 1)
                {
                    secondPointY--;
                }


                for (int i = firstPointY; i <= secondPointY; i++)    // цикл смотрит есть ли другой корабль в выбранной территории
                {
                    for (int j = firstPointX; j <= secondPointX; j++)
                    {
                        if (enemyPlayground[i,j] == '#')
                        {
                            return false;
                        }
                    }
                }
            
            
            return true;
        }

        bool reverse = false;

        public void EnemyChoose() // метод выбора места для корабля используя массив кораблей
        {
            for (int i = 0; i < ships.Length; i++)
            {
                setPos(ships[i], reverse);
            }
        }
    }
}
