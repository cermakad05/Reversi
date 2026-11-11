using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi_ROP
{
    public class Deska
    {
        public Policko[,] Policka { get; set; }

        public Deska()
        {
            Policka = new Policko[8, 8];

            for (int radek = 0; radek < 8; radek++)
            {
                for (int sloupec = 0; sloupec < 8; sloupec++)
                {
                    Policka[radek, sloupec] = new Policko(radek, sloupec);
                }
            }

        
            Policka[3, 3].Stav = StavKamene.Bily;
            Policka[3, 4].Stav = StavKamene.Cerny;
            Policka[4, 3].Stav = StavKamene.Cerny;
            Policka[4, 4].Stav = StavKamene.Bily;
        }

  
        public bool PolozKamen(int radek, int sloupec, StavKamene barva)
        {
            if (Policka[radek, sloupec].Stav != StavKamene.Prazdne)
                return false;

            Policka[radek, sloupec].Stav = barva;
            return true;
        }

    
        public int SpocitejKameny(StavKamene barva)
        {
            int pocet = 0;

            for (int radek = 0; radek < 8; radek++)
            {
                for (int sloupec = 0; sloupec < 8; sloupec++)
                {
                    if (Policka[radek, sloupec].Stav == barva)
                        pocet++;
                }
            }

            return pocet;
        }
    }
}
