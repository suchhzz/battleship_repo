using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    internal class Gameplay // дает игроку выбрать точку для выстрела
    {                       // при попадании в корабль, обозначает его как "@", при промахе - "▄"
        public Gameplay()   // конструктор
        {
            gameplay = new char[10, 10]; // создает пустое поле, заполняет пустым значением ("-") 
            FillPlayground();            // в дальнейшем сравнивает точки попадания с полем противника и обозначает промах/попадание 
            ShowPlayground();            // на этом поле
        }
        private void FillPlayground()
        {
            for (int i = 0; i < gameplay.GetLength(0); i++)
            {
                for (int j = 0; j < gameplay.GetLength(1); j++)
                {
                    gameplay[i, j] = '-';
                }
            }
        }
        private void ShowPlayground()
        {
            Console.Clear();
            for (int i = 0; i < gameplay.GetLength(0); i++)
            {
                for (int j = 0; j < gameplay.GetLength(1); j++)
                {
                    Console.Write(gameplay[i,j] + " ");
                }
                Console.WriteLine();
            }
        }

        private void ShowAllPlaygroundGameplay(Playground pg, EnemyGameplay enemy)
        {
            Console.Clear();
            for (int i = 0; i < gameplay.GetLength(0); i++)
            {
                for (int j = 0; j < gameplay.GetLength(1); j++)
                {
                    Console.Write(gameplay[i, j] + " ");
                }
                Console.WriteLine();
            }

            Console.WriteLine("\n");

            for (int i = 0; i < pg.playground.GetLength(0); i++)
            {
                for (int j = 0; j < pg.playground.GetLength(1); j++)
                {
                    Console.Write(pg.playground[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine($"enemy ships remaining: {shipsRemaining}"); // количество живых кораблей у противника
        }

        public char[,] gameplay;

        public int Xpos { get; set; }
        public int Ypos { get; set; }

        private void Shoot(EnemyPlayground enemy)   // сравнивает точку с игровым полем противника
        {
            if (enemy.enemyPlayground[Ypos, Xpos] == '#')
            {
                gameplay[Ypos, Xpos] = '@';         // при попадании по кораблю
                hit = true;
            }
            else
            {
                gameplay[Ypos, Xpos] = '▄';         // при промахе по кораблю
                hit = false;
            }
        }

        

        private void deleteLastPoint()              // удаление предыдущей позиции точки выбора "#"
        {                                           // 
            gameplay[Ypos, Xpos] = nextSymb;        // реализуется путем запоминания символа, перед тем, как "наступить" на него
        }                                           // при перемещении в другую точку, присваивает предыдущей клетке её символ

        private void NextMove()                     // перемещение на следующую точку выбора "#"
        {                                           // 
            nextSymb = gameplay[Ypos, Xpos];        // перед тем, как перейти на следующую точку, запоминает её символ,
            gameplay[Ypos, Xpos] = '#';             // после выхода с точки - присваиваетт её настоящий символ
        }

        private char nextSymb = '-';

        private bool hitCheck()                      // если точка выбора установлена на уже уничтоженном/подбитом корабле 
        {                                            // (ошибка ввода)
            if (gameplay[Ypos, Xpos] == '@')         //
            {
                return true;
            }
            return false;
        }

        private bool IsReversed(EnemyPlayground enemy)   // проверка повернут ли корабль, по которому попали
        {
            if (Xpos == 0)
            {
                if (enemy.enemyPlayground[Ypos, Xpos + 1] == '#')  // если по осе Х присутствует продолжение корабля, то корабль не повернут
                {                                                  // в данном случае возвращаем FALSE
                    return false;
                }
               
            }
            else if (Xpos == enemy.enemyPlayground.GetLength(0) - 1)
            {
                if (enemy.enemyPlayground[Ypos, Xpos - 1] == '#')
                {
                    return false;
                }
                
            }
            else                                                     // в ином случае, если по осе Х нет продолжения корабля, то он расположен ВЕРТИКАЛЬНО/ОДИНАРНЫЙ
            {                                                        // в этом случае возвращаем TRUE

                if (enemy.enemyPlayground[Ypos, Xpos - 1] == '#' || enemy.enemyPlayground[Ypos, Xpos + 1] == '#')
                {
                    return false;
                }
                
            }
            return true;
        }

        private int ShipTypeCount(EnemyPlayground enemy)            // счетчик размерности корабля, в который попал игрок, для
        {                                                           // определения уничтожен ли корабль в дальнейшем
            int counter = 1;

            if (_reversed)                                           // для вертикально расположенных кораблей
            {
                int tempY = Ypos;

                while (tempY - 1 >= 0 && enemy.enemyPlayground[tempY - 1, Xpos] == '#')  // итерируемся сначала к верхней крайней части корабля
                {
                    counter++;
                    tempY--;
                }
                tempY = Ypos;
                while (tempY + 1 <= enemy.enemyPlayground.GetLength(0) - 1 && enemy.enemyPlayground[tempY + 1, Xpos] == '#')  // затем к нижней крайней части корабля
                {                                                                                                             // в итоге получаем кол-во слотов, которые занимает целый корабль
                    counter++;
                    tempY++;
                }
            }
            else                                                      // для горизонтально расположенных кораблей
            {
                int tempX = Xpos;

                while (tempX - 1 >= 0 && enemy.enemyPlayground[Ypos, tempX - 1] == '#')      // итерируемся сначала к левой крайней части корабля
                {
                    counter++;
                    tempX--;
                }
                tempX = Xpos;
                while (tempX + 1 <= enemy.enemyPlayground.GetLength(0) - 1 && enemy.enemyPlayground[Ypos, tempX + 1] == '#')  // затем к правой крайней части корабля
                {                                                                                                             // в итоге получаем кол-во слотов, которые занимает целый корабль
                    counter++;
                    tempX++;
                }
            }
            return counter;      // возвращаем размерность корабля
        }

        private bool IsShipDestroyed(EnemyPlayground enemy)  // проверка уничтожен ли полностью корабль, в который попал игрок
        {
            ///////////////////////
            // обозначаем FIRST и LAST позиции корабля,
            // сравниваем поля EnemyPlayground (где расположены корабли противника) и Gameplay (где ходит игрок)
            // в итоге если выделенный корабль, который присутствует в EnemyPlayground и в Gameplay полях, 
            // то возвращаем TRUE
            // 
            // если корабль уничтожен не полностью - возвращаем FALSE
            ///////////////////////
            ///
            int firstPoint = 0;

            if (_reversed)    // для вертикально расположенных кораблей
            {
                firstPoint = Ypos; 

                while (firstPoint - 1 >= 0 && enemy.enemyPlayground[firstPoint - 1, Xpos] == '#')  // обозначаем верхнюю точку корабля
                {
                    firstPoint--;
                }
                Ypos = firstPoint;  // затем присваиваем начало корабля для Y

                while (firstPoint != Ypos + _shipType)
                {
                    if (gameplay[firstPoint, Xpos] != '@')  // если в определенной границе корабля, закрашенные значения не совпадают кораблю
                    {                                       // возвращаем FALSE
                        return false;                       //
                    }
                    firstPoint++;
                }
            }
            else         // для горизонтально расположенных кораблей
            {
                firstPoint = Xpos;

                while (firstPoint - 1 >= 0 && enemy.enemyPlayground[Ypos, firstPoint - 1] == '#')  // обозначаем левую точку корабля
                {
                    firstPoint--;
                }
                Xpos = firstPoint;   // затем присваиваем начало корабля для Х

                while (firstPoint != Xpos + _shipType)
                {
                    if (gameplay[Ypos, firstPoint] != '@')  // если в определенной границе корабля, закрашенные значения не совпадают кораблю
                    {                                       // возвращаем FALSE
                        return false;                       //
                    }
                    firstPoint++;
                }
            }
            return true;    // если границы и значения кораблей совпадают, значит он закрашен полностью
        }                   // возвращаем TRUE

        private void DestroyShip(EnemyPlayground enemy)  // закрашивание корабля и ячеек вокруг него
        {
            if (IsShipDestroyed(enemy))
            {
                
                    int fpY = Ypos - 1;        // левая верхняя граница корабля
                    int fpX = Xpos - 1;        //  (firstPointX / firstPointY)

                    int spY = 0;               // правая нижняя граница корабля
                    int spX = 0;               // (secondPointX / secondPointY)

                if (_reversed)                 // 
                {                             // определение крайних границ
                    spY = Ypos + _shipType;    //
                        spX = Xpos + 1;
                    }
                    else
                    {
                        spX = Xpos + _shipType;
                        spY = Ypos + 1;
                    }

                    if (fpX < 0)               //
                    {                          // поправка границ, если они выходят за поле
                        fpX++;                 //
                    }
                    if (fpY < 0)
                    {
                        fpY++;
                    }
                    if (spX > gameplay.GetLength(0) - 1)
                    {
                        spX--;
                    }
                    if (spY > gameplay.GetLength(0) - 1)
                    {
                        spY--;
                    }

                    bool shipElement = false;

                    if (_reversed)                    // обводка ВЕРТИКАЛЬНЫХ уничтоженных кораблей 
                    {
                        int endY = Ypos + _shipType;  // конечная позиция корабля

                        for (int i = fpX; i <= spX; i++)
                        {
                            for (int j = fpY; j <= spY; j++)
                            {
                                if (i == Xpos) 
                                { 
                                    int currentY = Ypos;

                                    while (currentY != endY)
                                    {
                                        if (j == currentY)
                                        {
                                            shipElement = true;
                                        }
                                        currentY++;
                                    }
                                }
                                if (shipElement)
                                {
                                    gameplay[j, i] = '@';  // закрашивание и обводка уничтоженного корабля
                                }
                                else
                                {
                                    gameplay[j, i] = '▄';
                                }
                            shipElement = false;
                        }
                        }
                    }
                else                              // обводка ГОРИЗОНТАЛЬНЫХ уничтоженных кораблей
                {
                        int endX = Xpos + _shipType;  // конечная позиция корабля

                    for (int i = fpY; i <= spY; i++)
                        {
                            for (int j = fpX; j <= spX; j++)
                            {
                                if (i == Ypos)
                                {
                                    shipElement = false;

                                    int currentX = Xpos;

                                    while (currentX != endX)
                                    {
                                        if (j == currentX)
                                        {
                                            shipElement = true;
                                        }
                                        currentX++;
                                    }
                                }
                                if (shipElement)
                                {
                                    gameplay[i, j] = '@'; // закрашивание и обводка уничтоженного корабля
                            }
                                else
                                {
                                    gameplay[i, j] = '▄';
                                }
                            shipElement = false;
                        }
                        }
                    }

                shipsRemaining--;            // уменьшаем кол-во оставшихся кораблей

            }
        }

        public int shipsRemaining = 10;      // кол-во кораблей противника
        private int _shipType = 0;            // тип корабля (количество палуб)
        private bool _reversed = false;       // перевернут ли корабль (FALSE - горизонтальный | TRUE - вертикальный)
        public bool hit = true;              // попадание по кораблю (если игрок попал по кораблю - TRUE, программа позволяет сделать еще один ход
                                             // в ином случае - FALSE, ход передается противнику
        public bool Control(Playground pg, EnemyPlayground enemy, EnemyGameplay enemyG)
        {
            bool check = false;              // находится в цикле управления, пока игрок не нажал ENTER
                                             // используется для выхода из цикла управления точкой "#"

            Xpos = 5;                        // значения Х и Y точки "#" по умолчанию, эта точка будет появляться
            Ypos = 5;                        // на этих координатах каждый ход игрока
            nextSymb = gameplay[Ypos, Xpos];
            gameplay[Ypos, Xpos] = '#';      // обьявляем точку "#" на данных координатах

            ShowAllPlaygroundGameplay(pg, enemyG); // вывод полей игрока и противника на экран

            while (!check)
            {
                bool move = true;           // если ход невозможен (точка упирается в границы - возвращаем FALSE (не позволяем выходить за границу)
                
                ConsoleKey key = Console.ReadKey().Key;

                //////////////////
                // ниже написан цикл управления точкой, координаты точки меняются соответствуя месту, куда походил игрок
                //
                // алгоритм выполнения: проверка границ точки - удаление предыдущей позиции "#" - присваивание новых координат для точки "#"
                /////////////////
                ///
                switch (key)
                {
                    case ConsoleKey.W:
                        if (Ypos - 1 >= 0)
                        { 
                        deleteLastPoint();
                        Ypos--;
                        }
                        else
                        {
                            move = false;
                        }
                        break;
                    case ConsoleKey.S:
                        if (Ypos + 1 < gameplay.GetLength(0))
                        {
                            deleteLastPoint();
                            Ypos++;
                        }
                        else
                        {
                            move = false;
                        }
                        break;
                    case ConsoleKey.A:
                        if (Xpos - 1 >= 0)
                        {
                            deleteLastPoint();
                            Xpos--;
                        }
                        else
                        {
                            move = false;
                        }
                        break;
                    case ConsoleKey.D:
                        if (Xpos + 1 < gameplay.GetLength(0))
                        {
                            deleteLastPoint();
                            Xpos++;
                        }
                        else
                        {
                            move = false;
                        }
                        break;
                    case ConsoleKey.Enter: // если игрок хочет выстрелить в данную точку
                        
                            Shoot(enemy);  // попал/не попал ли игрок, установка определенного знака

                            check = true;

                            if (hitCheck())  // если попал
                            {
                                _reversed = IsReversed(enemy); // определение ориентации корабля (вертикальный/горизонтальный)

                                _shipType = ShipTypeCount(enemy); // определение тип (кол-во палуб) корабля

                                DestroyShip(enemy);  // проверка уничтожен ли корабль
                            }                        // если уничтожен - закрашивание границ корабля
                            move = false;            // в ином случае - отметка "попадания" в корабль
                        break;

                }
                if (move)
                {
                    NextMove();  // запоминание символа следующей точки и переход на неё
                }

                ShowAllPlaygroundGameplay(pg, enemyG);  // вывод поля на экран
            }
            return hit;
        }


    }
}
