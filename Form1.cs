using System;
using System.Windows.Forms;
using System.Text;
using System.Collections;
using System.Diagnostics;
using Register;
using Microsoft.Win32;
using System.IO;

namespace RegList
{
	public class Form1 : System.Windows.Forms.Form
	{
		int		count;
		int		n1,n2,n3;	// for test only
		int		level;
		long	time;
		long	ModifyDate;
		const	string timefmt = "dd-MM-yyyy HH:mm:ss";
		const	string OFFLINE_SUBKEY = "RegList";
		string	offlineFile;
		string	searchText;
		string	excludeText;
		bool	blnCheckModifyDate;
		bool	blnCheckKey;
		bool	blnCheckName;
		bool	blnCheckValue;
		bool	blnExclude;
		bool	blnDeferredAddKey;
		bool	previousCheckState;
		bool	InProcess;
		#region local variables
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.CheckBox checkBoxHKLM;
		private System.Windows.Forms.CheckBox checkBoxHKCU;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.StatusBarPanel statusBarPanel1;
		private System.Windows.Forms.StatusBarPanel statusBarPanel2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox checkBoxKey;
		private System.Windows.Forms.CheckBox checkBoxName;
		private System.Windows.Forms.CheckBox checkBoxValue;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox textBoxExclude;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.DateTimePicker dateTimePicker1;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem mnuDelete;
		private System.Windows.Forms.MenuItem mnuEdit;
		private System.Windows.Forms.MenuItem mnuReport;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.CheckBox checkBoxFile;
		#endregion local varables
		public Form1()
		{
			InitializeComponent();
			General.StatusBar = this.statusBar1; // From now General.Log can be called.
			this.Text += General.GetVersion();
		}
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if ( checkBoxFile.Checked ) CloseRegistryFile();
					
