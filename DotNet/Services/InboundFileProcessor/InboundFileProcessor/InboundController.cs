using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InboundFileProcessor.DataAccess;
using System.Transactions;
using System.Data.SqlClient;
using System.Data;

namespace InboundFileProcessor
{
    class InboundController
    {
        public Logging _logFile;

        public InboundController()
        {
        }

        public int ProcessFile(string fileNameAndPath, string srcfileNameAndPath, string callerRef, string sentTo) //jvc will be the converted Tif
        {
            Int32 imageId = 0;

            string sqlConnectionIntegratedSecurityString = Properties.Settings.Default.SqlConnectionIntegratedSecurityString;
            
            InboundDal inboundDal = new InboundDal(sqlConnectionIntegratedSecurityString);
            inboundDal._logFile = _logFile;

            InboundDocsDto inbDocsDto = new InboundDocsDto();
            string subDir = new DirectoryInfo(srcfileNameAndPath).Parent.Name;
            inbDocsDto.Id = 0;
            inbDocsDto.CallerRef = callerRef;
            inbDocsDto.SentTo = sentTo; 
            inbDocsDto.RcvdTs = DateTime.Now; 
            inbDocsDto.FileName = Path.GetFileName(srcfileNameAndPath);
            inbDocsDto.Sender = null;
            inbDocsDto.Cmt = null;
            inbDocsDto.DocStatusCode = "OPEN"; //default
            inbDocsDto.HasAutoAsctedFlag = "N"; //default
            inbDocsDto.ProcFlag = "Y"; //default
            inbDocsDto.MappedCptySn = null;
            inbDocsDto.MappedBrkrSn = null;
            inbDocsDto.MappedCdtyCode = null;
            inbDocsDto.JobRef = null;

            InboundDocsBlobDto inbBlobDto = new InboundDocsBlobDto();
            inbBlobDto.MarkupImageFileExt = Path.GetExtension(fileNameAndPath).ToUpper(); //"TIF";
            inbBlobDto.OrigImageFileExt = Path.GetExtension(srcfileNameAndPath).ToUpper();

            imageId = inboundDal.Insert(fileNameAndPath, srcfileNameAndPath, inbDocsDto, inbBlobDto);

            return imageId;
        }

    }
}
