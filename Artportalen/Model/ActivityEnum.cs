﻿using System.ComponentModel.DataAnnotations;

namespace Artportalen.Model
{
    public enum ActivityEnum
    {
        [Display(Name = "", ShortName = "")]Undefined = 0,
        [Display(Name = "Bo, ägg/ungar", ShortName = "bo, ägg/ungar")]BoÄggUngar = 1,
        [Display(Name = "Bo, hörda ungar", ShortName = "bo, hörda ungar")]BoHördaUngar = 2,
        [Display(Name = "Misslyckad häckning", ShortName = "misslyckad häckning")]MisslyckadHäckning = 3,
        [Display(Name = "Ruvande", ShortName = "ruvande")]Ruvande = 4,
        [Display(Name = "Äggskal", ShortName = "äggskal")]Äggskal = 5,
        [Display(Name = "Föda åt ungar", ShortName = "föda åt ungar")]FödaÅtUngar = 6,
        [Display(Name = "Bär exkrementsäck", ShortName = "bär exkrementsäck")]BärExkrementsäck = 7,
        [Display(Name = "Besöker bebott bo", ShortName = "besöker bebott bo")]BesökerBebottBo = 8,
        [Display(Name = "Pulli/nyligen flygga ungar", ShortName = "pulli/nyligen flygga ungar")]PulliNyligenFlyggaUngar = 9,
        [Display(Name = "Nyligen använt bo", ShortName = "nyligen använt bo")]NyligenAnväntBo = 10,
        [Display(Name = "Avledningsbeteende", ShortName = "avledningsbeteende")]Avledningsbeteende = 11,
        [Display(Name = "Bobygge", ShortName = "bobygge")]Bobygge = 12,
        [Display(Name = "Ruvfläckar", ShortName = "ruvfläckar")]Ruvfläckar = 13,
        [Display(Name = "Upprörd, varnande", ShortName = "upprörd, varnande")]UpprördVarnande = 14,
        [Display(Name = "Bobesök?", ShortName = "bobesök")]Bobesök = 15,
        [Display(Name = "Parning/parningsceremonier", ShortName = "parning/parningsceremonier")]ParningParningsceremonier = 16,
        [Display(Name = "Permanent revir", ShortName = "permanent revir")]PermanentRevir = 17,
        [Display(Name = "Par i lämplig häckbiotop", ShortName = "par i lämplig häckbiotop")]ParILämpligHäckbiotop = 18,
        [Display(Name = "Spel/sång", ShortName = "spel/sång")]SpelSång = 19,
        [Display(Name = "Obs i häcktid, lämplig biotop", ShortName = "obs i häcktid, lämplig biotop")]ObsIHäcktidLämpligBiotop = 20,
        [Display(Name = "Rastande", ShortName = "rast")]Rastande = 22,
        [Display(Name = "Stationär", ShortName = "stationär")]Stationär = 23,
        [Display(Name = "Förbiflygande", ShortName = "förbifl.")]Förbiflygande = 24,
        [Display(Name = "Födosökande", ShortName = "födosökande")]Födosökande = 25,
        [Display(Name = "Spel/sång, ej häckning", ShortName = "spel/sång, ej häckning")]SpelSångEjHäckning = 26,
        [Display(Name = "Lockläte, övriga läten", ShortName = "lockläte")]LockläteÖvrigaLäten = 27,
        [Display(Name = "Revir, ej häckning", ShortName = "revir, ej häckning")]RevirEjHäckning = 28,
        [Display(Name = "Ringmärkt", ShortName = "ringm")]Ringmärkt = 29,
        [Display(Name = "Individmärkt", ShortName = "individmärkt")]Individmärkt = 30,
        [Display(Name = "Sträckförsök", ShortName = "sträckförsök")]Sträckförsök = 31,
        [Display(Name = "Sträckande", ShortName = "str")]Sträckande = 32,
        [Display(Name = "Sträckande N", ShortName = "str N")]SträckandeN = 33,
        [Display(Name = "Sträckande NO", ShortName = "str NO")]SträckandeNO = 34,
        [Display(Name = "Sträckande O", ShortName = "str O")]SträckandeO = 35,
        [Display(Name = "Sträckande SO", ShortName = "str SO")]SträckandeSO = 36,
        [Display(Name = "Sträckande S", ShortName = "str S")]SträckandeS = 37,
        [Display(Name = "Sträckande SV", ShortName = "str SV")]SträckandeSV = 38,
        [Display(Name = "Sträckande V", ShortName = "str V")]SträckandeV = 39,
        [Display(Name = "Sträckande NV", ShortName = "str NV")]SträckandeNV = 40,
        [Display(Name = "Död, krockat med kraftledning", ShortName = "död, krockat med kraftledning")]DödKrockatMedKraftledning = 68,
        [Display(Name = "Död, krockat med vindkraftverk", ShortName = "död, krockat med vindkraftverk")]DödKrockatMedVindkraftverk = 69,
        [Display(Name = "Död, krockat med fönster", ShortName = "död, krockat med fönster")]DödKrockatMedFönster = 70,
        [Display(Name = "Död, krockat med fyr", ShortName = "död, krockat med fyr")]DödKrockatMedFyr = 71,
        [Display(Name = "Trafikdödad", ShortName = "trafikdödad")]Trafikdödad = 72,
        [Display(Name = "Död, krockat med flygplan", ShortName = "död, krockat med flygplan")]DödKrockatMedFlygplan = 73,
        [Display(Name = "Död, krockat med staket", ShortName = "död, krockat med staket")]DödKrockatMedStaket = 74,
        [Display(Name = "Dödad av elektricitet", ShortName = "dödad av elektricitet")]DödadAvElektricitet = 75,
        [Display(Name = "Drunknad i fiskenät", ShortName = "drunknad i fiskenät")]DrunknadIFiskenät = 76,
        [Display(Name = "Dödad av predator", ShortName = "dödad av predator")]DödadAvPredator = 77,
        [Display(Name = "Död av sjukdom/svält", ShortName = "död av sjukdom/svält")]DödAvSjukdomSvält = 78,
        [Display(Name = "Funnen död", ShortName = "funnen död")]FunnenDöd = 41,
        [Display(Name = "Dödad av olja", ShortName = "dödad av olja")]DödadAvOlja = 102,
        [Display(Name = "Färska spår", ShortName = "färska spår")]FärskaSpår = 42,
        [Display(Name = "Äldre spår", ShortName = "äldre spår")]ÄldreSpår = 43,
        [Display(Name = "Färsk spillning", ShortName = "färsk spillning")]FärskSpillning = 44,
        [Display(Name = "Äldre spillning", ShortName = "äldre spillning")]ÄldreSpillning = 45,
    }
}
