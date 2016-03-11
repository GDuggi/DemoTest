using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Leadtools.Codecs;
using Leadtools;

namespace Sempra.Confirm.InBound.ImageEdit
{
    public class TifUtil
    {
        private const string PROJ_FILE_NAME = "TifUtil";
        public static void ExtractPages(string srcFileName, string destFileName, string pageList)
        {
            int[] pageNumList;
            validateData(srcFileName,pageList, out pageNumList) ;
            int length = pageList.Length;
            DoExtraction(srcFileName, destFileName, pageNumList,false);

        }

        public static void ExtractPagesWithAnnotation(string srcFileName, string destFileName, string pageList)
        {
            int[] pageNumList;
            validateData(srcFileName, pageList, out pageNumList);
            int length = pageList.Length;
            DoExtraction(srcFileName, destFileName, pageNumList,true);
        }

        private static void DoExtraction(string srcFileName, string destFileName, int[] pageNumList, bool annotation)
        {
            RasterCodecs codec = new RasterCodecs();
            RasterImage image = null;
            RasterTagMetadata[] tagsData = null;

            try
            {
                image = codec.Load(srcFileName);
                int pageCount = image.PageCount;
                if (annotation)
                {
                    tagsData = new RasterTagMetadata[pageCount];
                    for (int i = 0; i < pageCount; ++i)
                    {
                        tagsData[i] = codec.ReadTag(srcFileName, i + 1, RasterTagMetadata.AnnotationTiff);
                    }
                }
                int listCount = pageNumList.Length;
                if (File.Exists(destFileName))
                {
                    File.Delete(destFileName);
                }
                StatusForm statusForm = new StatusForm();
                statusForm.LoadFormData(srcFileName, destFileName, listCount);
                statusForm.Show();
                statusForm.Refresh();
                for (int i = 0; i < listCount; ++i)
                {
                    codec.Save(image, destFileName, RasterImageFormat.CcittGroup4, 1, pageNumList[i], pageNumList[i], i, CodecsSavePageMode.Append);
                    if (annotation)
                    {
                        if ((tagsData != null) && (tagsData[pageNumList[i] - 1] != null))
                        {
                            codec.WriteTag(destFileName, i + 1, tagsData[pageNumList[i]-1]);
                        }
                    }
                    statusForm.ShowPageInfo(pageNumList[i]);
                }

                if (image.PageCount > listCount)
                {
                    int deleteCount = 0;
                    for (int i = 1; i <= pageCount; ++i)
                    {
                        if (PageNumberFound(i, pageNumList))
                        {
                            image.RemovePageAt(i+deleteCount);
                            deleteCount--;
                        }
                    }
                    codec.Save(image, srcFileName, RasterImageFormat.CcittGroup4, 1, 1, image.PageCount, 1, CodecsSavePageMode.Overwrite);
                    if (annotation)
                    {
                        int newPageIndex = 0;
                        for (int i = 0; i < pageCount; ++i)
                        {
                            if (!PageNumberFound(i+1, pageNumList) && (tagsData != null) && (tagsData[i] != null))
                            {
                               codec.WriteTag(srcFileName, ++newPageIndex, tagsData[i]);
                            }
                        }
                    }
                    
                }
                else
                {
                    File.Delete(srcFileName);
                }
                statusForm.Close();

            }
            finally
            {
                if (image != null) {
                    image.Dispose();
                }
            }

        }

        private static bool PageNumberFound(int pageNumber, int[] pageNumList)
        {
            bool found = false;

            for (int i = 0;  i< pageNumList.Length; ++i)
            {
                if (pageNumList[i] == pageNumber)
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

        private static bool validateData(string srcFileName, string pageList, out int[] pageNumList)
        {
            if (srcFileName == null)
            {
                throw new Exception("Source file name is empty." + Environment.NewLine +
                    "Error CNF-395 in " + PROJ_FILE_NAME + ".validateData().");
            }
            if (!File.Exists(srcFileName))
            {
                throw new FileNotFoundException(srcFileName + " not found" + Environment.NewLine +
                    "Error CNF-396 in " + PROJ_FILE_NAME + ".validateData().");
            }
            if ( pageList == null || "".Equals(pageList))
            {
                throw new Exception("Page numbers list is empty" + Environment.NewLine +
                    "Error CNF-397 in " + PROJ_FILE_NAME + ".validateData().");
            }
            char[] deli = { ',' };
            string[] tokens = pageList.Split(deli);
            int length = tokens.Length;
            if (length <= 0)
            {
                throw new Exception("Page numbers list is empty" + Environment.NewLine +
                    "Error CNF-398 in " + PROJ_FILE_NAME + ".validateData().");
            }
            pageNumList = new int[length];
            for (int i = 0; i < length; ++i)
            {
                pageNumList[i] = Convert.ToInt32(tokens[i]);
            }
            Array.Sort(pageNumList);
            return true;
        }

        public static void MergeFiles(string mainFile, string appendFile, bool annotation)
        {


            if (MergeFilesByCopy(mainFile, appendFile))
            {
                return;
            }
            

            RasterCodecs codec = new RasterCodecs();
            RasterImage image = null;
            RasterTagMetadata[] tagsData = null;  // stores the annotatin
            RasterImage image2 = null;
            try
            {
                // load the image and annotation information for the 
                // append file.
                image = codec.Load(appendFile);
                int pageCount = image.PageCount;
                if (annotation)
                {
                    tagsData = new RasterTagMetadata[pageCount];
                    for (int i = 0; i < pageCount; ++i)
                    {
                        tagsData[i] = codec.ReadTag(appendFile, i + 1, RasterTagMetadata.AnnotationTiff);
                    }
                }
                StatusForm statusForm = new StatusForm();
                statusForm.LoadFormData(appendFile, mainFile, pageCount);
                statusForm.Show();
                statusForm.Refresh();
                image2 = codec.Load(mainFile);
                int mainPageNumber = image2.PageCount;
                for (int i = 0; i < pageCount; ++i)
                {
                    codec.Save(image, mainFile, RasterImageFormat.CcittGroup4, 1,  i + 1, i + 1, 1, CodecsSavePageMode.Append);
                    if (annotation)
                    {
                        if ((tagsData != null) && (tagsData[i] != null))
                        {
                            codec.WriteTag(mainFile, mainPageNumber + i + 1, tagsData[i]);
                        }
                    }
                    statusForm.ShowPageInfo(i);
                }
                statusForm.Close();

            }
            finally
            {
                if (image != null)
                {
                    image.Dispose();
                }
                if (image2 != null)
                {
                    image2.Dispose();
                }
            }


        }

        private static bool MergeFilesByCopy(string mainFile, string appendFile)
        {
            if (!File.Exists(mainFile))
            {
                if (File.Exists(appendFile))
                {
                    File.Copy(appendFile, mainFile);
                }
                return true;
            }
            if (!File.Exists(appendFile))
            {
                return true;
            }
            return false;
        }

        public static string GetUserNameWithoutDomain(string pNameWithDomain)
        {
            string userName = "";

            int delimPos = pNameWithDomain.IndexOf(@"\");
            //string testDomain = pNameWithDomain.Substring(0, delimPos);
            userName = pNameWithDomain.Substring(delimPos + 1, pNameWithDomain.Length - (delimPos + 1));

            return userName;
        }
    }
}
