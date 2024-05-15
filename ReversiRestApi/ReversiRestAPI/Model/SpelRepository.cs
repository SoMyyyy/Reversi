namespace ReversieISpelImplementatie.Model;

public class SpelRepository : ISpelRepository
{
    public List<Spel> Spellen { get; set; }
    private Dictionary<string, ZetInfoApi> ZetInfos { get; set; } // Add this line


    public SpelRepository()
    {
        Spel spel1 = new Spel();
        Spel spel2 = new Spel();
        Spel spel3 = new Spel();

        spel1.Speler1Token = "abcdef";
        spel1.Omschrijving = "Potje snel reversi, dus niet lang nadenken";
        spel2.Speler1Token = "ghijkl";
        spel2.Speler2Token = "mnopqr";
        spel2.Omschrijving = "Ik zoek een gevorderde tegenspeler!";
        spel3.Speler1Token = "stuvwx";
        spel3.Omschrijving = "Na dit spel wil ik er nog een paar spelen tegen zelfde tegenstander";

        Spellen = new List<Spel> {spel1, spel2, spel3};
        
        ZetInfos = new Dictionary<string, ZetInfoApi>(); // Initialize the dictionary

    }

    // create game
    public void AddSpel(Spel spel)
    {
        Spellen.Add(spel);
    }

    // get a list of the games
    public List<Spel> GetSpellen()
    {
        return Spellen;
    }

    // get a game using gameID
    public Spel GetSpel(string spelToken)
    {
        return Spellen.FirstOrDefault(s => s.Token == spelToken);
    }
    public Spel GetSpelFromSpeler1Token(string spelerToken)
    {
        return Spellen.FirstOrDefault(s => s.Speler1Token == spelerToken );
    }

    public Spel GetSpelFromSpeler2Token(string spelerToken)
    {
        return Spellen.FirstOrDefault(s => s.Speler2Token == spelerToken);
    }

    public Spel GetSpelById(int id)
    {
        return Spellen.FirstOrDefault(s => s.ID == id);
    }

    public void VerwijderSpel(Spel spel)
    {
        Spellen.Remove(spel);
    }

    public void SaveZetInfo(ZetInfoApi zetInfo)
    {
        ZetInfos[zetInfo.Speltoken] = zetInfo;

    }

    public ZetInfoApi GetZetInfo(string spelToken)
    {
        ZetInfos.TryGetValue(spelToken, out var zetInfo);
        return zetInfo;
    }


    public List<Spel> GetWaitingSpellen()
    {
        return Spellen.Where(s => s.Speler2Token == null).ToList();
    }

    public void UpdateSpel(Spel spel)
    {
        //! i am not sure about this logic ); 
        var index = Spellen.FindIndex(s => s.Token == spel.Token);
        Spellen[index] = spel;
    }
}