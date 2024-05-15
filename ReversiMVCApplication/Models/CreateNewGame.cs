using System.ComponentModel.DataAnnotations;

namespace ReversiMVCApplication.Models;

public class CreateNewGame
{
    [Required(ErrorMessage = "Omschrijving is required.")]
    [MinLength(3, ErrorMessage = "Omschrijving cannot be longer than 100 characters or less than 3 char.")]
    public string Omschrijving { get; set; }

    public string? Speler1Token { get; set; }
}