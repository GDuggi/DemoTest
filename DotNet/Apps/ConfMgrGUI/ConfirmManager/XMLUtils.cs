using System;
using System.Collections.Generic;
using System.Text;

namespace ConfirmManager
{
   class XMLUtils
   {
      static public readonly int TAG_OPEN_CLOSED = 0;
      static public readonly int TAG_OPEN = 1;
      static public readonly int TAG_CLOSED = 2;
      static public readonly string XML_HEADER = "<?xml version = \"1.0\" encoding=\"UTF-8\" ?>" + Environment.NewLine;

      static public string GetEntityRefs(string AValue)
      {
         string value = AValue;
         value = value.Replace("&", "&amp;");
         value = value.Replace("<", "&lt;");
         value = value.Replace(">", "&gt;");
         value = value.Replace("'", "&apos;");
         value = value.Replace("\"", "&quot;");
         return value;
      }

      static public string BuildTagItem(int ATabs, string ATagName, string ATagContent,
                                      int ATagStyle, String AAttributes)
      {
         const String CRLF = "\r\n";
         string tagItem = "";
         string startTag = "";
         if (AAttributes.Length > 0)
            startTag = GetTabString(ATabs) + "<" + ATagName + " " + AAttributes + ">";
        else
            startTag = GetTabString(ATabs) + "<" + ATagName + ">";

        if (ATagStyle == TAG_OPEN)
            tagItem = startTag + CRLF;
        else if (ATagStyle == TAG_CLOSED)
            tagItem = GetTabString(ATabs) + "</" + ATagName + ">" + CRLF;
        else
            tagItem = startTag + GetEntityRefs(ATagContent) + "</" + ATagName + ">" + CRLF;

        return tagItem;
      }

      static private string GetTabString(int ATabs)
      {
         string tabString = "";
         for (int i = 0; i < ATabs; i++)
            tabString += "\t"; 

         return tabString;
      }

   }
}
