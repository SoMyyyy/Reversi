using Microsoft.EntityFrameworkCore;
using ReversiRestAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using ReversieISpelImplementatie.Model;

namespace ReversiRestAPI.DAL
{
    public class SpelAccessLayer : ISpelRepository
    {
        private readonly ReversiAPI_DBContext _context;

        public SpelAccessLayer(ReversiAPI_DBContext context)
        {
            _context = context;
        }

        public void AddSpel(Spel spel)
        {
            _context.Spellen.Add(spel);
            _context.SaveChanges();
        }

        public Spel GetSpel(string spelToken)
        {
            return _context.Spellen.FirstOrDefault(s => s.Token == spelToken);
        }

        public Spel GetSpelFromSpeler1Token(string spelerToken)
        {
            return _context.Spellen.FirstOrDefault(s =>  s.Speler1Token == spelerToken);
        }
        
        public Spel GetSpelFromSpeler2Token(string spelerToken)
        {
            return _context.Spellen.FirstOrDefault(s =>  s.Speler2Token == spelerToken);
        }

        public Spel GetSpelById(int id)
        {
            return _context.Spellen.FirstOrDefault(s => s.ID == id);
        }


        public List<Spel> GetSpellen()
        {
            return _context.Spellen.ToList();
        }
        

        public List<Spel> GetWaitingSpellen()
        {
            return _context.Spellen.Where(s => s.Speler2Token == null).ToList();
        }

        public void VerwijderSpel(Spel spel)
        {
            _context.Spellen.Remove(spel);
            _context.SaveChanges();
        }


        public void UpdateSpel(Spel spel)
        {
            _context.Update(spel);
            _context.SaveChanges();
        }
    }
}