				if(components != null)
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.mnuEdit = new System.Windows.Forms.MenuItem();
            this.mnuDelete = new System.Windows.Forms.MenuItem();
            this.mnuReport = new System.Windows.Forms.MenuItem();
            this.checkBoxHKLM = new System.Windows.Forms.CheckBox();
            this.checkBoxHKCU = new System.Windows.Forms.CheckBox();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
            this.statusBarPanel2 = new System.Windows.Forms.StatusBarPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxKey = new System.Windows.Forms.CheckBox();
            this.checkBoxName = new System.Windows.Forms.CheckBox();
            this.checkBoxValue = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.textBoxExclude = new System.Windows.Forms.TextBox();
            this.checkBoxFile = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel2)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(64, 17);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(64, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "search  text";
            // 
            // btnGo
            // 
            this.btnGo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnGo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnGo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGo.Location = new System.Drawing.Point(7, 16);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(50, 23);
            this.btnGo.TabIndex = 1;
            this.btnGo.Text = "Search";
            this.btnGo.UseVisualStyleBackColor = false;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listView1.ContextMenu = this.contextMenu1;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 56);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(568, 248);
            this.listView1.TabIndex = 2;
            this.toolTip1.SetToolTip(this.listView1, "Double click to start Regedit");
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Last modified";
            this.columnHeader1.Width = 109;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Key";
            this.columnHeader2.Width = 298;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Name";
            this.columnHeader3.Width = 83;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Data";
            this.columnHeader4.Width = 253;
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuEdit,
            this.mnuDelete,
            this.mnuReport});
            // 
            // mnuEdit
            // 
            this.mnuEdit.Index = 0;
            this.mnuEdit.Text = "View Key";
            this.mnuEdit.Click += new System.EventHandler(this.mnuEdit_Click);
            // 
            // mnuDelete
            // 
            this.mnuDelete.Index = 1;
            this.mnuDelete.Text = "Delete Key...";
            this.mnuDelete.Click += new System.EventHandler(this.mnuDelete_Click);
            // 
            // mnuReport
            // 
            this.mnuReport.Index = 2;
            this.mnuReport.Text = "Report...";
            this.mnuReport.Click += new System.EventHandler(this.mnuReport_Click);
            // 
            // checkBoxHKLM
            // 
            this.checkBoxHKLM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.checkBoxHKLM.Checked = true;
            this.checkBoxHKLM.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxHKLM.Location = new System.Drawing.Point(154, 6);
            this.checkBoxHKLM.Name = "checkBoxHKLM";
            this.checkBoxHKLM.Size = new System.Drawing.Size(56, 14);
            this.checkBoxHKLM.TabIndex = 3;
            this.checkBoxHKLM.Text = "HKLM";
            this.checkBoxHKLM.UseVisualStyleBackColor = false;
            // 
            // checkBoxHKCU
            // 
            this.checkBoxHKCU.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.checkBoxHKCU.Checked = true;
            this.checkBoxHKCU.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxHKCU.Location = new System.Drawing.Point(154, 20);
            this.checkBoxHKCU.Name = "checkBoxHKCU";
            this.checkBoxHKCU.Size = new System.Drawing.Size(56, 14);
            this.checkBoxHKCU.TabIndex = 4;
            this.checkBoxHKCU.Text = "HKCU";
            this.checkBoxHKCU.UseVisualStyleBackColor = false;
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 304);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statusBarPanel1,
            this.statusBarPanel2});
            this.statusBar1.ShowPanels = true;
            this.statusBar1.Size = new System.Drawing.Size(568, 22);
            this.statusBar1.TabIndex = 5;
            this.statusBar1.Text = "statusBar1";
            // 
            // statusBarPanel1
            // 
            this.statusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.statusBarPanel1.Name = "statusBarPanel1";
            this.statusBarPanel1.Width = 491;
            // 
            // statusBarPanel2
            // 
            this.statusBarPanel2.Name = "statusBarPanel2";
            this.statusBarPanel2.Width = 60;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.label1.Location = new System.Drawing.Point(134, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "in";
            // 
            // checkBoxKey
            // 
            this.checkBoxKey.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.checkBoxKey.Checked = true;
            this.checkBoxKey.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxKey.Location = new System.Drawing.Point(214, 6);
            this.checkBoxKey.Name = "checkBoxKey";
            this.checkBoxKey.Size = new System.Drawing.Size(48, 14);
            this.checkBoxKey.TabIndex = 9;
            this.checkBoxKey.Text = "Key";
            this.checkBoxKey.UseVisualStyleBackColor = false;
            // 
            // checkBoxName
            // 
            this.checkBoxName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.checkBoxName.Checked = true;
            this.checkBoxName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxName.Location = new System.Drawing.Point(214, 20);
            this.checkBoxName.Name = "checkBoxName";
            this.checkBoxName.Size = new System.Drawing.Size(56, 14);
            this.checkBoxName.TabIndex = 10;
            this.checkBoxName.Text = "Name";
            this.checkBoxName.UseVisualStyleBackColor = false;
            // 
            // checkBoxValue
            // 
            this.checkBoxValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.checkBoxValue.Checked = true;
            this.checkBoxValue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxValue.Location = new System.Drawing.Point(214, 34);
            this.checkBoxValue.Name = "checkBoxValue";
            this.checkBoxValue.Size = new System.Drawing.Size(56, 14);
            this.checkBoxValue.TabIndex = 11;
            this.checkBoxValue.Text = "Data";
            this.checkBoxValue.UseVisualStyleBackColor = false;
            // 
            // textBoxExclude
            // 
            this.textBoxExclude.Location = new System.Drawing.Point(9, 16);
            this.textBoxExclude.Name = "textBoxExclude";
            this.textBoxExclude.Size = new System.Drawing.Size(63, 20);
            this.textBoxExclude.TabIndex = 13;
            this.textBoxExclude.Text = "t00";
            this.toolTip1.SetToolTip(this.textBoxExclude, "Exclude keys, names or data containing this text");
            // 
            // checkBoxFile
            // 
            this.checkBoxFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.checkBoxFile.Location = new System.Drawing.Point(154, 34);
            this.checkBoxFile.Name = "checkBoxFile";
            this.checkBoxFile.Size = new System.Drawing.Size(56, 14);
            this.checkBoxFile.TabIndex = 17;
            this.checkBoxFile.Text = "File";
            this.toolTip1.SetToolTip(this.checkBoxFile, "Open an offline Registry File. For example  C:\\documents and Settings\\user\\ntuser" +
                    ".dat");
            this.checkBoxFile.UseVisualStyleBackColor = false;
            this.checkBoxFile.CheckedChanged += new System.EventHandler(this.checkBoxFile_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.groupBox1.Controls.Add(this.textBoxExclude);
            this.groupBox1.Location = new System.Drawing.Point(304, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(83, 46);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Exclude text";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.groupBox2.Controls.Add(this.dateTimePicker1);
            this.groupBox2.Location = new System.Drawing.Point(408, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(152, 46);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Show keys modified since";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CalendarMonthBackground = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.dateTimePicker1.CalendarTitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.dateTimePicker1.Checked = false;
            this.dateTimePicker1.CustomFormat = "dd MMM yyyy  HH:mm";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(8, 16);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.ShowCheckBox = true;
            this.dateTimePicker1.Size = new System.Drawing.Size(136, 20);
            this.dateTimePicker1.TabIndex = 9;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "txt";
            this.saveFileDialog1.Filter = "txt|*.txt|Doc|*.Doc|All files|*.*";
            this.saveFileDialog1.Title = "Save Report as";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "dat";
            this.openFileDialog1.FileName = "ntuser.dat";
            this.openFileDialog1.Filter = "*.dat|*.dat|*.sav|*.sav|All files|*.*";
            this.openFileDialog1.InitialDirectory = "Documents and Settings";
            this.openFileDialog1.ReadOnlyChecked = true;
            this.openFileDialog1.Title = "Open an offline Registry File. For example C:\\Documents and Settings\\user\\ntuser." +
                "dat";
            // 
            // Form1
            // 
            this.AcceptButton = this.btnGo;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(568, 326);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkBoxFile);
            this.Controls.Add(this.checkBoxValue);
            this.Controls.Add(this.checkBoxName);
            this.Controls.Add(this.checkBoxKey);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.checkBoxHKCU);
            this.Controls.Add(this.checkBoxHKLM);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.btnGo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Rudy\'s Forensic Registry List";
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void btnGo_Click(object sender, System.EventArgs e)
		{
			btnGo.Enabled = false;
			level = count = 0;
			n1=n2=n3=0;
			searchText			= textBox1.Text.ToLower();
			excludeText			= textBoxExclude.Text.ToLower();
			blnCheckModifyDate	= dateTimePicker1.Checked;
			blnCheckKey			= checkBoxKey.Checked;
			blnCheckName		= checkBoxName.Checked;
			blnCheckValue		= checkBoxValue.Checked;
			blnExclude			= textBoxExclude.Text.Length > 0;

			ModifyDate = (blnCheckModifyDate)?dateTimePicker1.Value.AddSeconds(-dateTimePicker1.Value.Second).ToFileTime():0;
			listView1.ListViewItemSorter = null;
			listView1.Items.Clear();
			int start = Environment.TickCount;
			if ( checkBoxHKCU.Checked )	SearchAndDisplayKeys( Reg.HKEY_CURRENT_USER, "HKCU");
			Application.DoEvents();
			if ( checkBoxHKLM.Checked )	SearchAndDisplayKeys( Reg.HKEY_LOCAL_MACHINE, "HKLM");
			if ( checkBoxFile.Checked )	
			{
				int subrk;
				int retCode = Reg.RegOpenKeyEx(Reg.HKEY_USERS,OFFLINE_SUBKEY,0, Reg.KEY_READ, out subrk);
				if (retCode == Reg.SUCCESS )
				{
					SearchAndDisplayKeys( subrk, offlineFile);
					Reg.RegCloseKey(subrk);
				}
				else General.LogError("{0} {1} {2}",offlineFile,retCode,Reg.GetMessage(retCode));
			}
			int eind = Environment.TickCount;
			General.Log("Keys processed: {0}   Found: {1}   Elapsed time: {2} ms", count, listView1.Items.Count, eind-start,n1,n2);
			btnGo.Enabled = true;
		}
		private void addLine(string key, string name, string values)
		{
			if ( !blnExclude || blnExclude && (key + name + values).ToString().ToLower().IndexOf(excludeText) == -1 )
			{
				listView1.Items.Add( new ListViewItem(new string[]{DateTime.FromFileTime(time).ToString(timefmt), key ,name.ToString(),values.ToString()}));
			}
			blnDeferredAddKey = false;
		}

		private void SearchAndDisplayKeys( int rk, string key)
		{
			int retCode;
			int nrofSubkeys;
			int nrofValues;
			if ((++count & 0x1ff) == 0) statusBarPanel1.Text = count.ToString();
			//
			// Search names and values
			//
			Reg.RegQueryInfoKey(rk,null,null,IntPtr.Zero,out nrofSubkeys,null,null,out nrofValues,null,null,null,null);
			StringBuilder name = new StringBuilder(0x100);
			StringBuilder values = new StringBuilder(0x100);
			if (nrofSubkeys > 1000)
				Console.WriteLine("{0} {1}",key,nrofSubkeys);
			if ( time > ModifyDate ) // V103
				for ( int i=0;i<nrofValues;i++ )
				{
					int maxname = 0x100;
					int maxval  = 0x100;
					int type;
				loop:			retCode = Reg.RegEnumValue(rk, i, name, ref maxname, IntPtr.Zero, out type, values, ref maxval);
					//				if (key.EndsWith("Services\\Netlogon"))
					//				{
					//					break;
					//				}
					if (retCode == Reg.SUCCESS )
					{
//V108					if ( blnCheckValue && type == Reg.REG_SZ && values.ToString().ToLower().IndexOf(searchText) > -1 ) 
//V109					if ( blnCheckValue && type <= Reg.REG_EXPAND_SZ && values.ToString().ToLower().IndexOf(searchText) > -1 ) 
//V114					if ( blnCheckValue && type <= Reg.REG_BINARY && values.ToString().ToLower().IndexOf(searchText) > -1 ) 
						if ( blnCheckValue && ( type <= Reg.REG_BINARY || type == Reg.REG_MULTI_SZ )&& values.ToString().ToLower().IndexOf( searchText ) > -1 ) 
							addLine(key,name.ToString(),values.ToString());
						else if ( blnCheckName && maxname > 0 && name.ToString().ToLower().IndexOf(searchText) > -1 ) 
							addLine(key,name.ToString(),values.ToString());
					}
					else if (retCode == Reg.MORE_DATA && type > Reg.REG_BINARY ) //V109
						break;
					else if (retCode == Reg.MORE_DATA )
					{
						//					if (type == Reg.REG_BINARY) {n1 ++;}
						//					n2++;
						values = new StringBuilder(maxval);
						goto loop;
					}
					else Console.WriteLine("RegEnumValue: {0} {3} {1} {2}",retCode, key, name, maxval);
				}
			if ( blnDeferredAddKey )
			{
				if ( nrofValues == 1) 
					addLine(key,name.ToString(),values.ToString()); 
				else if ( nrofValues == 0) //V111
					addLine(key,"Default",""); 
				else
					addLine(key,"<===","");
			}
			name = null;
			values = null;
			//
			// Search subkeys
			//
			StringBuilder subkey = new StringBuilder(0x100);
			for ( int i=0;i<nrofSubkeys;i++)
			{
				int maxbuf = 0x100;
				retCode = Reg.RegEnumKeyEx(rk, i, subkey, out maxbuf, null, null, null, out time);
				if (retCode == Reg.SUCCESS )
				{
					if ( time > ModifyDate )
						if ( blnCheckKey && subkey.ToString().ToLower().IndexOf(searchText) > -1 )
							blnDeferredAddKey = true;
					//
					// Open the subkey
					//
					int subrk;
					retCode = Reg.RegOpenKeyEx(rk,subkey.ToString(),0, Reg.KEY_READ, out subrk);
					if (retCode == Reg.SUCCESS )
					{
						++level;
						SearchAndDisplayKeys(subrk, key + @"\" + subkey); // Recursion
						--level;
						Reg.RegCloseKey(subrk);
					}
					else if (retCode == Reg.ACCESS_DENIED ){}
					else Console.WriteLine("OpenKeyEx: {0} {1}",retCode,key+subkey);
				}
				else Console.WriteLine("EnumKeyEx: {0} {1}",retCode, key+subkey);
			}
			subkey = null;
		}

		private void listView1_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			listView1.ListViewItemSorter = new SortColumn(e.Column );
		}

		private void startRegEditAndNavigate2Lastkey(string key)
		{
			// HKCU\Software\Microsoft\Windows\CurrentVersion\Applets\Regedit\LastKey	SUCCESS	"My Computer\HKEY_CURRENT_USER\Software\mirkes.de\Tiny Hexer"	
            //key = @"My Computer\" + key.Replace("HKCU","HKEY_CURRENT_USER").Replace("HKLM","HKEY_LOCAL_MACHINE");
            string computerName = General.ReadRegistry(@"SOFTWARE\Classes\CLSID\{20D04FE0-3AEA-1069-A2D8-08002B30309D}")+"\\"; //V115
            key = computerName + key.Replace("HKCU", "HKEY_CURRENT_USER").Replace("HKLM", "HKEY_LOCAL_MACHINE");
			if ( offlineFile != null) key = key.Replace(offlineFile,"HKEY_USERS\\"+OFFLINE_SUBKEY); //V106
			RegistryKey sk = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Applets\Regedit",true);
            if (sk != null) //V116
            {
                sk.SetValue("LastKey", key);
                sk.Close();
                // Kill regedit to intiate navigation.
                killRegedit();
                // Now start registry editor.
            }
            Process.Start("regedit");
		}
		private bool killRegedit()
		{
//			Process[] at = Process.GetProcesses();
//			int i=0;
//			foreach (Process p in at)
//				Console.WriteLine("Process {0}: {1}",i++,p.ProcessName);
			Process[] ap = Process.GetProcessesByName("regedit"); //V113
//			Process[] ap = Process.GetProcessesByName("REGEDIT"); //V112
			if (ap.Length > 0) 
				ap[0].Kill();
			return (ap.Length > 0);
		}

		private void dateTimePicker1_ValueChanged(object sender, System.EventArgs e)
		{
			if ( dateTimePicker1.Checked && !previousCheckState )
			{
				checkBoxName.Checked = false;
				checkBoxValue.Checked = false;
				textBox1.Text = "";
			}
			previousCheckState = dateTimePicker1.Checked;
		}

		private void listView1_DoubleClick(object sender, System.EventArgs e)
		{
			mnuEdit_Click(sender,e);
		}
		private void mnuEdit_Click(object sender, System.EventArgs e)
		{
			string key = listView1.FocusedItem.SubItems[1].Text;
			startRegEditAndNavigate2Lastkey(key);
		}

		private void mnuReport_Click(object sender, System.EventArgs e)
		{
			if (listView1.SelectedItems.Count == 0)
				General.Log("Please select lines to report.");
			else if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				// Compute fieldwidths
				int[] width = new int[listView1.Columns.Count];
				for (int i=0;i<width.Length;i++)
				{
					width[i] = listView1.Columns[i].Width/6+1;
				}
				string reportFile = saveFileDialog1.FileName;
				StreamWriter report = new StreamWriter(reportFile);
				report.WriteLine("Rudy's Registry List Report		created on: {0}\r\n",DateTime.Now);
				// report Column Header
				for (int i=0;i<width.Length;i++)
				{
					report.Write( fixWidth(listView1.Columns[i].Text,width[i],' '));
				}
				report.WriteLine();
				// report =================
				for (int i=0;i<width.Length;i++)
				{
					report.Write( fixWidth("",width[i],'='));
				}
				report.WriteLine();
				// report data rows.
				foreach (ListViewItem item in listView1.SelectedItems)
				{
					for (int i=0;i<width.Length;i++)
						report.Write( fixWidth(item.SubItems[i].Text,width[i],' ')); 
					report.WriteLine();
				}
				report.WriteLine("\r\nTotal {0} reported: {1}","lines",listView1.SelectedItems.Count);
				report.Close();
				Process.Start("iexplore",reportFile);
			}
		}
		private string fixWidth(string s,int width, char pad)
		{
			if (s.Length > width )
				return s.Substring(0,width)+" ";
			else
				return s.PadRight(width,pad)+" ";
		}

		private bool LoadRegistryFile()
		{
			int retCode = -1;
			General.clearStatusBar();
			if ( openFileDialog1.ShowDialog() == DialogResult.OK )
			{
				Reg.SetBackupRestorePrivileges();
				offlineFile = openFileDialog1.FileName;
			loop2:			retCode = Reg.RegLoadKey(Reg.HKEY_USERS,OFFLINE_SUBKEY,offlineFile);
				if ( retCode == Reg.SUCCESS ) General.Log("Succesfull loaded hive {0}",offlineFile);
				else if ( retCode == Reg.NOT_REGISTRY_FILE ) General.Log("This is not a valid registry file {0}",offlineFile);
				else if ( retCode == Reg.ACCESS_DENIED && CloseRegistryFile() )
					goto loop2;
				else General.LogError("RegLoadKey: {0} {1} {2}",offlineFile,retCode, Reg.GetMessage(retCode));
			}
			return retCode == Reg.SUCCESS;
		}

		private bool CloseRegistryFile()
		{
			General.clearStatusBar();
			Reg.SetBackupRestorePrivileges();
			loop:		int retCode = Reg.RegUnLoadKey(Reg.HKEY_USERS,OFFLINE_SUBKEY);
			if ( retCode == Reg.SUCCESS ) General.Log("Succesfull unloaded hive");
			else if ( retCode == Reg.ACCESS_DENIED && killRegedit())goto loop;
			else if ( retCode == Reg.INVALID_PARAMETER ) General.Log("No hive loaded.");
			else General.LogError("RegUnLoadKey: {0} {1}",retCode, Reg.GetMessage( retCode ));
			return retCode == Reg.SUCCESS;
		}

		private void checkBoxFile_CheckedChanged(object sender, System.EventArgs e)
		{
			if ( !InProcess )
			{
				InProcess = true;
				CloseRegistryFile();
				//				if ( checkBoxFile.Checked )
				//				{
				if ( LoadRegistryFile() )
				{
					checkBoxFile.Checked = true;
					checkBoxHKCU.Checked = checkBoxHKLM.Checked = false;
				}
				else checkBoxFile.Checked = false;
				//				}
				//				else if ( !checkBoxFile.Checked && !CloseRegistryFile() )
				//					checkBoxFile.Checked = true;

				InProcess = false;
			}
		}

		private void checkBoxFile_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
		
		}

		private void mnuDelete_Click(object sender, System.EventArgs e)
		{
			string key = listView1.FocusedItem.SubItems[1].Text; //V107
			string msg = String.Format("Are you sure you want to delete {0} selected key(s) and all of its subkeys?",listView1.SelectedItems.Count);
			if ( MessageBox.Show(msg,"Delete selected key(s) from registry.",MessageBoxButtons.YesNo) == DialogResult.Yes ) //V110
//			if ( MessageBox.Show(key,"Are you sure you want to delete this key and all of its subkeys?",MessageBoxButtons.YesNo) == DialogResult.OK ) //V110
			{
				foreach (ListViewItem item in listView1.SelectedItems) //V110
				{
					key = item.SubItems[1].Text;
					int rk = (key.StartsWith("HKLM"))?Reg.HKEY_LOCAL_MACHINE:Reg.HKEY_CURRENT_USER;
					int retCode = Reg.RegDeleteKey(rk,key.Substring(5));
					if (retCode == Reg.SUCCESS )
					{
						listView1.Items.Remove(item);
					}
					else General.LogError("{0} {1}",Reg.GetMessage(retCode),key);
				}
			}
		}


	}
	public class SortColumn : IComparer
	{
		const  int  UNDEF=1, DATE=2, NUMBER=3, STRING=4;
		static int  stringType;
		static int  sortOnColumn = -1; // Start descending on column 0.
		static bool descending;


		public SortColumn(int column)
		{
			if (column == sortOnColumn)	descending = !descending;	// Toggle sortorder.
			sortOnColumn = column;
			stringType = UNDEF;
		}
		int myTypeOf(string s)
		{
			int stringType = UNDEF;
			if (isNumber(s))	stringType = NUMBER;
			else if (isDate(s)) stringType = DATE;
			else				stringType = STRING;
			return stringType;
		}
		/// <summary>
		/// This function determines if string s can be interpret as a date.
		/// This is the case when:
		/// a) the first digit is a number 0-9.
		/// b) any of the first 5 character must contain a - or / symbol.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		bool isDate(string s)
		{
			if (s.Length<5) return false; //V1704 V1715
			int p = s.IndexOfAny(new Char[] {'-','/'},0,5);
			return p>0 && char.IsNumber(s,0);
		}
		bool isNumber(string s)
		{
			if (s.Length==0) return false; //V1704
			int i=0;
			//V1660		while (i<s.Length && Char.IsNumber(s,i)) i++;
			//V1716		while (i<s.Length && s[i]>='0' && s[i]<='F') i++; //fout bij "C:"
			while (i<s.Length && ((s[i]>='0' && s[i]<='9') ||(s[i]>='A' && s[i]<='F'))) i++;
			return i == s.Length;
		}
		public int Compare(object px,object py)
		{
			string sx=((ListViewItem) px).SubItems[sortOnColumn ].Text;
			string sy=((ListViewItem) py).SubItems[sortOnColumn ].Text;
			if ( stringType == UNDEF) stringType = myTypeOf(sx);
			if ( stringType == DATE)
			{
				DateTime dx = Convert.ToDateTime(sx);
				DateTime dy = Convert.ToDateTime(sy);
				return (descending)?(dx.CompareTo(dy)):(dy.CompareTo(dx));
			}
			else if ( stringType == NUMBER)
			{
				int ix = Convert.ToInt32(sx,16); //V1660
				int iy = Convert.ToInt32(sy,16);
				return (descending)?(ix.CompareTo(iy)):(iy.CompareTo(ix));
			}
			return (descending)?(sx.CompareTo(sy)):(sy.CompareTo(sx));
		}
	}
}
