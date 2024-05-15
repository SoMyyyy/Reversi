namespace ReversieISpelImplementatie.Model;

public class OpgevenInfoApi
{
    public string SpelToken { get; set; }
    public string SpelerToken { get; set; }

    public bool Verify(Spel spel)
    {
        if (spel == null || (!spel.Speler1Token.Equals(SpelerToken) && !spel.Speler2Token.Equals(SpelerToken)))
        {
            return false;
        }
        return true;
    }
    
}