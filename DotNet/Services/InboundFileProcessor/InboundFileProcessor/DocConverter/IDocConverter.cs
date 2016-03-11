using System;
using System.Collections.Generic;
using System.Text;

namespace InboundFileProcessor
{
    
    public interface IDocConverter
    {
        void Convert(string origFile, string fromFile, string outputDir, string processedDir, string callerRef, string sentTo);
    }

}
