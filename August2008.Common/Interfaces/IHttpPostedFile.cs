using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace August2008.Common.Interfaces
{
    public interface IHttpPostedFile
    {
        string FileName { get; }
        int Width { get; }
        int Height { get; }

        bool CanSave { get; }

        bool Validate(int maxWidth = 0, int maxHeight = 0);
        void SaveAs(string fileName);  
    }
}
