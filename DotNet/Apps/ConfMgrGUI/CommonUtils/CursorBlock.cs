using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommonUtils
{
    public class CursorBlock : IDisposable
    {
        private readonly Cursor originalCursor;
        private volatile bool isDisposed;

        public CursorBlock(Cursor newCursor, Cursor orig = null)
        {
            originalCursor = orig ?? Cursor.Current;
            Cursor.Current = newCursor;
        }

        public void Dispose()
        {
            lock (this)
            {
                if (isDisposed) return;
                isDisposed = true;
                Cursor.Current = originalCursor;
            }            
        }
    }
}
