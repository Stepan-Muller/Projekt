using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelEditor
{
    internal class CellPosition
    {
        public int mapIndex;
        public int x;
        public int y;

        public CellPosition(int mapIndex, int x, int y) 
        { 
            this.mapIndex = mapIndex;
            this.x = x;
            this.y = y;
        }
    }
}
