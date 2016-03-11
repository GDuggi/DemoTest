using System;
using System.Collections.Generic;
using System.Text;

namespace VaultUtils
{
    public interface IAXTab
    {
        void PrintDocument();
        void TransmitDocument();
        void SaveDocument(string filePath, string fileName);
    }
}
