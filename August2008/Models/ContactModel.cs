using System;
using System.ComponentModel.DataAnnotations;
using Resources.Home;

namespace August2008.Models
{
    public class ContactModel
    {
        [Required]
        [Display(Name = "Name", ResourceType = typeof(Labels))]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Email", ResourceType = typeof(Labels))]
        public string Email { get; set; }         
        [Required]
        [Display(Name = "Subject", ResourceType = typeof(Labels))]
        public string Subject { get; set; }
        [Required]
        [MaxLength(500)]
        [Display(Name = "Message", ResourceType = typeof(Labels))]
        public string Message { get; set; }
    }
}