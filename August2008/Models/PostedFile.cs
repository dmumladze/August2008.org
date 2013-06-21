using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Drawing;
using August2008.Common.Interfaces;

namespace August2008.Models
{
    public class PostedFile : IPostedFile, IDisposable
    {
        private HttpPostedFileBase _postedFile;
        private Bitmap _bitmap;
        private bool _disposed;
        private readonly Dictionary<string, string> _attributes = new Dictionary<string, string>();

        public PostedFile(HttpPostedFileBase postedFile)
        {
            _postedFile = postedFile;
        }
        public void Save(string fileName = null)
        {
            if (_postedFile != null && _postedFile.InputStream.Length != 0)
            {
                _postedFile.SaveAs(fileName ?? FileName);
            }
        }
        public bool Validate(int maxWidth = 0, int maxHeight = 0)
        {
            return ((maxWidth <= 0 || (this.Width <= maxWidth)) && (maxHeight <= 0 || (this.Height <= maxHeight)));
        }
        private bool EnsureBitmap()
        {
            if (_bitmap == null && _postedFile != null && _postedFile.InputStream.Length > 0)
            {
                _bitmap = new Bitmap(_postedFile.InputStream);
            }
            return _bitmap != null;
        }
        public string FileName
        {
            get
            {
                if (this.EnsureBitmap())
                {
                    string fileName;
                    if (!_attributes.TryGetValue("FileName", out fileName))
                    {
                        fileName = Path.GetFileName(_postedFile.FileName);
                    }
                    return fileName;
                }
                return  string.Empty;
            }
        }
        public string ContentType
        {
            get { return _postedFile.ContentType; }
        }
        public int Width
        {
            get { return this.EnsureBitmap() ? _bitmap.Width : 0; }
        }
        public int Height
        {
            get
            {
                return this.EnsureBitmap() ? _bitmap.Height : 0;
            }
        }
        public bool CanSave
        {
            get { return _postedFile != null && _postedFile.InputStream.Length > 0; }
        }
        public Dictionary<string, string> Attributes
        {
            get { return _attributes; }
        }
        public void Dispose()
        {
            if (!_disposed)
            {
                if (_bitmap != null)
                {
                    _bitmap.Dispose();
                }
                _bitmap = null;
                _postedFile = null;
                _disposed = true;                
            }
            GC.SuppressFinalize(this);
        }
    }
}