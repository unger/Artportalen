namespace Artportalen.Model
{
    using System.ComponentModel.DataAnnotations;

    public enum ProvinceEnum
    {
        [Display(Name = "", ShortName = "")]Undefined = 0,
        [Display(Name = "Blekinge", ShortName = "Bl")]Blekinge = 8059,
        [Display(Name = "Bohuslän", ShortName = "Bo")]Bohuslän = 8064,
        [Display(Name = "Dalarna", ShortName = "Dr")]Dalarna = 8073,
        [Display(Name = "Dalsland", ShortName = "Ds")]Dalsland = 8065,
        [Display(Name = "Gotland", ShortName = "Go")]Gotland = 8062,
        [Display(Name = "Gästrikland", ShortName = "Gä")]Gästrikland = 8074,
        [Display(Name = "Halland", ShortName = "Hl")]Halland = 8063,
        [Display(Name = "Hälsingland", ShortName = "Hs")]Hälsingland = 8075,
        [Display(Name = "Härjedalen", ShortName = "Hr")]Härjedalen = 8080,
        [Display(Name = "Jämtland", ShortName = "Jä")]Jämtland = 8081,
        [Display(Name = "Lule lappmark", ShortName = "Lu")]LuleLappmark = 8085,
        [Display(Name = "Lycksele lappmark", ShortName = "Ly")]LyckseleLappmark = 8083,
        [Display(Name = "Medelpad", ShortName = "Me")]Medelpad = 8076,
        [Display(Name = "Norrbotten", ShortName = "Nb")]Norrbotten = 8079,
        [Display(Name = "Närke", ShortName = "Nä")]Närke = 8067,
        [Display(Name = "Pite lappmark", ShortName = "Pi")]PiteLappmark = 8084,
        [Display(Name = "Skåne", ShortName = "Sk")]Skåne = 8058,
        [Display(Name = "Småland", ShortName = "Sm")]Småland = 8060,
        [Display(Name = "Södermanland", ShortName = "Sö")]Södermanland = 8069,
        [Display(Name = "Torne lappmark", ShortName = "Sk")]TorneLappmark = 8086,
        [Display(Name = "Uppland", ShortName = "Up")]Uppland = 8070,
        [Display(Name = "Värmland", ShortName = "Vr")]Värmland = 8072,
        [Display(Name = "Västerbotten", ShortName = "Vb")]Västerbotten = 8078,
        [Display(Name = "Västergötland", ShortName = "Vg")]Västergötland = 8066,
        [Display(Name = "Västmanland", ShortName = "Vs")]Västmanland = 8071,
        [Display(Name = "Ångermanland", ShortName = "Ån")]Ångermanland = 8077,
        [Display(Name = "Åsele lappmark", ShortName = "Ås")]ÅseleLappmark = 8082,
        [Display(Name = "Öland", ShortName = "Öl")]Öland = 8061,
        [Display(Name = "Östergötland", ShortName = "Ög")]Östergötland = 8068,
    }
}
