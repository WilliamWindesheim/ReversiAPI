using ReversiTestAPI.Model;
using System;
using System.Collections.Generic;

namespace ReversiAPI.Model
{
    public class SpelTbvJson
    {
        private const int bordOmvang = 8;
        private readonly int[,] richting = new int[8, 2] {
                                {  0,  1 },         // naar rechts
                                {  0, -1 },         // naar links
                                {  1,  0 },         // naar onder
                                { -1,  0 },         // naar boven
                                {  1,  1 },         // naar rechtsonder
                                {  1, -1 },         // naar linksonder
                                { -1,  1 },         // naar rechtsboven
                                { -1, -1 } };       // naar linksboven

        public int ID { get; set; }
        public string Omschrijving { get; set; }
        public string Token { get; set; }
        public string Speler1Token { get; set; }
        public string Speler2Token { get; set; }
        public int Winnaar { get; set; }

        private string bord;

        public string Bord
        {
            get { return bord; }
            set { bord = value; }
        }
        public Kleur AandeBeurt { get; set; }


        public SpelTbvJson(Spel spel)
        {
            if (spel == null) 
                throw new Exception("Geen spel");
            ID = spel.ID;
            Omschrijving = spel.Omschrijving;
            Token = spel.Token;
            Speler1Token = spel.Speler1Token;
            Speler2Token = spel.Speler2Token;
            AandeBeurt = spel.AandeBeurt;
            Bord = Spel.ConvertBordToString(spel.Bord);
            Winnaar = spel.Winnaar;
        }


        public static IEnumerable<SpelTbvJson> ConvertList(IEnumerable<Spel> spelLijst)
        {
            List< SpelTbvJson> list = new();
            foreach(Spel spel in spelLijst)
            {
                list.Add(new SpelTbvJson(spel));
            }
            return list;
        }
    }
}
