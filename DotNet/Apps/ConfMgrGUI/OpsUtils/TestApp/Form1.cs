using System;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Sempra;

namespace TestApp
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.DateTimePicker dateTimePicker1;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Button button7;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1(string[] args)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			/*foreach( string line in args )
				MessageBox.Show( "\'" + line + "\'" );

			MessageBox.Show( "Parameter d: \'" + Ops.GetParamValue( "d", args ) + "\'" );
			*///
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.button2 = new System.Windows.Forms.Button();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.button3 = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.button4 = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.button5 = new System.Windows.Forms.Button();
			this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.button6 = new System.Windows.Forms.Button();
			this.button7 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(48, 40);
			this.textBox1.Name = "textBox1";
			this.textBox1.TabIndex = 0;
			this.textBox1.Text = "-u Parameter -p password -d database";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(168, 40);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(120, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "Test Param Reader";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(48, 64);
			this.label1.Name = "label1";
			this.label1.TabIndex = 2;
			this.label1.Text = "label1";
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(48, 16);
			this.textBox2.MaxLength = 1;
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(24, 20);
			this.textBox2.TabIndex = 3;
			this.textBox2.Text = "";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(8, 104);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(144, 23);
			this.button2.TabIndex = 4;
			this.button2.Text = "Test Version Info Reader";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// listBox1
			// 
			this.listBox1.Location = new System.Drawing.Point(8, 136);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(248, 147);
			this.listBox1.TabIndex = 5;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(64, 304);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(136, 23);
			this.button3.TabIndex = 6;
			this.button3.Text = "Test Machine Name";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 344);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(240, 16);
			this.label2.TabIndex = 7;
			this.label2.Text = "label2";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 408);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(240, 16);
			this.label3.TabIndex = 9;
			this.label3.Text = "label3";
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(64, 368);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(136, 23);
			this.button4.TabIndex = 8;
			this.button4.Text = "Test User ID";
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 472);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(272, 23);
			this.label4.TabIndex = 10;
			this.label4.Text = "label4";
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(64, 440);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(136, 23);
			this.button5.TabIndex = 11;
			this.button5.Text = "Test Ini File";
			this.button5.Click += new System.EventHandler(this.button5_Click);
			// 
			// dateTimePicker1
			// 
			this.dateTimePicker1.Location = new System.Drawing.Point(8, 504);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.TabIndex = 12;
			// 
			// textBox3
			// 
			this.textBox3.Location = new System.Drawing.Point(8, 528);
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new System.Drawing.Size(200, 20);
			this.textBox3.TabIndex = 13;
			this.textBox3.Text = "textBox3";
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(208, 504);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(75, 40);
			this.button6.TabIndex = 14;
			this.button6.Text = "Test Oracle Date Time";
			this.button6.Click += new System.EventHandler(this.button6_Click);
			// 
			// button7
			// 
			this.button7.Location = new System.Drawing.Point(168, 104);
			this.button7.Name = "button7";
			this.button7.TabIndex = 15;
			this.button7.Text = "String Parse";
			this.button7.Click += new System.EventHandler(this.button7_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(288, 605);
			this.Controls.Add(this.button7);
			this.Controls.Add(this.button6);
			this.Controls.Add(this.textBox3);
			this.Controls.Add(this.dateTimePicker1);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.listBox1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.button1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) 
		{
			Application.Run(new Form1(args));
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			string[] paramArray = new string[1] {textBox1.Text};
			label1.Text = "\'" + Ops.GetParamValue( textBox2.Text, paramArray ) + "\'";
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			try
			{
				VersionInfo versionInfo = Ops.GetVersionInfo( Application.ExecutablePath );
				listBox1.Items.Add( "CompanyName: "		+ versionInfo.CompanyName );
				listBox1.Items.Add( "FileDescription: "	+ versionInfo.FileDescription );
				listBox1.Items.Add( "FileVersion: "		+ versionInfo.FileVersion );
				listBox1.Items.Add( "InternalName: "	+ versionInfo.InternalName );
				listBox1.Items.Add( "LegalCopyright: "	+ versionInfo.LegalCopyright );
				listBox1.Items.Add( "LegalTradeMarks: "	+ versionInfo.LegalTradeMarks );
				listBox1.Items.Add( "OriginalFileName: "+ versionInfo.OriginalFileName );
				listBox1.Items.Add( "ProductName: "		+ versionInfo.ProductName );
				listBox1.Items.Add( "ProductVersion: "	+ versionInfo.ProductVersion );
				listBox1.Items.Add( "Comments: "		+ versionInfo.Comments );
			}
			catch( Exception error )
			{
				MessageBox.Show( error.Message );
			}
		
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			label2.Text = "'" + Ops.GetComputerName() + "'";
		}

		private void button4_Click(object sender, System.EventArgs e)
		{
			label3.Text = "'" + Ops.GetUserName() + "'";
		}

		private void button5_Click(object sender, System.EventArgs e)
		{
			label4.Text = "'" + Ops.GetUserIniFileName( "C:\\temp" ) + "'";
		}

		private void button6_Click(object sender, System.EventArgs e)
		{
			textBox3.Text = Ops.OracleDate( dateTimePicker1.Value );
		}

		private void button7_Click(object sender, System.EventArgs e)
		{
			StringCollection list = Sempra.Ops.ListFromString( "item 1, item 2, item 3" );

			listBox1.Items.Clear();
			foreach( string s in list )
				listBox1.Items.Add( s );		
		}
	}
}
