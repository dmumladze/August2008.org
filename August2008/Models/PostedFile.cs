using System;
using System.Web;
using System.Drawing;
using August2008.Common.Interfaces;

namespace August2008.Models
{
    public class PostedFile : IHttpPostedFile, IDisposable
    {
        private HttpPostedFileBase _postedFile;
        private Bitmap _bitmap;
        private bool _isDisposed;

        public PostedFile(HttpPostedFileBase postedFile)
        {
            _postedFile = postedFile;
        }
        public void SaveAs(string fileName)
        {
            if (_postedFile != null && _postedFile.InputStream.Length != 0)
            {
                _postedFile.SaveAs(fileName);
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
                    return _postedFile.FileName;
                }
                return string.Empty;
            }
        }
        public int Width
        {
            get
            {
                if (this.EnsureBitmap())
                {
                    return _bitmap.Width;
                }
                return 0;
            }
        }
        public int Height
        {
            get
            {
                if (this.EnsureBitmap())
                {
                    return _bitmap.Height;
                }
                return 0;
            }
        }
        public bool CanSave
        {
            get { return _postedFile != null && _postedFile.InputStream.Length > 0; }
        }
        public void Dispose()
        {
            if (!_isDisposed)
            {
                if (_bitmap != null)
                {
                    _bitmap.Dispose();
                }
                _bitmap = null;
                _postedFile = null;
                _isDisposed = true;                
            }
            GC.SuppressFinalize(this);
        }
    }
}