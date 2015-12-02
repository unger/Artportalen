using System.ComponentModel.DataAnnotations;

namespace Artportalen.Model
{
    public enum GenderEnum
    {
        [Display(Name = "", ShortName = "")]Undefined = 0,
        [Display(Name = "Hane", ShortName = "hane")]Hane = 1,
        [Display(Name = "Hona", ShortName = "hona")]Hona = 2,
        [Display(Name = "Honfärgad", ShortName = "honf")]Honfärgad = 3,
        [Display(Name = "I par", ShortName = "i par")]Ipar = 4,
    }
}
