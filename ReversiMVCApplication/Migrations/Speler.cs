using System;
using System.Collections.Generic;

namespace ReversiMVCApplication.Migrations
{
    public partial class Speler
    {
        public string Guid { get; set; } = null!;
        public string Naam { get; set; } = null!;
        public int AantalGewonnen { get; set; }
        public int AantalVerloren { get; set; }
        public int AantalGelijk { get; set; }
    }
}
