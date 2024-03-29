﻿using ReversiAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace ReversiTestAPI.Model
{
    public class Spel : ISpel
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

        private Kleur[,] bord;
        [NotMapped]
        public Kleur[,] Bord
        {
            get{ return bord; }
            set{ bord = value; }
        }

        public IEnumerable<Spel> ConvertList(IEnumerable<SpelTbvJson> oldSpellen)
        {
            List<Spel> spellen = new();
            foreach (SpelTbvJson spel in oldSpellen)
            {
                spellen.Add(new Spel(spel));
            }
            return spellen;
        }

        public Kleur AandeBeurt { get; set; }
        public Spel()
        {
            Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            Token = Token.Replace("/", "q");    // slash mijden ivm het opvragen van een spel via een api obv het token
            Token = Token.Replace("+", "r");    // plus mijden ivm het opvragen van een spel via een api obv het token

            Bord = new Kleur[bordOmvang, bordOmvang];
            Bord[3, 3] = Kleur.Wit;
            Bord[4, 4] = Kleur.Wit;
            Bord[3, 4] = Kleur.Zwart;
            Bord[4, 3] = Kleur.Zwart;

            Winnaar = (int)Kleur.Geen;
            AandeBeurt = Kleur.Wit;
        }
        public Spel(SpelTbvJson spel)
        {
            if (spel == null) 
                throw new Exception("Geen spel");
            ID = spel.ID;
            Omschrijving = spel.Omschrijving;
            Token = spel.Token;
            Speler1Token = spel.Speler1Token;
            Speler2Token = spel.Speler2Token;
            AandeBeurt = spel.AandeBeurt;
            Bord = ConvertStringToBord(spel.Bord);
            Winnaar = spel.Winnaar;
        }

        public static Kleur[,] ConvertStringToBord(string _bord)
        {
            Kleur[,] _spelbord = new Kleur[bordOmvang, bordOmvang];
            for (int rijZet = 0; rijZet < bordOmvang; rijZet++)
            {
                for (int kolomZet = 0; kolomZet < bordOmvang; kolomZet++)
                {
                    int index = rijZet * bordOmvang + kolomZet;
                    if (index >= _bord.Length) 
                        return _spelbord;
                    _spelbord[rijZet, kolomZet] = (Kleur)(_bord[index] - 48);
                }
            }
            return _spelbord;
        }
        public static string ConvertBordToString(Kleur[,] spel)
        {
            string _bord = "";
            for (int rijZet = 0; rijZet < bordOmvang; rijZet++)
                for (int kolomZet = 0; kolomZet < bordOmvang; kolomZet++)
                    _bord += (int)spel[rijZet, kolomZet];
            return _bord;
        }

        public void Pas()
        {
            // controleeer of er geen zet mogelijk is voor de speler die wil passen, alvorens van beurt te wisselen.
            if (IsErEenZetMogelijk(AandeBeurt))
                throw new Exception("Passen mag niet, er is nog een zet mogelijk");
            else
                WisselBeurt();
        }


        public bool Afgelopen()     // return true als geen van de spelers een zet kan doen
        {
            return !(IsErEenZetMogelijk(Kleur.Wit) || IsErEenZetMogelijk(Kleur.Zwart));
        }

        public Kleur OverwegendeKleur()
        {
            int aantalWit = 0;
            int aantalZwart = 0;
            for (int rijZet = 0; rijZet < bordOmvang; rijZet++)
            {
                for (int kolomZet = 0; kolomZet < bordOmvang; kolomZet++)
                {
                    if (bord[rijZet, kolomZet] == Kleur.Wit)
                        aantalWit++;
                    else if (bord[rijZet, kolomZet] == Kleur.Zwart)
                        aantalZwart++;
                }
            }
            if (aantalWit > aantalZwart)
                return Kleur.Wit;
            if (aantalZwart > aantalWit)
                return Kleur.Zwart;
            return Kleur.Geen;
        }

        public bool ZetMogelijk(int rijZet, int kolomZet)
        {
            if (!PositieBinnenBordGrenzen(rijZet, kolomZet))
                throw new Exception($"Zet ({rijZet},{kolomZet}) ligt buiten het bord!");
            return ZetMogelijk(rijZet, kolomZet, AandeBeurt);
        }

        public void DoeZet(int rijZet, int kolomZet)
        {
            if (!ZetMogelijk(rijZet, kolomZet)) 
                throw new Exception($"Zet ({rijZet},{kolomZet}) is niet mogelijk!");
            for (int i = 0; i < 8; i++)
            {
                DraaiStenenVanTegenstanderInOpgegevenRichtingOmIndienIngesloten(rijZet, kolomZet, AandeBeurt, richting[i, 0], richting[i, 1]);
            }
            Bord[rijZet, kolomZet] = AandeBeurt;
            WisselBeurt();
            if (Afgelopen())
                Winnaar = (int)OverwegendeKleur();
        }

        private static Kleur GetKleurTegenstander(Kleur kleur)
        {
            if (kleur == Kleur.Wit)
                return Kleur.Zwart;
            else if (kleur == Kleur.Zwart)
                return Kleur.Wit;
            return Kleur.Geen;
        }

        private bool IsErEenZetMogelijk(Kleur kleur)
        {
            if (kleur == Kleur.Geen)
                throw new Exception("Kleur mag niet gelijk aan Geen zijn!");
            for (int rijZet = 0; rijZet < bordOmvang; rijZet++)
                for (int kolomZet = 0; kolomZet < bordOmvang; kolomZet++)
                    if (ZetMogelijk(rijZet, kolomZet, kleur))
                        return true;
            return false;
        }

        private bool ZetMogelijk(int rijZet, int kolomZet, Kleur kleur)
        {
            for (int i = 0; i < 8; i++)
                if (StenenInTeSluitenInOpgegevenRichting(rijZet, kolomZet, kleur, richting[i, 0], richting[i, 1]))
                    return true;
            return false;
        }

        private void WisselBeurt()
        {
            if (AandeBeurt == Kleur.Wit)
                AandeBeurt = Kleur.Zwart;
            else
                AandeBeurt = Kleur.Wit;
        }

        private static bool PositieBinnenBordGrenzen(int rij, int kolom)
        {
            return (rij >= 0 && rij < bordOmvang &&
                    kolom >= 0 && kolom < bordOmvang);
        }

        private bool ZetOpBordEnNogVrij(int rijZet, int kolomZet)
        {
            return (PositieBinnenBordGrenzen(rijZet, kolomZet) && Bord[rijZet, kolomZet] == Kleur.Geen);
        }

        private bool StenenInTeSluitenInOpgegevenRichting(int rijZet, int kolomZet, Kleur kleurZetter, int rijRichting, int kolomRichting)
        {
            int rij, kolom;
            Kleur kleurTegenstander = GetKleurTegenstander(kleurZetter);
            if (!ZetOpBordEnNogVrij(rijZet, kolomZet))
                return false;

            // Zet rij en kolom op de index voor het eerst vakje naast de zet.
            rij = rijZet + rijRichting;
            kolom = kolomZet + kolomRichting;

            int aantalNaastGelegenStenenVanTegenstander = 0;
            // Zolang Bord[rij,kolom] niet buiten de bordgrenzen ligt, en je in het volgende vakje 
            // steeds de kleur van de tegenstander treft, ga je nog een vakje verder kijken.
            // Bord[rij, kolom] ligt uiteindelijk buiten de bordgrenzen, of heeft niet meer de
            // de kleur van de tegenstander.
            // N.b.: deel achter && wordt alleen uitgevoerd als conditie daarvoor true is.
            while (PositieBinnenBordGrenzen(rij, kolom) && Bord[rij, kolom] == kleurTegenstander)
            {
                rij += rijRichting;
                kolom += kolomRichting;
                aantalNaastGelegenStenenVanTegenstander++;
            }

            // Nu kijk je hoe je geeindigt bent met bovenstaande loop. Alleen
            // als alle drie onderstaande condities waar zijn, zijn er in de
            // opgegeven richting stenen in te sluiten.
            return (PositieBinnenBordGrenzen(rij, kolom) &&
                    Bord[rij, kolom] == kleurZetter &&
                    aantalNaastGelegenStenenVanTegenstander > 0);
        }

        private bool DraaiStenenVanTegenstanderInOpgegevenRichtingOmIndienIngesloten(int rijZet, int kolomZet,
                                                                                     Kleur kleurZetter,
                                                                                     int rijRichting, int kolomRichting)
        {
            int rij, kolom;
            Kleur kleurTegenstander = GetKleurTegenstander(kleurZetter);
            bool stenenOmgedraaid = false;

            if (StenenInTeSluitenInOpgegevenRichting(rijZet, kolomZet, kleurZetter, rijRichting, kolomRichting))
            {
                rij = rijZet + rijRichting;
                kolom = kolomZet + kolomRichting;

                while (Bord[rij, kolom] == kleurTegenstander)
                {
                    Bord[rij, kolom] = kleurZetter;
                    rij += rijRichting;
                    kolom += kolomRichting;
                }
                stenenOmgedraaid = true;
            }
            return stenenOmgedraaid;
        }
    }
}
