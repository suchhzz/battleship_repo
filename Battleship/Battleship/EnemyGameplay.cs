using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Globalization;
using System.Security.Policy;

namespace Battleship
{
    internal class EnemyGameplay
    {
        public int Ypos = 0;
        public int Xpos = 0;

        private bool reversed = false;
        private bool singlePoint = true;
        public int shipsRemaining = 10;

        bool hit = false;
        bool check = true;

        private int firstBorderX = 0;
        private int firstBorderY = 0;

        private int lastBorderX = 0;
        private int lastBorderY = 0;

        private void ShowAllPlaygroundGameplay(Playground pg, Gameplay gmpl)
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
                for (int j = 0; j < pg.playground.GetLength(1); j++)
                {
                    Console.Write(pg.playground[i, j] + " ");
                }
                Console.WriteLine();
            }

            Console.WriteLine($"y: {Ypos} x: {Xpos}");
        }

        private bool IsTaken(Playground plgr)
        {
            if (plgr.playground[Ypos, Xpos] == '▄' || plgr.playground[Ypos, Xpos] == '@')
            {
                return true;
            }
            return false;
        }

        private void setPos(Playground plgr, Gameplay gmpl)
        {
            if (plgr.playground[Ypos, Xpos] == '-')
            {
                plgr.playground[Ypos, Xpos] = '▄';
                check = false;
            }
            else if (plgr.playground[Ypos, Xpos] == '#')
            {
                plgr.playground[Ypos, Xpos] = '+';

                destroyShip(plgr);

                Thread.Sleep(1500);
            }

        }

        private bool checkPlayground(Playground plgr)
        {
            for (int i = 0; i < plgr.playground.GetLength(0); i++)
            {
                for (int j = 0; j < plgr.playground.GetLength(1); j++)
                {
                    if (plgr.playground[i, j] == '+')
                    {
                        Ypos = i;
                        Xpos = j;
                        return true; 
                    }
                }
            }
            return false;
        }

        private void reverseAndSingleCheck(Playground plgr)
        {

            bool up = false;
            bool down = false;
            singlePoint = true;
            reversed = false;

            if (Ypos > 0 && plgr.playground[Ypos - 1, Xpos] == '+')
            {
                up = true;
                singlePoint = false;
            }
            if (Ypos < plgr.playground.GetLength(0) - 1 && plgr.playground[Ypos + 1, Xpos] == '+')
            {
                down = true;
                singlePoint = false;
            }
            if (Xpos > 0 && plgr.playground[Ypos, Xpos - 1] == '+')
            {
                singlePoint = false;
            }
            if (Xpos < plgr.playground.GetLength(0) - 1 && plgr.playground[Ypos, Xpos + 1] == '+')
            {
                singlePoint = false;
            }

            if (up || down)
            {
                reversed = true;
            }
        }

        private void hitChoice(Playground plgr)
        {
            reverseAndSingleCheck(plgr);

            Random rnd = new Random();


            if (singlePoint)
            {
                int rndCoord = rnd.Next(0, 2);

                if (rndCoord == 0)
                {
                    int rndDirection = rnd.Next(0, 2);

                    if (rndDirection == 0)
                    {
                        if (Ypos - 1 >= 0 && plgr.playground[Ypos - 1, Xpos] != '▄')
                        {
                            Ypos--;
                        }
                        else
                        {
                            Ypos++;
                        }
                    }
                    else
                    {
                        if (Ypos + 1 <= plgr.playground.GetLength(0) - 1 && plgr.playground[Ypos + 1, Xpos] != '▄')
                        {
                            Ypos++;
                        }
                        else
                        {
                            Ypos--;
                        }
                    }
                }
                else
                {
                    int rndDirection = rnd.Next(0, 2);

                    if (rndDirection == 0)
                    {
                        if (Xpos - 1 >= 0 && plgr.playground[Ypos, Xpos - 1] != '▄')
                        {
                            Xpos--;
                        }
                        else
                        {
                            Xpos++;
                        }
                    }
                    else
                    {
                        if (Xpos + 1 <= plgr.playground.GetLength(0) - 1 && plgr.playground[Ypos, Xpos + 1] != '▄')
                        {
                            Xpos++;
                        }
                        else
                        {
                            Xpos--;
                        }
                    }
                }

            }
            else if (reversed && !singlePoint)
            {
                int rndDirection = rnd.Next(0, 2);

                int currentY = Ypos;

                if (rndDirection == 0)
                {
                    while (currentY - 1 >= 0 && plgr.playground[currentY - 1, Xpos] == '+')
                    {
                        currentY--;
                    }
                    if (currentY == 0 || plgr.playground[currentY -1, Xpos] == '▄')
                    {
                        while (plgr.playground[currentY + 1, Xpos] == '+')
                        {
                            currentY++;
                        }
                        Ypos = currentY + 1;
                    }
                    else
                    {
                        Ypos = currentY - 1;
                    }
                }
                else
                {
                    while (currentY + 1 <= plgr.playground.GetLength(0) - 1 && plgr.playground[currentY + 1, Xpos] == '+')
                    {
                        currentY++;
                    }
                    if (currentY == plgr.playground.GetLength(0) - 1 || plgr.playground[currentY + 1, Xpos] == '▄')
                    {
                        while (plgr.playground[currentY - 1, Xpos] == '+')
                        {
                            currentY--;
                        }
                        Ypos = currentY - 1;
                    }
                    else
                    {
                        Ypos = currentY + 1;
                    }
                }
            }
            else if (!reversed && !singlePoint)
            {
                int rndDirection = rnd.Next(0, 2);

                int currentX = Xpos;

                if (rndDirection == 0)
                {
                    while (currentX - 1 >= 0 && plgr.playground[Ypos, currentX - 1] == '+')
                    {
                        currentX--;
                    }
                    if (currentX == 0 || plgr.playground[Ypos, currentX - 1] == '▄')
                    {
                        while (plgr.playground[Ypos, currentX + 1] == '+')
                        {
                            currentX++;
                        }
                        Xpos = currentX + 1;
                    }
                    else
                    {
                        Xpos = currentX - 1;
                    }
                }
                else
                {
                    while (currentX + 1 <= plgr.playground.GetLength(0) - 1 && plgr.playground[Ypos, currentX + 1] == '+')
                    {
                        currentX++;
                    }
                    if (currentX == plgr.playground.GetLength(0) - 1 || plgr.playground[Ypos, currentX + 1] == '▄')
                    {
                        while (plgr.playground[Ypos, currentX - 1] == '+')
                        {
                            currentX--;
                        }
                        Xpos = currentX - 1;
                    }
                    else
                    {
                        Xpos = currentX + 1;
                    }
                }
            }
        }
        private bool IsShipDestroyed(Playground plgr)
        {
            bool borderCheck = true;

            reverseAndSingleCheck(plgr);

            if (singlePoint)
            {
                if (Xpos - 1 >= 0 && plgr.playground[Ypos, Xpos - 1] == '#')
                {
                    borderCheck = false;
                }
                if (Xpos + 1 <= plgr.playground.GetLength(0) - 1 && plgr.playground[Ypos, Xpos + 1] == '#')
                {
                    borderCheck = false;
                }
                if (Ypos - 1 >= 0 && plgr.playground[Ypos - 1, Xpos] == '#')
                {
                    borderCheck = false;
                }
                if (Ypos + 1 <= plgr.playground.GetLength(0) - 1 && plgr.playground[Ypos + 1, Xpos] == '#')
                {
                    borderCheck = false;
                }
            }
            else if (reversed && !singlePoint)
            {
                firstBorderX = lastBorderX = Xpos;

                int upperShipBorderY = Ypos;

                while (upperShipBorderY - 1 <= 0 && plgr.playground[upperShipBorderY - 1, Xpos] == '+')
                {
                    upperShipBorderY--;
                }
                if (upperShipBorderY != 0 && plgr.playground[upperShipBorderY - 1, Xpos] == '#')
                {
                    borderCheck = false;
                }
                else
                {
                    firstBorderY = upperShipBorderY;
                }

                int lowerShipBorderY = Ypos;

                while (lowerShipBorderY + 1 <= plgr.playground.GetLength(0) - 1 && plgr.playground[lowerShipBorderY + 1, Xpos] == '+')
                {
                    lowerShipBorderY++;
                }
                if (lowerShipBorderY != plgr.playground.GetLength(0) - 1 && plgr.playground[lowerShipBorderY + 1, Xpos] == '#')
                {
                    borderCheck = false;
                }
                else
                {
                    lastBorderY = lowerShipBorderY;
                }
            }
            else if (!reversed && !singlePoint)
            {
                firstBorderY = lastBorderY = Ypos;

                int leftShipBorderX = Xpos;

                while (leftShipBorderX - 1 >= 0 && plgr.playground[Ypos, leftShipBorderX - 1] == '+')
                {
                    leftShipBorderX--;
                }
                if (leftShipBorderX != 0 && plgr.playground[Ypos, leftShipBorderX - 1] == '#')
                {
                    borderCheck = false;
                }
                else
                {
                    firstBorderX = leftShipBorderX;
                }
                int rightShipBorderX = Xpos;

                while (rightShipBorderX + 1 <= plgr.playground.GetLength(0) - 1 && plgr.playground[Ypos, rightShipBorderX + 1] == '+')
                {
                    rightShipBorderX++;
                }
                if (rightShipBorderX != plgr.playground.GetLength(0) - 1 && plgr.playground[Ypos, rightShipBorderX + 1] == '#')
                {
                    borderCheck = false;
                }
                else
                {
                    lastBorderX = rightShipBorderX;
                }
            }
            return borderCheck;
        }
        private void borderPointsCheck(Playground plgr)
        {


            if (firstBorderX - 1 >= 0)
            {
                firstBorderX--;
            }
            if (firstBorderY - 1 >= 0)
            {
                firstBorderY--;
            }
            if (lastBorderX + 1 <= plgr.playground.GetLength(0) - 1)
            {
                lastBorderX++;
            }
            if (lastBorderY + 1 <= plgr.playground.GetLength(0) - 1)
            {
                lastBorderY++;
            }
        }

        private void destroyShip(Playground plgr)
        {
            if (IsShipDestroyed(plgr))
            {
                borderPointsCheck(plgr);

                if (reversed)
                {
                    for (int i = firstBorderX; i <= lastBorderX; i++)
                    {
                        for (int j = firstBorderY; j <= lastBorderY; j++)
                        {
                            if (plgr.playground[j, i] == '+')
                            {
                                plgr.playground[j, i] = '@';
                            }
                            else
                            {
                                plgr.playground[j, i] = '▄';
                            }
                        }
                    }
                }
                else
                {
                    for (int i = firstBorderY; i <= lastBorderY; i++)
                    {
                        for (int j = firstBorderX; j <= lastBorderX; j++)
                        {
                            if (plgr.playground[i, j] == '+')
                            {
                                plgr.playground[i, j] = '@';
                            }
                            else
                            {
                                plgr.playground[i, j] = '▄';
                            }
                        }
                    }
                }
                shipsRemaining--;
            }
        }

        private void tempFooPos(int y, int x)
        {
            Ypos = y;
            Xpos = x;
        }

        private bool temp = false;

        public bool enemyMove(Playground plgr, Gameplay gmpl)
        {
           


            Random rnd = new Random();

            hit = false;
            check = true;

            while (check)
            {
                if (checkPlayground(plgr))
                {
                    hitChoice(plgr);

                    setPos(plgr, gmpl);

                    
                }

                else
                {

                    Ypos = rnd.Next(0, 10);
                    Xpos = rnd.Next(0, 10);


                    if (!temp)
                    {
                        tempFooPos(6, 8);
                        temp = true;
                    }

                    if (!IsTaken(plgr))
                    {
                        setPos(plgr, gmpl);


                    }
                }
                ShowAllPlaygroundGameplay(plgr, gmpl);

            }
            return check;
        }
    }
}
