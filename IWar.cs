using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfWarships
{
    abstract class IWar
    {
        public abstract bool Shoot(int[,] arr, int x, int y);

        public abstract bool CanToShot(int[,] arr, int x, int y);
    }
}
