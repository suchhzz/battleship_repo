using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Battleship.Playground;

namespace Battleship
{
    internal partial class Playground // игровое поле игрока, в котором игрок размещает свои корабли,
    {                                 // затем противник делает свои ходы на этом поле
        public Playground() // конструткор
        {
            playground = new char[10, 10]; // игровое поле игрока с кораблями
            FillPlayground();              // заполняем пустым значением
            ShowPlayground();              // выводим на экран
        }
        public void FillPlayground()
        {
            for (int i = 0; i < playground.GetLength(0); i++)
            {
                for (int j = 0; j < playground.GetLength(1); j++)
                {
                    playground[i, j] = '-';
                }
            }
        }
        public void ShowPlayground(Ship ship = null)
        {
            Console.Clear();
            for (int i = 0; i < playground.GetLength(0); i++)
            {
                for (int j = 0; j < playground.GetLength(1); j++)
                {
                    Console.Write(playground[i, j] + " ");
                }
                Console.WriteLine();
            }


            if (ship != null)
            {
                Console.WriteLine("\ncurrent ship: ");
                for (int i = 0; i < ship.type; i++)
                {
                    Console.Write('#');
                }
                Console.WriteLine();
            }

            Console.WriteLine("\nR - rotate\nENTER - place");
        }
        public char[,] playground;

        public class Ship     // класс корабля, имеющий в себе тип (1-4 палубные)
        {
            public int type { get; set; }
            public Ship(int type = 0)
            {
                this.type = type;
            }
        }
        public Ship[] ships =  // массив кораблей с количеством x-палубных кораблей
        {
            new Ship(4),

            new Ship(3),
            new Ship(3),

            new Ship(2),
            new Ship(2),
            new Ship(2),

            new Ship(1),
            new Ship(1),
            new Ship(1),
            new Ship(1)
        };

        void deleteLastPos(Ship ship, bool reverse = false)   // удаление прошлой позиции при перемещении корабля
        {
            if (reverse) // для вертикально расположенных кораблей
            {
                                             // определяем границы корабля
                int end = _Ypos + ship.type;  // нижняя граница корабля
                int current = _Ypos;          // верхняя граница корабля
                while (current != end)
                {
                    playground[current, _Xpos] = '-'; // затирание корабля пустым значением (удаление)
                    current++;
                }
            }
            else     // для горизонтально расположенных кораблей
            {
                                             // определяем границы корабля
                int end = _Xpos + ship.type;  // левая граница корабля
                int current = _Xpos;          // правая граница корабля
                while (current != end)
                {
                    playground[_Ypos, current] = '-'; // затирание корабля пустым значением (удаление)
                    current++;
                }
            }
        }

        public void setPos(Ship ship, bool reverse = false) // установка корабля на новую позицию
        {
            if (reverse)   // для вертикально расположенных кораблей

            {                                   // определяем границы корабля
                int end = _Ypos + ship.type;     // нижняя граница корабля

                if (end > playground.GetLength(0))  // если корабль выходит за границы
                {
                    Console.WriteLine("you can not place it there");

                    this._reverse = false;       // запрещаем его переворачивать в этом месте
                }
                else                            // в ином случае
                {
                   
                int current = _Ypos;             // определяем верхнюю границу корабля

                    while (current != end)      // размещаем корабль в его пределах
                    {                           // (закрашиваем ячейки значением "#")
                        playground[current, _Xpos] = '#';
                        current++;
                    }
                }
            }
            else           // для вертикально расположенных кораблей
            {
                                                      // определяем границы корабля
                int end = this._Xpos + ship.type;      // правая граница корабля
                int current = this._Xpos;              // левая граница корабля

                while (current != end)                // размещаем корабль в его пределах
                {                                     // (закрашиваем ячейки значением "#")
                    playground[this._Ypos, current] = '#';
                    current++;
                }
            }

            ShowPlayground(ship);                     // вывод на экран
        }


