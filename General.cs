using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using Microsoft.Win32;

	/// <summary>
	/// Add following code snipped to your Form1() constructor.
	/// 
	///	General.StatusBar = this.statusBar1; // From now General.Log can be called.
	///	
	/// </summary>
	public class General
	{
		public static StatusBar StatusBar; //V1650
		public static bool ErrorLogged; //V1755
		public static void LogError(string format, params object[] args)
		{
			Log(0,format,args);
			ErrorLogged = true;
		}
		public static void Log(string format, params object[] args){Log(0,format,args);}
		public static void Log(int panel, string format, params object[] args)
		{
			if ( !ErrorLogged ) //V1755
				StatusBar.Panels[panel].Text = String.Format(format,args);
		}
		public static string GetVersion()
		{
			int i = Application.ProductVersion.LastIndexOf(".");
			return " V" + Application.ProductVersion.Substring(0,i);
		}
		public static void clearStatusBar() //V1755
		{
			Log("");
			ErrorLogged = false; 
			Log(1,"");
		}
        public static string ReadRegistry(string keyName) //V115
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(keyName);
            if ((key == null) || (key.ValueCount == 0))
            {
                return null;
            }
            return key.GetValue("").ToString();
        }
	}

