using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ReversieISpelImplementatie.Model
{
    public class Spel : ISpel
    {
        private const int bordOmvang = 8;
        
       

        private readonly int[,] richting = new int[8, 2]
        {
            { 0, 1 }, // naar rechts
            { 0, -1 }, // naar links
            { 1, 0 }, // naar onder
            { -1, 0 }, // naar boven
            { 1, 1 }, // naar rechtsonder
            { 1, -1 }, // naar linksonder
            { -1, 1 }, // naar rechtsboven
            { -1, -1 }
        }; // naar linksboven

        [Key] public int ID { get; set; }
        public State GameState { get; set; }
        public string Omschrijving { get; set; }
        public string Token { get; set; }
        public string Speler1Token { get; set; }

        public string? Speler2Token { get; set; }

        [NotMapped] private Kleur[,] bord;

        // ! added property 
        // This is the property that Entity Framework will map to the database
        // [Column("Bord")]
        // private string BordAsString 
        // { 
        //     get => ConvertBordToString(Bord);
        //     set => Bord = ConvertStringToBord(value);
        // }
        // private string ConvertBordToString(Kleur[,] bord)
        // {
        //     // Implement the logic to convert the 2D array to a string
        //     return string.Join(",", bord.Cast<Kleur>().Select(x => (int)x));
        // }
        // private Kleur[,] ConvertStringToBord(string bordString)
        // {
        //     // Implement the logic to convert the string back to a 2D array
        //     var flatArray = bordString.Split(',').Select(x => (Kleur)int.Parse(x)).ToArray();
        //     var result = new Kleur[8, 8];
        //     Buffer.BlockCopy(flatArray, 0, result, 0, flatArray.Length);
        //     return result;
        // }

        // // This is the property that the app will use
        // [NotMapped]
        // public Kleur[,] Bord
        // {
        //     get { return bord; }
        //     set { bord = value; }
        // }
        [NotMapped] public Kleur[,] Bord { get; set; }

        public string bordString
        {
            get { return JsonConvert.SerializeObject(Bord); }
            set { Bord = JsonConvert.DeserializeObject<Kleur[,]>(value); }
        }

        public Kleur AandeBeurt { get; set; }


        public Spel()
        {
            Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            Token = Token.Replace("/", "q"); // slash mijden ivm het opvragen van een spel via een api obv het token
            Token = Token.Replace("+", "r"); // plus mijden ivm het opvragen van een spel via een api obv het token

            Bord = new Kleur[bordOmvang, bordOmvang];
            Bord[3, 3] = Kleur.Wit;
            Bord[4, 4] = Kleur.Wit;
            Bord[3, 4] = Kleur.Zwart;
            Bord[4, 3] = Kleur.Zwart;

            AandeBeurt = Kleur.Wit;
        }

        public void Pas()
        {
            // controleeer of er geen zet mogelijk is voor de speler die wil passen, alvorens van beurt te wisselen.
            if (IsErEenZetMogelijk(AandeBeurt))
                throw new Exception("Passen mag niet, er is nog een zet mogelijk");
            else
                WisselBeurt();
        }


        public bool Afgelopen() // return true if neither player can make a move
        {
            return (!IsErEenZetMogelijk(Kleur.Wit) && !IsErEenZetMogelijk(Kleur.Zwart));
            
            
        }

        public Kleur OverwegendeKleur()
        {
            int aantalWit = 0;
            int aantalZwart = 0;
            for (int rijZet = 0; rijZet < bordOmvang; rijZet++)
            {
                for (int kolomZet = 0; kolomZet < bordOmvang; kolomZet++)
                {
                    if (bord[rijZet, kolomZet] == Kleur.Wit)
                        aantalWit++;
                    else if (bord[rijZet, kolomZet] == Kleur.Zwart)
                        aantalZwart++;
                }
            }

            if (aantalWit > aantalZwart)
                return Kleur.Wit;
            if (aantalZwart > aantalWit)
                return Kleur.Zwart;
            return Kleur.Geen;
        }

        public bool ZetMogelijk(int rijZet, int kolomZet)
        {
            if (!PositieBinnenBordGrenzen(rijZet, kolomZet))
                throw new Exception($"Zet ({rijZet},{kolomZet}) ligt buiten het bord!");

            return ZetMogelijk(rijZet, kolomZet, AandeBeurt);
        }

        private bool ZetMogelijk(int rijZet, int kolomZet, Kleur kleur)
        {
            // // Check if the specified position is within the board boundaries
            // if (!PositieBinnenBordGrenzen(rijZet, kolomZet))
            //     return false;
            //
            // // Check if the specified position is empty
            // if (Bord[rijZet, kolomZet] != Kleur.Geen)
            //     return false;

            // Check if the move is possible in any direction
            for (int i = 0; i < 8; i++)
            {
                if (StenenInTeSluitenInOpgegevenRichting(rijZet, kolomZet, kleur, richting[i, 0], richting[i, 1]))
                {
                    Console.WriteLine($"rijZer: {rijZet} kolomZet: {kolomZet} richting: {i}");
                    return true;
                }
            }

            // Return false if no direction allows the move
            return false;
        }


        // public void DoeZet(int rijZet, int kolomZet)
        // {
        //     if (!ZetMogelijk(rijZet, kolomZet))
        //         throw new Exception($"Zet ({rijZet},{kolomZet}) is niet mogelijk!");
        //
        //     // Flip opponent's stones in all possible directions
        //     for (int i = 0; i < bordOmvang; i++)
        //     {
        //         DraaiStenenVanTegenstanderInOpgegevenRichtingOmIndienIngesloten(rijZet, kolomZet,
        //             AandeBeurt, richting[i, 0], richting[i, 1]);
        //     }
        //
        //     // Perform the actual move
        //     Bord[rijZet, kolomZet] = AandeBeurt;
        //
        //
        //     // Switch turn to the other player
        //     WisselBeurt();
        // }

        private void printBord(int curRij, int curkolom)
        {
            for (int i = 0; i < Bord.GetLength(0); i++)
            {
                string row = "";
                for (int j = 0; j < Bord.GetLength(1); j++)
                {
                    char speler = '0';
                    if (i == curRij && j == curkolom)
                    {
                        speler = 'X';
                    }
                    else if (Bord[i, j] == Kleur.Zwart)
                    {
                        speler = 'z';
                    }
                    else if (Bord[i, j] == Kleur.Wit)
                    {
                        speler = 'w';
                    }

                    row = $"{row}{speler}";
                }

                Console.WriteLine(row);
            }
        }

        public void DoeZet(int rijZet, int kolomZet)
        {
            if (!PositieBinnenBordGrenzen(rijZet, kolomZet))
                throw new Exception($"Zet ({rijZet},{kolomZet}) ligt buiten het bord!");

            if (!ZetMogelijk(rijZet, kolomZet, AandeBeurt))
                throw new Exception($"Zet ({rijZet},{kolomZet}) is niet mogelijk!");

            Console.WriteLine("before:");
            printBord(rijZet, kolomZet);
            for (int i = 0; i < 8; i++)
            {
                bool inTeSluiten =
                    StenenInTeSluitenInOpgegevenRichting(rijZet, kolomZet, AandeBeurt, richting[i, 0], richting[i, 1]);
                bool omgedraaid = DraaiStenenVanTegenstanderInOpgegevenRichtingOmIndienIngesloten(rijZet, kolomZet,
                    AandeBeurt, richting[i, 0], richting[i, 1]);
                Console.WriteLine($" in te sluiten: {inTeSluiten} omgedraaid: {omgedraaid}");
            }

            Bord[rijZet, kolomZet] = AandeBeurt;
            Console.WriteLine("after:");
            printBord(-1, -1);
            if (!Afgelopen())
            {
                WisselBeurt();
            }
            else
            {
                AandeBeurt = Kleur.Geen;
                GameState = State.Klaar;
            }


            // throw new NotImplementedException();    // todo: maak hierbij gebruik van de reeds in deze klassen opgenomen methoden!
        }


        private static Kleur GetKleurTegenstander(Kleur kleur)
        {
            return kleur switch
            {
                Kleur.Wit => Kleur.Zwart,
                Kleur.Zwart => Kleur.Wit,
                _ => Kleur.Geen
            };
        }

        public bool IsErEenZetMogelijk(Kleur kleur)
        {
            if (kleur == Kleur.Geen)
                throw new Exception("Kleur mag niet gelijk aan Geen zijn!");
            // controleeer of er een zet mogelijk is voor kleur
            for (int rijZet = 0; rijZet < bordOmvang; rijZet++)
            {
                for (int kolomZet = 0; kolomZet < bordOmvang; kolomZet++)
                {
                    if (ZetMogelijk(rijZet, kolomZet, kleur))
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        private void WisselBeurt()
        {
            AandeBeurt = AandeBeurt == Kleur.Wit ? Kleur.Zwart : Kleur.Wit;
        }

        private static bool PositieBinnenBordGrenzen(int rij, int kolom)
        {
            return (rij >= 0 && rij < bordOmvang &&
                    kolom >= 0 && kolom < bordOmvang);
        }

        private bool ZetOpBordEnNogVrij(int rijZet, int kolomZet)
        {
            // Als op het bord gezet wordt, en veld nog vrij, dan return true, anders false
            return (PositieBinnenBordGrenzen(rijZet, kolomZet) && Bord[rijZet, kolomZet] == Kleur.Geen);
        }

        private bool StenenInTeSluitenInOpgegevenRichting(int rijZet, int kolomZet,
            Kleur kleurZetter,
            int rijRichting, int kolomRichting)
        {
            int rij, kolom;
            Kleur kleurTegenstander = GetKleurTegenstander(kleurZetter);
            if (!ZetOpBordEnNogVrij(rijZet, kolomZet))
                return false;

            // Zet rij en kolom op de index voor het eerst vakje naast de zet.
            rij = rijZet + rijRichting;
            kolom = kolomZet + kolomRichting;

            int aantalNaastGelegenStenenVanTegenstander = 0;
            // Zolang Bord[rij,kolom] niet buiten de bordgrenzen ligt, en je in het volgende vakje 
            // steeds de kleur van de tegenstander treft, ga je nog een vakje verder kijken.
            // Bord[rij, kolom] ligt uiteindelijk buiten de bordgrenzen, of heeft niet meer de
            // de kleur van de tegenstander.
            // N.b.: deel achter && wordt alleen uitgevoerd als conditie daarvoor true is.
            while (PositieBinnenBordGrenzen(rij, kolom) && Bord[rij, kolom] == kleurTegenstander)
            {
                rij += rijRichting;
                kolom += kolomRichting;
                aantalNaastGelegenStenenVanTegenstander++;
            }

            // Nu kijk je hoe je geeindigt bent met bovenstaande loop. Alleen
            // als alle drie onderstaande condities waar zijn, zijn er in de
            // opgegeven richting stenen in te sluiten.
            return (PositieBinnenBordGrenzen(rij, kolom) &&
                    Bord[rij, kolom] == kleurZetter &&
                    aantalNaastGelegenStenenVanTegenstander > 0);
        }

        private bool DraaiStenenVanTegenstanderInOpgegevenRichtingOmIndienIngesloten(int rijZet, int kolomZet,
            Kleur kleurZetter,
            int rijRichting, int kolomRichting)
        {
            int rij, kolom;
            Kleur kleurTegenstander = GetKleurTegenstander(kleurZetter);
            bool stenenOmgedraaid = false;

            if (StenenInTeSluitenInOpgegevenRichting(rijZet, kolomZet, kleurZetter, rijRichting, kolomRichting))
            {
                rij = rijZet + rijRichting;
                kolom = kolomZet + kolomRichting;

                // N.b.: je weet zeker dat je niet buiten het bord belandt,
                // omdat de stenen van de tegenstander ingesloten zijn door
                // een steen van degene die de zet doet.
                while (Bord[rij, kolom] == kleurTegenstander)
                {
                    Bord[rij, kolom] = kleurZetter;
                    rij += rijRichting;
                    kolom += kolomRichting;
                }

                stenenOmgedraaid = true;
            }

            return stenenOmgedraaid;
        }
    }

    public enum State
    {
        Wachten,
        Bezig,
        Klaar
    }
}