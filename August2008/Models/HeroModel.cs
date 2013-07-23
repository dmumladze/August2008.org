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
        public DateTime DateUpdated { get; set; }
        public int UpdatedBy { get; set; }
        public int LanguageId { get; set; }
        public string Thumbnail { get; set; }
        public List<HeroPhoto> Photos { get; set; }

        public MilitaryRank MilitaryRank { get; set; }
        public MilitaryGroup MilitaryGroup { get; set; }

        [Required]
        [Display(Name="FirstName", ResourceType = typeof(Form))]
        public string FirstName { get; set; }

        [Required]
        [Display(Name="LastName", ResourceType = typeof(Form))]
        public string LastName { get; set; }

        [Display(Name="MiddleName", ResourceType = typeof(Form))]
        public string MiddleName { get; set; }

        [Display(Name="Dob", ResourceType = typeof(Form))]
        public DateTime? Dob { get; set; }

        [Display(Name="Died", ResourceType = typeof(Form))]
        public DateTime? Died { get; set; }

        [Display(Name="Biography", ResourceType = typeof(Form))]
        public string Biography { get; set; }     

        [Display(Name = "MilitaryRank", ResourceType = typeof(Form))]
        public SelectList MilitaryRanks { get; set; }

        [Display(Name = "MilitaryGroup", ResourceType = typeof(Form))]
        public SelectList MilitaryGroups { get; set; }

        public bool IsNew
        {
            get { return !HeroId.HasValue; }
        }
    }
}