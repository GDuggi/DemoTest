using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace WSAccess
{
    public class ConfirmDocsDal
    {
        const string DOCX_TESTDOC_1 = @"\\CNF01INF01\cnf01\Apps\Tools\TestDocs\0009_NGAS.PHYS.LONG.FORM.docx";
        const string DOCX_TESTDOC_2 = @"\\CNF01INF01\cnf01\Apps\Tools\TestDocs\0039_OIL.SWAP.FLO.FLO.ISDA.PARTY.B.docx";

        public byte[] GetStubDocxAuto()
        {
            byte[] resultDoc;
            resultDoc = File.ReadAllBytes(DOCX_TESTDOC_1);
            return resultDoc;
        }

        public byte[] GetStubDocxManual()
        {
            byte[] resultDoc;
            resultDoc = File.ReadAllBytes(DOCX_TESTDOC_2);
            return resultDoc;
        }

        public string GetStubTemplateList()
        {
            string filename = @"\\CNF01INF01\cnf01\Apps\Tools\TestDocs\TemplateList.xml";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filename);

            //string innerXmlText = xmlDoc.nodenodes[0].InnerText;
            //return innerXmlText;

            return xmlDoc.InnerXml;
        }

    }
}
