using System;
using System.Collections.Generic;

namespace ReversiMVCApplication.Migrations
{
    public partial class Spel
    {
        public int Id { get; set; }
        public int? GameState { get; set; }
        public string Omschrijving { get; set; } = null!;
        public string? Speler1Token { get; set; }
        public string? Speler2Token { get; set; }
        public int? AanDeBeurt { get; set; }
    }
}
