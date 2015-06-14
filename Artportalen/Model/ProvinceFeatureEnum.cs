namespace Artportalen.Model
{
    using System.ComponentModel.DataAnnotations;

    public enum ProvinceFeatureEnum
    {
        [Display(Name = "", ShortName = "")]Undefined = 0,
        [Display(Name = "Skåne", ShortName = "Sk")]Skåne = 1,
        [Display(Name = "Blekinge", ShortName = "Bl")]Blekinge = 2,
        [Display(Name = "Småland", ShortName = "Sm")]Småland = 3,
        [Display(Name = "Öland", ShortName = "Öl")]Öland = 4,
        [Display(Name = "Gotland", ShortName = "Go")]Gotland = 5,
        [Display(Name = "Halland", ShortName = "Hl")]Halland = 6,
        [Display(Name = "Bohuslän", ShortName = "Bo")]Bohuslän = 7,
        [Display(Name = "Dalsland", ShortName = "Ds")]Dalsland = 8,
        [Display(Name = "Västergötland", ShortName = "Vg")]Västergötland = 9,
        [Display(Name = "Närke", ShortName = "Nä")]Närke = 10,
        [Display(Name = "Östergötland", ShortName = "Ög")]Östergötland = 11,
        [Display(Name = "Södermanland", ShortName = "Sö")]Södermanland = 12,
        [Display(Name = "Uppland", ShortName = "Up")]Uppland = 13,
        [Display(Name = "Västmanland", ShortName = "Vs")]Västmanland = 14,
        [Display(Name = "Värmland", ShortName = "Vr")]Värmland = 15,
        [Display(Name = "Dalarna", ShortName = "Dr")]Dalarna = 16,
        [Display(Name = "Gästrikland", ShortName = "Gä")]Gästrikland = 17,
        [Display(Name = "Hälsingland", ShortName = "Hs")]Hälsingland = 18,
        [Display(Name = "Medelpad", ShortName = "Me")]Medelpad = 19,
        [Display(Name = "Ångermanland", ShortName = "Ån")]Ångermanland = 20,
        [Display(Name = "Västerbotten", ShortName = "Vb")]Västerbotten = 21,
        [Display(Name = "Norrbotten", ShortName = "Nb")]Norrbotten = 22,
        [Display(Name = "Härjedalen", ShortName = "Hr")]Härjedalen = 23,
        [Display(Name = "Jämtland", ShortName = "Jä")]Jämtland = 24,
        [Display(Name = "Åsele lappmark", ShortName = "Ås")]ÅseleLappmark = 25,
        [Display(Name = "Lycksele lappmark", ShortName = "Ly")]LyckseleLappmark = 26,
        [Display(Name = "Pite lappmark", ShortName = "Pi")]PiteLappmark = 27,
        [Display(Name = "Lule lappmark", ShortName = "Lu")]LuleLappmark = 28,
        [Display(Name = "Torne lappmark", ShortName = "Sk")]TorneLappmark = 29,
    }
}
