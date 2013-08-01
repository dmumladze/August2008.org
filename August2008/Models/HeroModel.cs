using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using August2008.Model;
using Resources.Hero;

namespace August2008.Models
{
    public class HeroModel
    {
        public int? HeroId { get; set; }      
        public int? MilitaryRankId { get; set; }
        public int? MilitaryGroupId { get; set; }
        public int? MilitaryAwardId { get; set; } 
        public DateTime DateUpdated { get; set; }
        public int UpdatedBy { get; set; }
        public int LanguageId { get; set; }
        public string Thumbnail { get; set; }
        public List<HeroPhoto> Photos { get; set; }

        [Display(Name = "MilitaryRank", ResourceType = typeof(Labels))]
        public MilitaryRank MilitaryRank { get; set; }

        [Display(Name = "MilitaryGroup", ResourceType = typeof(Labels))]
        public MilitaryGroup MilitaryGroup { get; set; }

        [Display(Name = "MilitaryAward", ResourceType = typeof(Labels))]
        public MilitaryAward MilitaryAward { get; set; }

        [Required]
        [Display(Name="FirstName", ResourceType = typeof(Labels))]
        public string FirstName { get; set; }

        [Required]
        [Display(Name="LastName", ResourceType = typeof(Labels))]
        public string LastName { get; set; }

        [Display(Name="MiddleName", ResourceType = typeof(Labels))]
        public string MiddleName { get; set; }

        [Display(Name="Dob", ResourceType = typeof(Labels))]
        public DateTime? Dob { get; set; }

        [Display(Name="Died", ResourceType = typeof(Labels))]
        public DateTime? Died { get; set; }

        [Display(Name="Biography", ResourceType = typeof(Labels))]
        public string Biography { get; set; }     

        [Display(Name = "MilitaryRank", ResourceType = typeof(Labels))]
        public SelectList MilitaryRanks { get; set; }

        [Display(Name = "MilitaryGroup", ResourceType = typeof(Labels))]
        public SelectList MilitaryGroups { get; set; }

        [Display(Name = "MilitaryAward", ResourceType = typeof(Labels))]
        public SelectList MilitaryAwards { get; set; } 

        public bool IsNew
        {
            get { return !HeroId.HasValue; }
        }
    }
}