        public bool SetShip(ConsoleKey key, Ship ship) // метод расположения (управлением кораблем) для его установки
        {

            //////////////////
            // ниже написан цикл управления кораблем, координаты корабля меняются соответствуя месту, куда походил игрок
            //
            // алгоритм выполнения: проверка границ - проверка не лежит ли на данной точке другой корабль - затирание старого месторасположения корабля - изменение координат -
            // - установка корабля в новом месте
            /////////////////

            switch (key)
            {
                case ConsoleKey.W:
                    if (_Ypos != 0 && ShipContact(ship, key, _reverse))
                    {
                        deleteLastPos(ship, _reverse);
                        _Ypos--;
                    }
                    break;
                case ConsoleKey.S:
                    if (_Ypos != playground.GetLength(0) - 1 && ShipContact(ship, key, _reverse))
                    {
                        deleteLastPos(ship, _reverse);
                        _Ypos++;
                    }
                    break;
                case ConsoleKey.A:
                    if (_Xpos != 0 && ShipContact(ship, key, _reverse))
                    {
                        deleteLastPos(ship, _reverse);
                        _Xpos--;
                    }
                    break;
                case ConsoleKey.D:
                    if (_Xpos != playground.GetLength(0) - 1 && ShipContact(ship, key, _reverse))
                    {
                        deleteLastPos(ship, _reverse);
                        _Xpos++;
                    }
                    break;
                case ConsoleKey.R: // поворот корабля

                    if (!_reverse)  // если не был перевернут
                    {
                        if (_Ypos + ship.type <= playground.GetLength(0) && ShipContact(ship,key,_reverse))  // проверка границ корабля, 
                        {                                                                                  // не будет ли он соприкосаться с границами/другими кораблями
                            deleteLastPos(ship, _reverse);
                            _reverse = true;                                                                // обозначаем корабль как ПЕРЕВЕРНУТЫЙ
                        }
                    }
                    else            // если корабль был перевернут
                    {
                        if (_Xpos + ship.type <= playground.GetLength(0) && ShipContact(ship,key, _reverse))  // проверка границ корабля, 
                        {                                                                                   // не будет ли он соприкосаться с границами/другими кораблями
                            deleteLastPos(ship, _reverse);   
                            _reverse = false;                                                                // обозначаем корабль как НЕ ПЕРЕВЕРНУТЫЙ
                        }
                    }
                    break;
                case ConsoleKey.Enter:                         // кнопка РАЗМЕЩЕНИЯ корабля
                    if (ShipContact(ship, key, _reverse))       // проверка границ корабля, не будет ли он соприкосаться с границами/другими кораблями
                    {
                        setPos(ship, _reverse);                 // окончательная установка корабля на поле
                        _reverse = false;                    
                        return true;                           // возвращаем TRUE, размещаем следующий корабль
                    }
                    break;

            }
            setPos(ship, _reverse);                             // окончательная установка корабля на поле
            return false;                                      // не выходим из цикла размещения корабля
        }

