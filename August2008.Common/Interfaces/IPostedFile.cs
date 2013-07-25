using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace August2008.Common.Interfaces
{
    public interface IPostedFile
    {
        string FileName { get; }
        string ContentType { get; }
        bool CanSave { get; }

        Stream Stream { get; }
        Dictionary<string, string> Attributes { get; }

        void Save(string fileName = null);
    }
}
