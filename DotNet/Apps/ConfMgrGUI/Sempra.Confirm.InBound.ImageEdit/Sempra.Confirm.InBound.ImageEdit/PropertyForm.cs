using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Leadtools.WinForms;
using Leadtools.Annotations;

namespace Sempra.Confirm.InBound.ImageEdit
{
    public partial class PropertyForm : Form
    {
        AnnObject annControl;
        public static string userData;
        /*
        public string UserData
        {
            get { return txtData.Text ; }
            set { 
                txtData.Text = value;
                userData = value;
            }
        }
         * */

        public AnnObject AnnControl
        {
            get { return annControl; }
            set { annControl = value; }
        }

        public PropertyForm()
        {
            InitializeComponent();
            
        }

        private void PropertyForm_Load(object sender, EventArgs e)
        {
            
            AnnObject annObject = TifEditor.GetCurrentEditor().AnnAutomation.CurrentEditObject;
            AnnTextObject obj = annObject as AnnTextObject;
            if (obj != null &&  obj.GetType() != typeof(AnnStampObject))
            {
                this.Opacity = 100;
                InitializeValue();

            }
            else
            {
                
               /* if (obj != null && obj.GetType() == typeof(AnnStampObject))
                {
                * */
                if (obj == null)
                {

                    MessageBox.Show("The property form is not supported for selected object in this version.");
                }
                /*
                }*/
                this.Close();
                 
                
            }
            

        }

        private void InitializeValue()
        {
            AnnObject annObject = TifEditor.GetCurrentEditor().AnnAutomation.CurrentEditObject;
            AnnTextObject obj = annObject as AnnTextObject;
            if (obj != null)
            {
                txtData.Text = obj.Text;
                
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {

            userData = txtData.Text;
            SetObjectValue();
            
        }

        private void SetObjectValue()
        {
            AnnObject annObject = TifEditor.GetCurrentEditor().AnnAutomation.CurrentEditObject;
            AnnTextObject obj = annObject as AnnTextObject;
            if (obj != null)
            {
                obj.Text = txtData.Text;
            }
        }

        private void PropertyForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                TifEditor.GetCurrentEditor().Viewer.Invalidate();
            }
            catch (Exception ex)
            {
            }
        }

       
    }
}