        private bool ShipContact(Ship ship, ConsoleKey key, bool reverse = false)  // проверка корабля на границы с полем/другими кораблями
        {
            //////////////////
            // ниже написан цикл проверки корабля на соприкосновение с другими кораблями/границей
            //
            // алгоритм выполнения: проверка границ - проверка не лежит ли на данной точке другой корабль - возврат TRUE/FALSE
            /////////////////

            int end = 0;
            int current = 0;

            switch (key)
            {
                case ConsoleKey.W:
                    if (reverse)
                    {
                        if (playground[_Ypos - 1, _Xpos] == '#')
                        {
                            return false;
                        }
                    }
                    else
                    {
                        end = _Xpos + ship.type;
                        current = _Xpos;
                        while (current != end)
                        {
                            if (playground[_Ypos - 1, current] == '#')
                            {
                                return false;
                            }
                            current++;
                        }
                    }
                break;

            case ConsoleKey.S:

                    if (reverse)
                {
                    end = _Ypos + ship.type -1;

                    if (end == playground.GetLength(0) - 1 || playground[end + 1, _Xpos] == '#')
                    {
                        return false;
                    }
                }
                else
                {
                    end = _Xpos + ship.type;
                    current = _Xpos;
                    while (current != end)
                    {
                        if (playground[_Ypos + 1, current] == '#')
                        {
                            return false;
                        }
                        current++;
                    }
                }
                break;

            case ConsoleKey.A:

                if (reverse)
                    {
                        end = _Ypos + ship.type;
                        current = _Ypos;
                        while (current != end)
                        {
                            if (playground[current, _Xpos - 1] == '#')
                            {
                                return false;
                            }
                            current++;
                        }
                    }

                if (playground[_Ypos, _Xpos - 1] == '#')
                {
                    return false;
                }

                break;


            case ConsoleKey.D:
                if (reverse)
                {
                        end = _Ypos + ship.type;
                        current = _Ypos;
                        while (current != end)
                        {
                            if (playground[current, _Xpos + 1] == '#')
                            {
                                return false;
                            }
                            current++; 
                        }
                    }
                else
                {
                    end = _Xpos + ship.type;

                    if (_Xpos == playground.GetLength(0) - ship.type || playground[_Ypos, end] == '#')
                    {
                        return false;
                    }
                }
                break;

                case ConsoleKey.R:
                    if (!reverse)
                    {
                        end = _Ypos + ship.type;

                        current = _Ypos + 1;

                        while (current != end)
                        {
                            if (playground[current, _Xpos] == '#')
                            {
                                return false;
                            }
                            current++;
                        }
                    }
                    else
                    {
                        end = _Xpos + ship.type;

                        current = _Xpos + 1;

                        while (current != end)
                        {
                            if (playground[_Ypos, current] == '#')
                            {
                                return false;
                            }
                            current++;
                        }
                    }
                    break;

                case ConsoleKey.Enter: // если игрок хочет разместить корабль

                    deleteLastPos(ship, reverse);  // удаление корабля, для корректного размещения

                    int secondPointX;      // крайняя правая нижняя граница корабля
                    int secondPointY;      //

                    if (reverse)           // для ВЕРТИКАЛЬНО расположенных кораблей
                    {
                        secondPointX = _Xpos + 1;           // определяем правую нижнюю крайнюю границу
                        secondPointY = _Ypos + ship.type;   //
                    }
                    else                  // для ГОРИЗОНТАЛЬНО расположенных кораблей
                    {
                        secondPointX = _Xpos + ship.type;   // определяем правую нижнюю крайнюю границу
                        secondPointY = _Ypos + 1;           //
                    }


                    int firstPointX = _Xpos - 1;            // определяем левую верхнюю крайнюю границу
                    int firstPointY = _Ypos - 1;            // формула для левой верхней границы ВСЕГДА для всех типов кораблей одинаковая


                                            // поправка границ корабля, если они выходят за границы поля
                    if (firstPointX < 0)    //
                    {                       //
                        firstPointX++;      //
                    }
                    if (firstPointY < 0)
                    {
                        firstPointY++;
                    }
                    if (secondPointX > playground.GetLength(0) - 1)
                    {
                        secondPointX--;
                    }
                    if (secondPointY > playground.GetLength(0) - 1)
                    {
                        secondPointY--;
                    }

                    for (int i = firstPointY; i <= secondPointY; i++)   
                    {
                        for (int j = firstPointX; j <= secondPointX; j++)
                        {
                            if (playground[i, j] == '#')                    // если в границах текущего корабля присутствует другой корабль - возвращаем FALSE (запрещаем располагать его здесь)
                            {                                               
                                return false;
                            }
                        }
                    }
                    break;
            }
            return true;        // в ином случае - TRUE (можно расположить)
        }

        private bool _reverse = false;

        public void checkFirstPos(Ship ship)  // проверка лежит ли на дефолтных координатах (0;0) другой корабль
        {                                     // если корабль на координатах (0;0) присутствует - повышаем Y на 1
            bool empty = false;

            _Xpos = 0;
            _Ypos = 0;

            while (!empty)
            {

                empty = true;
                int current = _Xpos;

                while (current != ship.type)
                {
                    if (playground[_Ypos, current] == '#')
                    {
                        empty = false;
                        _Ypos++;
                        break;
                    }
                    current++;
                }
            }
        }

        private int _Xpos = 0;
        private int _Ypos = 0;
        public void ShipChoose()
        {
                for (int i = 0; i < ships.Length; i++) // цикл для итерации по массиву кораблей, имеет ship.type - кол-во палуб у корабля
                {
                    SetShip(0, ships[i]);   // размещение x-палубного корабля

                    bool set = false;

                while (!set)
                {
                    ConsoleKey key = Console.ReadKey().Key;

                    set = SetShip(key, ships[i]);  // определение set (при TRUE - продолжаем располагать корабль
                }                                  // если set = FALSE - переходим к следующему кораблю

                checkFirstPos(ships[i]);           // проверка лежит ли на дефолтных координатах (0;0) другой корабль
            }
        }
    }
}
