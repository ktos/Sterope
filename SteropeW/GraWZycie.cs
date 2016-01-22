using System;
using System.Collections.Generic;
using System.Text;

namespace Ktos.Sterope
{
    [global::System.Serializable]
    public class LifeException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public LifeException() { }
        public LifeException(string message) : base(message) { }
        public LifeException(string message, Exception inner) : base(message, inner) { }
        protected LifeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    public class GraWZycie
    {
        /// <summary>
        /// Plansza, o wymiarach różnych, zwykle 10x10, w których komórka może być 
        /// albo martwa (false) albo żywa (true).
        /// </summary>
        private int[,] plansza;

        /// <summary>
        /// Tutaj będą umieszczone ilości sąsiadów, przy których martwa komórka staje
        /// się żywa w następnej jednostce czasu. W klasycznej grze Conveya jest to 
        /// tylko 3, ale my przecież stawiamy na różnorodność :-)
        /// </summary>
        private int[] regulyOzywiania;

        /// <summary>
        /// Analogicznie do reguł ożywiania - jeżeli liczba sąsiadów będzie _inna_ niż
        /// umieszczone tutaj, to komórka umrze - albo z "samotności", albo z przeludnienia
        /// </summary>
        private int[] regulyZabijania;

        private int runda = 0;

        public int IluSasiadow(int x, int y)
        {
            if ((x == 0) || (y == 0) || (x == plansza.GetLength(0)) || (y == plansza.GetLength(1)))
                throw new LifeException("Nie można odwoływać się komórek na krawędziach");
            else
            {
                int suma;
                suma = plansza[x - 1, y - 1] + plansza[x, y - 1] + plansza[x + 1, y - 1] + plansza[x - 1, y] 
                        + plansza[x + 1, y] + plansza[x - 1, y + 1] + plansza[x, y + 1] + plansza[x + 1, y + 1];

                return suma;
            }
        }        

        public GraWZycie(int rozmiar, int[] regulyOzywiania, int[] regulyZabijania)
        {
            // inicjalizacja i wypełnianie zerami
            plansza = new int[rozmiar, rozmiar];
            for (int i = 0; i < plansza.GetLength(0); i++)
            {
                for (int j = 0; j < plansza.GetLength(1); j++)
                {
                    plansza[i, j] = 0;
                }
            }

            this.regulyOzywiania = regulyOzywiania;
            this.regulyZabijania = regulyZabijania;
        }

        public void NastepnaRunda()
        {
            int[,] temp;
            temp = new int[plansza.GetLength(0), plansza.GetLength(1)];

            int stan;
            int iloscSasiadow;
            for (int x = 1; x < plansza.GetLength(0) - 1; x++)
                for (int y = 1; y < plansza.GetLength(1) - 1; y++)
                {
                    stan = plansza[x, y];
                    iloscSasiadow = this.IluSasiadow(x, y);

                    // jeżeli komórka żyje, to być może zaszła konieczność
                    // jej uśmiercenia
                    if (stan == 1)
                    {
                        if (Array.IndexOf(this.regulyZabijania, iloscSasiadow) == -1)
                        {
                            stan = 0;
                        }
                    }
                    else
                    {
                        if (Array.IndexOf(this.regulyOzywiania, iloscSasiadow) != -1)
                            stan = 1;
                    }

                    temp[x, y] = stan;
                }

            // przepisywanie tablicy tymczasowej do właściwej planszy
            for (int i = 1; i < plansza.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < plansza.GetLength(1) - 1; j++)
                {
                    plansza[i, j] = temp[i, j];
                }
            }

            runda++;
        }

        public int GetRunda()
        {
            return runda;
        }

        public int[,] GetPlansza()
        {
            return plansza;
        }

        public void SetStan(int x, int y, int stan)
        {
            plansza[x, y] = stan;
        }

        public int GetStan(int x, int y)
        {
            return plansza[x, y];
        }

        public void Clear()
        {
            for (int i = 0; i < plansza.GetLength(0); i++)
            {
                for (int j = 0; j < plansza.GetLength(1); j++)
                {
                    plansza[i, j] = 0;
                }
            }
            runda = 1;
        }
    }
}
