using ReversiTestAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiAPI.Model
{
    public interface ISpelRepository
    {
        void AddSpel(Spel spel);

        void ClearAll();

        public List<SpelTbvJson> GetSpellen();
        
        Spel GetSpelByToken(string spelToken);
        Spel GetSpel(int id);
        void Update(Spel spel);
        void Remove(Spel spel);
    }
}
