using System;
using System.Collections.Generic;
using System.Text;
using Leadtools.Annotations;
using Leadtools.Codecs;
using Leadtools.WinForms;
using System.Windows.Forms;
using Leadtools;
using System.Drawing;
using System.IO;

namespace Sempra.Confirm.InBound.ImageEdit
{
    public class EditEventHandler
    {
        private TifEditor editor;

        public TifEditor Editor
        {
            get { return editor; }
            set { editor = value; }
        }

        public void ItemsAdded(object sender, AnnPaintEventArgs e)
        {
            this.editor.Viewer.Invalidate();
        }

        public void Objects_ItemAddedRemoved(object sender, RasterCollectionEventArgs<AnnObject> e)
        {
            HandleObject(e.Item);
         }

        private void HandleObject(AnnObject annObject)
        {
            Type t = annObject.GetType();
            
            if ("AnnStampObject".Equals(t.Name, StringComparison.CurrentCultureIgnoreCase))
             {
                 string fileName = null;
                 
                if ( TifEditor._CHECK_MARK_NAME.Equals(annObject.Name,StringComparison.OrdinalIgnoreCase ) )
                {
                    
                    AnnStampObject obj = (AnnStampObject)annObject; 
                    if (obj.Picture.Image == null)
                    {
                        obj.Picture = new AnnPicture(Image.FromHbitmap(ImageEdit.CheckmarkOnly.GetHbitmap()));
                        editor.Viewer.Invalidate(obj.InvalidRectangle);
                    }

                    // keep selecting
                    this.Editor.AnnAutomation.Manager.CurrentObjectId = AnnAutomationManager.UserObjectId;
                  
                }
                else 
                {
                    fileName = GetSignatureFileName();
                    AnnStampObject obj = (AnnStampObject)annObject;
                    obj.Text = "";
                    if (obj.Picture.Image == null)
                    {
                        if (fileName != null && File.Exists(fileName))
                        {
                            obj.Picture = new AnnPicture(Image.FromFile(fileName));
                            editor.Viewer.Invalidate(obj.InvalidRectangle);
                        }
                        else
                        {
                            obj.Text = "Signature is missing";
                        }
                    }
                
               }

            }          
        }

        private string GetSignatureFileName()
        {
            //Israel 11/06/2015 -- Make sure there is no Domain name when retrieving signature.
            //string userName = editor.UserName;
            string userName = TifUtil.GetUserNameWithoutDomain(editor.UserName);
            string fileName = null;
            if (userName != null)
            {
                //Israel 11/06/2015 -- Added configurable folder for signatures.
                //fileName = AppDomain.CurrentDomain.BaseDirectory + "\\Sig\\" + userName + ".bmp";                
                fileName = TifEditor.SignatureLocation + @"\" + userName + ".bmp";
            }
            return fileName;
        }

        public void automation_CurrentDesignerChanged(object sender, EventArgs e)
        {
            
            CurrentDesignerChanged();
            
        }

        public void CurrentDesignerChanged()
        {
        
          AnnAutomation automation = editor.AnnAutomation;


          if (automation.CurrentDesigner != null && automation.CurrentDesigner is AnnRunDesigner)
          {
             AnnRunDesigner runDesigner = automation.CurrentDesigner as AnnRunDesigner;
             runDesigner.Run += new EventHandler<AnnRunDesignerEventArgs>(MyRunDesignerHandler);
          }
            
        }

        private void MyRunDesignerHandler(object sender, AnnRunDesignerEventArgs e)
        {

            if (e.OperationStatus == AnnDesignerOperationStatus.End)
            {
                if (!e.Cancel && (e.Object.Hyperlink == null || e.Object.Hyperlink == string.Empty))
                {
                    e.Cancel = true;

                    StringBuilder sb = new StringBuilder();
                    sb.Append("You clicked an object that has no hyperlink:");
                    sb.Append(Environment.NewLine);
                    sb.Append(Environment.NewLine);
                    sb.Append(string.Format("Name: {0}", e.Object.Name));
                    sb.Append(Environment.NewLine);
                    sb.Append(string.Format("Type: {0}", e.Object.GetType().Name));
                    sb.Append(Environment.NewLine);
                    MessageBox.Show(sb.ToString());
                }
            }
        }

        public void automation_AfterObjectChanged(object sender, AnnAfterObjectChangedEventArgs e)
        {
            
            //if (e.ChangeType == AnnObjectChangedType.
            AnnEditDesigner annDes;
            annDes = editor.AnnAutomation.CurrentDesigner as AnnEditDesigner;
            //if (typeof(editor.AnnAutomation.CurrentDesigner)) is AnnEditDesigner) 
            if (annDes != null)
            {
                AnnObject ann = annDes.EditObject;
               // if (e.Objects.Count > 0)
                {
                   //ShowPropertyDialog(e.Objects[0]);
                }
                ShowPropertyDialog(ann);
            }
        }
        private void ShowPropertyDialog(AnnObject annObject)
        {
                if (IsEligible(annObject))
                {
                    AnnTextObject textObj = annObject as AnnTextObject;
                    if (textObj != null)
                    {
                        PropertyForm.userData = textObj.Text;
                        editor.AnnAutomation.ShowObjectPropertiesDialog();
                        textObj.Text = PropertyForm.userData;
                        editor.AnnAutomation.Container.Objects.Add(textObj);
                        editor.AnnAutomation.Viewer.Invalidate(textObj.InvalidRectangle);                        
                    }
                    
                }
        }

        private bool IsEligible(AnnObject annObject)
        {
            string[] objectList = new string[] { "AnnTextObject", "AnnNoteObject" };
            Type type = annObject.GetType();
            if (Array.IndexOf(objectList,type.Name) >= 0){
                return true;
            }
            return false;
        }

    }
}
