using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace August2008.Common.Interfaces
{
    public interface IPostedFile
    {
        string FileName { get; }
        string ContentType { get; }
        int Width { get; }
        int Height { get; }

        bool CanSave { get; }

        bool Validate(int maxWidth = 0, int maxHeight = 0);
        void Save(string fileName = null);

        Dictionary<string, string> Attributes { get; }
    }
}
