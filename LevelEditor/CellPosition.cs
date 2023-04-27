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

        /// <summary>
        /// Vytvori objekt <c>CellPosition</c> se vsemi hodnotami
        /// </summary>
        /// <param name="mapIndex">Poradove cislo mapy</param> 
        /// <param name="x">Souradnice x v mape</param> 
        /// <param name="y">Souradnice y v mape</param> 
        public CellPosition(int mapIndex, int x, int y) 
        { 
            this.mapIndex = mapIndex;
            this.x = x;
            this.y = y;
        }
    }
}
