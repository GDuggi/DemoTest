using System;
using System.IO;
using System.Reflection;

namespace InboundFileProcessor
{
	/// <summary>
	/// Summary description for Logging.
	/// </summary>
	public class Logging
	{
		FileStream fs;
		StreamWriter sw;
		string m_path;
		string m_file="";

        public string logFileName;

		public Logging(string path)
		{
            Init(path);
		}

        public Logging()
        {
            //Following is necessary for current path to be recognized from within Service App.
            string path = Assembly.GetExecutingAssembly().Location;
            
            FileInfo fileInfo = new FileInfo(path);
            string dirName = fileInfo.DirectoryName + "\\log";
            if (!Directory.Exists(dirName))
                Directory.CreateDirectory(dirName);
            Init(dirName);
        }
        
        private void Init(string path)
        {
            m_path = path;
            if (!m_path.EndsWith("\\"))
            {
                m_path = m_path + "\\";
            }
        }
        
		public void WriteToLog(string msg)
        {
            WriteToLog(msg, logFileName);
        }


		public void WriteToLog(string msg, string fname)
		{
			try
			{
                if (fname != null && logFileName == null) //jvc ensure filename
                {
                    logFileName = fname;
                }
				String file = m_path + string.Format("{0:yyyyMMdd}" , DateTime.Now) + "_" + fname + ".txt";
				if (file != m_file)
				{
					m_file= file;									
					if (fs!=null)
					{
						try
						{
							fs.Close();
						}
						catch(Exception e)
						{
							Console.WriteLine(e.ToString());

						}
					}
					fs = new FileStream(m_file,FileMode.Append,FileAccess.Write,FileShare.ReadWrite);
					sw = new StreamWriter(fs);
					sw.WriteLine("*******************************************************");	
					sw.WriteLine("*   Datetime: " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());
					sw.WriteLine("*******************************************************");	
					sw.Flush();

				}
				try
				{
                    sw.WriteLine(DateTime.Now.ToLongTimeString() + " " + msg);
                }
				catch(Exception e1)
				{
					sw.WriteLine("{0,8:hh:mm:ss}" , DateTime.Now);	
					sw.WriteLine(msg + "; " +e1.Message);	
				}
				sw.Flush();
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
			}

		}


	}
}
