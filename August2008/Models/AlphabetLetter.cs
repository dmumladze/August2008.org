using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace August2008.Models
{
    public class AlphabetLetter
    {
        public AlphabetLetter(string letter, bool active)
        {
            Letter = letter;
            Active = active;
        }
        public string Letter { get; set; }
        public bool Active { get; set; }
    }
}