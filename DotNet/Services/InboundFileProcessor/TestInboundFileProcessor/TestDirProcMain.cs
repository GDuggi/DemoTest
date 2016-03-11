using InboundFileProcessor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace TestInboundFileProcessor
{
    public partial class TestDirProcMain : Form
    {
        private const string THUMBS_DB = "Thumbs.db";
        private const string ROOTDIR = "ROOTDIR";
        private string[] _scanDirs;
        private List<string> _validExtList;
        private string _outputDir;
        private string _processedDir;
        public Logging _EventLog;
        public bool _isDebugLogEnabled = false;
        private string _RootDir;
        //jvc
        private string _failedDir;

        public TestDirProcMain()
        {
            InitializeComponent();
            _EventLog = new Logging();
            _EventLog.logFileName = "EventLog";
            _EventLog.WriteToLog("EventLog has been created.");

            _validExtList =  Properties.Settings.Default.ValidFileExt.Split(',').ToList<string>();
            System.Collections.Specialized.StringCollection sc = new System.Collections.Specialized.StringCollection();
            sc = Properties.Settings.Default.ScanDirectories;
            if (sc.Count > 0)
            {
                _scanDirs = new string[sc.Count];
                sc.CopyTo(_scanDirs, 0);
            }
            else
            {
                _scanDirs = new string[1];
                _scanDirs[0] = Properties.Settings.Default.ScanDirectory;
            }
            
            tbRootDir.Text = initTestDirectory(_scanDirs[0], "Scan");
            tbOuputDir.Text = initTestDirectory(Properties.Settings.Default.OutputDirectory,"Output");
            tbFailedDir.Text = initTestDirectory(Properties.Settings.Default.FailedDirectory,"Failed");;
            tbProcessedDir.Text = initTestDirectory(Properties.Settings.Default.ProcessedDirectory, "Processed"); ;
            tbFileName.Text = Properties.Settings.Default.TestFileName;
           
        }

        private String initTestDirectory(String pDir, String pDirType)
        {
            string result = pDir;
            if (result == null || result.Trim() == "")
            {
                result = Path.GetDirectoryName(Application.ExecutablePath) + @"\InboundDocs\" +pDirType;
            }
            if (!Directory.Exists(result))
            {
                DirectoryInfo directoryInfo = Directory.CreateDirectory(result);
                result = directoryInfo.FullName;
            }
            return result;
        }

        private void btnProcessDirs_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            tssLabelStatus.Text = "Processing...";
            this.Refresh();
            try
            {
                string validFileExt = Properties.Settings.Default.ValidFileExt;

                _scanDirs[0] = initTestDirectory(this.tbRootDir.Text, "");
                _outputDir = initTestDirectory(tbOuputDir.Text, "");
                _failedDir = initTestDirectory(tbFailedDir.Text, "");
                _processedDir = initTestDirectory(tbProcessedDir.Text, "");
                DirectoryProcessor dirProcessor = new DirectoryProcessor(_scanDirs, validFileExt, _outputDir, _processedDir, _failedDir, null);
                dirProcessor._isDebugLogEnabled = true;
                dirProcessor._EventLog = _EventLog;
                dirProcessor.ProcessDirectories();
            }
            finally
            {
                this.Cursor = Cursors.Default;
                tssLabelStatus.Text = "Done.";
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fileNameAndPath = Assembly.GetExecutingAssembly().Location;
            string parent = Directory.GetParent(fileNameAndPath).Name;
            string fileNameOnly = Path.GetFileName(fileNameAndPath);
            string extOnly = Path.GetExtension(fileNameAndPath);
        }

        private void btnExtDirRead_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (string path in _scanDirs)
                {

                    if (File.Exists(path))
                    {
                        ProcessFile(path);
                    }
                    else if (Directory.Exists(path))
                    {
                        ProcessDirectory(path);
                    }
                    else
                    {
                        MessageBox.Show("Else: " + path);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory. 
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            // Recurse into subdirectories of this directory. 
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        public void ProcessFile(string fileNameAndPath)
        {
            try
            {
                string extWithPeriod = Path.GetExtension(fileNameAndPath);
                string ext = extWithPeriod.Replace(".", "");
                if (fileNameAndPath.Contains(THUMBS_DB))
                    return;
                else if (_validExtList.Contains(ext))
                {
                    //Move original data file to processed dir
                    string origNameOnly = Path.GetFileName(fileNameAndPath);
                    string datafileProcessedNameAndPath = Path.Combine(_processedDir, origNameOnly);

                    bool processedFileExists = File.Exists(datafileProcessedNameAndPath);
                    if (!processedFileExists)
                    {
                        FileUtils.MoveToDir(new FileInfo(fileNameAndPath), _processedDir);
                    }
                    else
                    {
                        File.Delete(fileNameAndPath);
                    }
                }
                else
                {
                }
            }
            catch (Exception e)
            {
                _EventLog.WriteToLog(e.Message);
            }
        }

        private string GenerateFileName()
        {
            string scannedFileName = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
            return scannedFileName;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            _RootDir = tbRootDir.Text;
            textBoxCallerRef.Text = getCallerRef(_RootDir, tbFileName.Text);
            textBoxSentTo.Text = getSentTo(tbFileName.Text);
        }


        private string getCallerRef(string pRootDir, string pFileName)
        {
            //Strip everything except immediate folder parent from pFileName
            string result = String.Empty;
            result = @pFileName.Replace(pRootDir + @"\", "");
            return result;
        }

        private string getSentTo(string pFileName){
            string result = String.Empty;
            string currentRoot = Path.GetDirectoryName(pFileName);
            DirectoryInfo di = System.IO.Directory.GetParent(pFileName);
            if (currentRoot == _RootDir)
                result = ROOTDIR;
            else
                result = di.Name;

            return result;
        }

    }
}
