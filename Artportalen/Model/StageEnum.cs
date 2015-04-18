namespace Artportalen.Model
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public enum StageEnum
    {
        [Display(Name = "", ShortName = "")]Undefined = 0,
        [Display(Name = "Ägg", ShortName = "ägg")]Ägg = 1,
        [Display(Name = "Pulli", ShortName = "pull")]Pulli = 2,
        [Display(Name = "Adult", ShortName = "ad")]Adult = 23,
        [Display(Name = "1K", ShortName = "1K")]S1K = 3,
        [Display(Name = "1K+", ShortName = "1K+")]S1Kplus = 4,
        [Display(Name = "2K", ShortName = "2K")]S2K = 5,
        [Display(Name = "2K+", ShortName = "2K+")]S2Kplus = 6,
        [Display(Name = "2K-", ShortName = "2K-")]S2Kminus = 7,
        [Display(Name = "3K", ShortName = "3K")]S3K = 8,
        [Display(Name = "3K+", ShortName = "3K+")]S3Kplus = 9,
        [Display(Name = "3K-", ShortName = "3K-")]S3Kminus = 10,
        [Display(Name = "4K", ShortName = "4K")]S4K = 11,
        [Display(Name = "4K+", ShortName = "4K+")]S4Kplus = 12,
        [Display(Name = "4K-", ShortName = "4K-")]S4Kminus = 13,
        [Display(Name = "5K", ShortName = "5K")]S5K = 14,
        [Display(Name = "5K+", ShortName = "5K+")]S5Kplus = 15,
        [Display(Name = "5K-", ShortName = "5K-")]S5Kminus = 16,
        [Display(Name = "6K", ShortName = "6K")]S6K = 17,
        [Display(Name = "6K+", ShortName = "6K+")]S6Kplus = 18,
        [Display(Name = "6K-", ShortName = "6K-")]S6Kminus = 19,
        [Display(Name = "7K", ShortName = "7K")]S7K = 20,
        [Display(Name = "7K+", ShortName = "7K+")]S7Kplus = 21,
        [Display(Name = "7K-", ShortName = "7K-")]S7Kminus = 22,
    }
}
