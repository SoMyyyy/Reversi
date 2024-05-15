namespace ReversieISpelImplementatie.Model;

public interface ISpelRepository
{
    void AddSpel(Spel spel);
    List<Spel> GetSpellen();
    Spel GetSpel(string spelToken);
    Spel GetSpelFromSpeler1Token(string spelerToken);
    Spel GetSpelFromSpeler2Token(string spelerToken);
    List<Spel> GetWaitingSpellen();
    void UpdateSpel(Spel spel);
    Spel GetSpelById(int id);
    void VerwijderSpel(Spel spel);



}

