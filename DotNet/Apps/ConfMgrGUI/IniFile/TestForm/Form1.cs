using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Collections.Specialized;

namespace TestForm
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.TextBox textBox3;

		Sempra.IniFile iniFile;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.NumericUpDown numericUpDown2;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.Button button8;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
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
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.button2 = new System.Windows.Forms.Button();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.button3 = new System.Windows.Forms.Button();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.button4 = new System.Windows.Forms.Button();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
			this.button5 = new System.Windows.Forms.Button();
			this.button6 = new System.Windows.Forms.Button();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.button7 = new System.Windows.Forms.Button();
			this.button8 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
			this.SuspendLayout();
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(8, 24);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(160, 20);
			this.textBox1.TabIndex = 0;
			this.textBox1.Text = "textBox1";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(176, 24);
			this.button1.Name = "button1";
			this.button1.TabIndex = 1;
			this.button1.Text = "Write String";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(8, 56);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(160, 20);
			this.textBox2.TabIndex = 2;
			this.textBox2.Text = "textBox2";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(176, 56);
			this.button2.Name = "button2";
			this.button2.TabIndex = 3;
			this.button2.Text = "Read String";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// textBox3
			// 
			this.textBox3.Location = new System.Drawing.Point(8, 296);
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new System.Drawing.Size(280, 20);
			this.textBox3.TabIndex = 4;
			this.textBox3.Text = "H:\\VisualStudioProjects\\SempraIniFile\\TestIni.ini";
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(176, 152);
			this.button3.Name = "button3";
			this.button3.TabIndex = 5;
			this.button3.Text = "Write Bool";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// checkBox1
			// 
			this.checkBox1.Location = new System.Drawing.Point(8, 152);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.TabIndex = 6;
			this.checkBox1.Text = "checkBox1";
			// 
			// checkBox2
			// 
			this.checkBox2.Location = new System.Drawing.Point(8, 184);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.TabIndex = 7;
			this.checkBox2.Text = "checkBox2";
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(176, 184);
			this.button4.Name = "button4";
			this.button4.TabIndex = 8;
			this.button4.Text = "Read Bool";
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Location = new System.Drawing.Point(8, 88);
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.TabIndex = 9;
			// 
			// numericUpDown2
			// 
			this.numericUpDown2.Location = new System.Drawing.Point(8, 120);
			this.numericUpDown2.Name = "numericUpDown2";
			this.numericUpDown2.TabIndex = 10;
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(176, 88);
			this.button5.Name = "button5";
			this.button5.TabIndex = 12;
			this.button5.Text = "Write Int";
			this.button5.Click += new System.EventHandler(this.button5_Click);
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(176, 120);
			this.button6.Name = "button6";
			this.button6.TabIndex = 11;
			this.button6.Text = "Read Int";
			this.button6.Click += new System.EventHandler(this.button6_Click);
			// 
			// listBox1
			// 
			this.listBox1.Location = new System.Drawing.Point(8, 216);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(160, 56);
			this.listBox1.TabIndex = 13;
			// 
			// button7
			// 
			this.button7.Location = new System.Drawing.Point(176, 216);
			this.button7.Name = "button7";
			this.button7.TabIndex = 14;
			this.button7.Text = "Write List";
			this.button7.Click += new System.EventHandler(this.button7_Click);
			// 
			// button8
			// 
			this.button8.Location = new System.Drawing.Point(176, 248);
			this.button8.Name = "button8";
			this.button8.TabIndex = 15;
			this.button8.Text = "Read List";
			this.button8.Click += new System.EventHandler(this.button8_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 325);
			this.Controls.Add(this.button8);
			this.Controls.Add(this.button7);
			this.Controls.Add(this.listBox1);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.button6);
			this.Controls.Add(this.numericUpDown2);
			this.Controls.Add(this.numericUpDown1);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.checkBox2);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.textBox3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.textBox1);
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			iniFile = new Sempra.IniFile(textBox3.Text);
			iniFile.WriteValue( "Text", "test", textBox1.Text );
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			iniFile = new Sempra.IniFile(textBox3.Text);
			textBox2.Text = iniFile.ReadValue( "Text", "test", "default" );
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			iniFile = new Sempra.IniFile(textBox3.Text);
			iniFile.WriteValue( "Bool", "test", checkBox1.Checked );
		}

		private void button4_Click(object sender, System.EventArgs e)
		{
			iniFile = new Sempra.IniFile(textBox3.Text);
			checkBox2.Checked = iniFile.ReadValue( "Bool", "test", true );
		}

		private void button5_Click(object sender, System.EventArgs e)
		{
			iniFile = new Sempra.IniFile(textBox3.Text);
			iniFile.WriteValue( "Int", "test", (int)numericUpDown1.Value );		
		}

		private void button6_Click(object sender, System.EventArgs e)
		{
			iniFile = new Sempra.IniFile(textBox3.Text);
			numericUpDown2.Value = iniFile.ReadValue( "Int", "test", 42 );		
		}

		private void button7_Click(object sender, System.EventArgs e)
		{
			iniFile = new Sempra.IniFile(textBox3.Text);
			StringCollection list = new StringCollection();

			foreach( string s in listBox1.Items )
				list.Add( s );

			iniFile.WriteValue( "List", list );		
		}

		private void button8_Click(object sender, System.EventArgs e)
		{
			iniFile = new Sempra.IniFile(textBox3.Text);
			StringCollection list = iniFile.ReadValue( "List" );

			listBox1.Items.Clear();
			for( int i = 0; i < list.Count; i++ )
				listBox1.Items.Add( list[i] );
		}
	}
}
