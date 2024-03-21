using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReversiAPI.Model;
using ReversiTestAPI.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReversiAPI.Controllers
{
    [Route("api/spel")]
    [ApiController]
    public class SpelController : ControllerBase
    {
        private readonly ISpelRepository iRepository;

        public SpelController(ISpelRepository repository)
        {
            iRepository = repository;
        }

        // Post api/spel
        [HttpPost]
        public ActionResult MaakNieuwSpel( string spelerToken, string spelOmschrijving)
        {
            Spel spel = new()
            {
                Speler1Token = spelerToken,
                Speler2Token = "",
                Omschrijving = spelOmschrijving
            };
            iRepository.AddSpel(spel);
            return Ok();
        }

        //GET api/spel/join
        [HttpGet("join")]
        public IActionResult JoinSpel(string spelerToken, int id)
        {
            Spel spel = iRepository.GetSpel(id);
            if (spel == null)
                return BadRequest("Spel bestaat niet!");
            if (spel.Speler2Token != "")
                return BadRequest("Spel is al vol");
            spel.Speler2Token = spelerToken;
            iRepository.Update(spel);
            return Ok();
        }

        // GET api/spel/beurt
        [HttpGet("beurt")]
        public ActionResult<Kleur> GetBeurt(int id)
        {
            return iRepository.GetSpellen().First(spel => spel.ID == id).AandeBeurt;
        }

        // POST api/spel/zet
        [HttpGet("zet")]
        public ActionResult<SpelTbvJson> DoeZet(int id, int rijZet, int kolomZet, string spelerId)
        {
            Spel spel = iRepository.GetSpel(id);
            if (spel.Speler2Token == String.Empty)
                return BadRequest("Spel nog niet gestart!");
            if (!((spel.AandeBeurt == Kleur.Wit && spelerId == spel.Speler1Token) || 
                (spel.AandeBeurt == Kleur.Zwart && spelerId == spel.Speler2Token)))
                return BadRequest("Je bent niet aan de beurt!");

            try {
                spel.DoeZet(rijZet, kolomZet);
            } catch (Exception){
                return BadRequest("Zet is onmogelijk!");
            }
            iRepository.Update(spel);
            return Ok(new SpelTbvJson(spel));
        }
        
        // GET api/spel/pas
        [HttpGet("pas")]
        public ActionResult Pas(int id)
        {
            Spel spel = iRepository.GetSpel(id);
            try{
                spel.Pas(); 
                iRepository.Update(spel);
            }
            catch(Exception){ 
                return BadRequest("Zet mogelijk"); 
            }
            return Ok("Gepast");
        }
        
        // GET api/spel/opgeven
        [HttpGet("opgeven")]
        public ActionResult GeefOp(int id, string speler)
        {
            Spel spel = iRepository.GetSpel(id);
            spel.Winnaar = (int)(speler == spel.Speler1Token ? Kleur.Zwart : Kleur.Wit);
            iRepository.Update(spel);
            return Ok();
        }

        // GET api/spel/end
        [HttpGet("end")]
        public ActionResult End(int id)
        {
            Spel spel = iRepository.GetSpel(id);
            if (spel == null)
                return BadRequest();
            if (spel.Winnaar == 0)
                return BadRequest();
            spel.Winnaar = 3;
            iRepository.Update(spel);
            return Ok();
        }

        // GET api/spel
        [HttpGet]
        public ActionResult<IEnumerable<SpelTbvJson>> GetSpelOmschrijvingenVanSpellenMetWachtendeSpeler()
        {
            return Ok(iRepository.GetSpellen().Where(spel => spel.Speler2Token == "" && spel.Winnaar == 0));
        }

        // GET api/spel/id
        [HttpGet("{id}")]
        public ActionResult<SpelTbvJson> GetSpel(int id)
        {
            var spel = iRepository.GetSpel(id);
            if (spel == null)
                return BadRequest("Spel bestaat niet!");
            return Ok(new SpelTbvJson(spel));
        }

        // GET api/spelspeler/spelerToken
        [HttpGet("speler/spelerToken")]
        public ActionResult<SpelTbvJson> GetSpel(string spelerToken)
        {
            return iRepository.GetSpellen().FirstOrDefault(s => s.Winnaar == (int)Kleur.Geen && (s.Speler1Token == spelerToken || s.Speler2Token == spelerToken));
        }

        // Delete api/spel/clear
        [HttpDelete]
        public IActionResult ClearAlleSpellen()
        {
            iRepository.ClearAll();
            return Ok();
        }

    }
}
