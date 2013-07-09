using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace August2008.Model
{
    public class HeroPhoto
    {
        public int HeroPhotoId { get; set; }
        public string PhotoUrl { get; set; }
        public int HeroId { get; set; }
        public string ContentType { get; set; }
        public bool IsThumbnail { get; set; }
        public DateTime DateCreated { get; set; }
        public int UpdatedBy { get; set; }
        public long Version { get; set; }
    }
}
