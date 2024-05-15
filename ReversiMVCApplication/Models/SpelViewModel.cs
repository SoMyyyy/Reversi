namespace ReversiMVCApplication.Models;

public class SpelViewModel
{
    public int Id { get; set; }
    public string Speler1UserName { get; set; }
    public string Speler2UserName { get; set; }
    public int? GameState { set; get; }

    public string Omschrijving { get; set; }
    public string? Token { get; set; }
    
    public int? AanDeBeurt { get; set; }
}