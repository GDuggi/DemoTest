using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace Sempra
{
	/// <summary>
	/// Builds/Maintains an XML list of login history information
	/// </summary>
	public class LoginHistory
	{
		private string xmlFileLocation;
		private string xmlSchemaLocation;
		private int historyLength;

		/// <param name="XmlSchema">Full path to directory where XML schemas are stored</param>
		/// <param name="XmlFile">Full path and file name of XML that contains or will contain
		/// the history of login information</param>
		public LoginHistory( string XmlSchema, string XmlFile )
		{
			xmlSchemaLocation = XmlSchema;
			xmlFileLocation = XmlFile;
		}

		///<summary>Adds a user's login information to the history XML</summary>
		///<param name="Username">Username to add</param>
		///<param name="Server">Database to add</param>
		public void AddLogin( string Username, string Server, string Database )
		{
			//Always puts the login being added at the top of the list
			StringCollection logins = GetLogins();
			string newLogin = Username + ";" + Server + ";" + Database;
			int oldIndex = logins.IndexOf( newLogin );

			//find out if the old list had this login
			if ( logins.Contains( newLogin ) )
			{
				if ( oldIndex == 0 ) //is it already at the top of the list?
					return;			 //if so, we don't need to add it - all done
				
				//if not at top, remove it so it can be inserted at the top
				logins.RemoveAt( oldIndex );
			}

			//Add it to the top of the list
			logins.Insert( 0, newLogin );

			//If the file had existed, get rid of it, so we can build a new one from the old one's
			//   contents, already in memory now
			if( File.Exists( XmlFileLocation ) )
				File.Delete( xmlFileLocation );

			XmlTextWriter writer = new XmlTextWriter(XmlFileLocation, null);
			writer.Formatting = Formatting.Indented;
			
			writer.WriteStartDocument();
			writer.WriteStartElement( "LoginList" );
			writer.WriteAttributeString( "xmlns", null, "sempratrading.com/OpsDev/LoginHistory.xsd" );

			//Figure out how long the list has to be
			int lastItem;
			if ( logins.Count < historyLength )
				lastItem = logins.Count;
			else
				lastItem = historyLength;

            foreach (string login in logins)
            {
                string[] fields  = login.Split(';');
                string user = fields[0];
                string server = fields[1];
                string database = fields[2];

                writer.WriteStartElement("LoginEntry");

                writer.WriteStartElement("Username");
                writer.WriteString(user);
                writer.WriteEndElement();

                writer.WriteStartElement("Server");
                writer.WriteString(server);
                writer.WriteEndElement();

                writer.WriteStartElement("Database");
                writer.WriteString(database);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }

			//Close up the XML
			writer.WriteEndElement();
			writer.WriteEndDocument();
			writer.Close();
		}

		/// <summary>
		/// Gets a list of the login usernames and databases, each separated by a ", "
		/// </summary>
		public StringCollection GetLogins()
		{
			StringCollection result = new StringCollection();

			//If it's empty, return the empty StringCollection
			if ( !File.Exists( xmlFileLocation ) )
				return result;

			string tempStr = "";
            foreach (var loginEntry in XmlHelper.StreamLoginEntries(xmlFileLocation))
            {
                tempStr = loginEntry.UserName + ";" + loginEntry.Server + ";" + loginEntry.Database;
                result.Add(tempStr);
                tempStr = "";
            }
			return result;
		}

        public class XmlLoginEntry 
        {
            public string UserName {get; set;}
            public string Server {get; set;}
            public string Database {get; set;}
        }

        public class XmlHelper
        {
            public static IEnumerable<XmlLoginEntry> StreamLoginEntries(string uri)
            {
                using (XmlReader reader = XmlReader.Create(uri))
                {
                    string _username = null;
                    string _server = null;
                    string _database = null;

                    reader.MoveToContent();
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element
                            && reader.Name == "LoginEntry")
                        {
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Element &&
                                    reader.Name == "Username")
                                {
                                    _username = reader.ReadString();
                                    break;
                                }
                            }
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Element &&
                                    reader.Name == "Server")
                                {
                                    _server = reader.ReadString();
                                    break;
                                }
                            }
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Element &&
                                    reader.Name == "Database")
                                {
                                    _database = reader.ReadString();
                                    break;
                                }
                            }
                            yield return new XmlLoginEntry() { UserName = _username, Server = _server, Database = _database };
                        }
                    }
                }
            }
        }

		/// <summary>
		/// Gets or sets path and file name of the XML file
		/// </summary>
		public string XmlFileLocation
		{
			get
			{
				return this.xmlFileLocation;
			}

			set
			{
				this.xmlFileLocation = value;
			}
		}

		/// <summary>
		/// Gets or sets the number of items to store in the history dropdown
		/// </summary>
		public int HistoryLength
		{
			get
			{
				return this.historyLength;
			}

			set
			{
				this.historyLength = value;
			}
		}
	}
}
