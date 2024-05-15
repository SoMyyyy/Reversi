using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReversiMVCApplication.Models;

public class Spel
{
    [Key] public int Id { get; set; }
    public int? GameState { set; get; }

    [Required(ErrorMessage = "Omschrijving is required.")]
    [MinLength(3, ErrorMessage = "Omschrijving cannot be longer than 100 characters or less than 3 char.")]
    public string Omschrijving { get; set; }

    public string? Speler1Token { get; set; }
    [NotMapped]public string? Token { get; set; }

    public string? Speler2Token { get; set; }

    [NotMapped] public int[,]? Bord { get; set; }
    public int? AanDeBeurt { get; set; }


}

// public enum Kleur
// {
//     Wit,
//     Zwart
// }

// public enum SpelStatus
// {
//     Wachten, == 1
//     Bezig, == 2
//     Klaar == 3
// }