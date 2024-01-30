using Microsoft.EntityFrameworkCore;
using ReversiAPI.DataAccess;
using ReversiAPI.Model;
using ReversiTestAPI.Model;
using System.Collections.Generic;
using System.Linq;

namespace ReversiAPI.Repositories
{
    public class SpelAccessLayer : ISpelRepository
    {
        private readonly SpelDbContext context;
        public SpelAccessLayer(SpelDbContext _context)
        {
            context = _context;
        }
        public void AddSpel(Spel spel)
        {
            if (spel.Omschrijving == null) 
                return;
            context.Spellen.Add(spel);
            context.SaveChanges();
        }

        public void ClearAll()
        {
            context.Spellen.RemoveRange(context.Spellen);
            context.SaveChanges();
        }

        public Spel GetSpelByToken(string spelToken)
        {
            return context.Spellen.Find(spelToken);
        }
        public Spel GetSpel(int id)
        {
            return context.Spellen.Find(id);
        }

        public List<SpelTbvJson> GetSpellen()
        {
            return SpelTbvJson.ConvertList(context.Spellen).ToList();;
        }
        public void Update(Spel spel)
        {
            context.Entry(spel).State = EntityState.Modified;
            context.SaveChanges();
        }
        public void Remove(Spel spel)
        {
            context.Remove(spel);
            context.SaveChanges();
        }
    }
}
