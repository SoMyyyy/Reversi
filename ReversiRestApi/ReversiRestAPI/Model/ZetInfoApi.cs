using System.ComponentModel.DataAnnotations;

namespace ReversieISpelImplementatie.Model;

public class ZetInfoApi
{
    [Key] public string Speltoken { get; set; }
    public string SpelerToken { get; set; }
    public int RijZet { get; set; }
    public int KolomZet { get; set; }

    public bool Pass { get; set; } = false;
}