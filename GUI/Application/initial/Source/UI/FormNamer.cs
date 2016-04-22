using System;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using NSRMCommon;
//>>>>>>> Test

namespace NSRiskManager {
    public partial class FormNamer : XtraForm {
        #region fields
        public event ValidNameHandler validateName;
        #endregion

      
        public string oldWindowName = "";
        private bool closeErrorExists = false;
        public int windowId = 0;

        #region ctor
        public FormNamer() {

            closeErrorExists = false;
            InitializeComponent();
            this.teNewName.DataBindings.Add("Text",this,"formName");
        }


        public FormNamer(string defaultName) 
        {
            closeErrorExists = false;
            oldWindowName = defaultName;

            InitializeComponent();
            this.teNewName.DataBindings.Add("Text", this, "formName");

            try
            {
                this.teNewName.Text = defaultName;
            }
            catch (Exception e)
            { 
            }

           
        }

        #endregion

        #region properties
        public string formName { get; set; }
       

        #endregion

        #region constants
        const string KEY = "FormNameFrame";
        #endregion

        #region action-methods
        void FormNamer_Load(object sender,EventArgs e) {

            
            this.teNewName.SelectAll();
        }

        protected override void OnClosing(CancelEventArgs e) 
        {

            if (this.DialogResult != DialogResult.Cancel)
            {
                if (closeErrorExists)
                    e.Cancel = true;

            }

            base.OnClosing(e);
          
        }

        void CancelClickedHandler(object sender, EventArgs e)
        {
            closeErrorExists = false;
        }

        void OkClickedHandler(object sender,EventArgs e) 
        {
            CancelEventArgs cancelArgs = new CancelEventArgs();

            closeErrorExists = false;

            formNameText_Validating(sender, cancelArgs);

            if (cancelArgs.Cancel)
                closeErrorExists = true;

        }
        
        
         void formNameText_Validating(object sender,CancelEventArgs e) 
         {
            

            errorProvider1.SetError(this.teNewName,string.Empty);
            
            if (string.IsNullOrEmpty(this.teNewName.Text))
            {
                errorProvider1.SetError(this.teNewName,"You must provide a name for the window!");
                e.Cancel = true;
            }
            else if (validateName != null)
            {
                if (!validateName(this, oldWindowName, this.teNewName.Text, windowId)) 
                {
                    errorProvider1.SetError(this.teNewName, "A window called '" + this.teNewName.Text + "' already exist or is invalid/reserved.  Choose another, please.");
                    e.Cancel = true;
                }
            }
         }

        void FormNamer_Activated(object sender,EventArgs e) {
            this.teNewName.SelectAll();
        }

        #endregion

        
    }

    public delegate bool ValidNameHandler(object sender, string oldName, string newName, int windowID);
}