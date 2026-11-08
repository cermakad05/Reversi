using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi_ROP
{
    public class Policko
    {
        public int Radek { get; set; }
        public int Sloupec { get; set; }
        public StavKamene Stav { get; set; }

        public Policko(int radek, int sloupec)
        {
            Radek = radek;
            Sloupec = sloupec;
            Stav = StavKamene.Prazdne;
        }
    }
}
