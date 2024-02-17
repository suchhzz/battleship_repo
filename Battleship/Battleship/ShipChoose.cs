using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    internal class Ship : Playground
    {
        private Ships[] ships = null;
        public Ship()  // массив кораблей
        {
            Ships[] ships = {
            new Ships(1),
            new Ships(1),
            new Ships(1),
            new Ships(1),

            new Ships(2),
            new Ships(2),
            new Ships(2),

            new Ships(3),
            new Ships(3),

            new Ships(4) };
            }
    
        public class Ships
        {
            public Ships(int type)
            {
                this.type = type;
            }
            public int type = 0;

            
        }


        public void FillPG()
        {
            bool check = false;

            while (!check)
            {
                ShowPlayground();
            }
        }
    }
}
