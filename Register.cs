using System;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Register
{
	/// <summary>
	/// Summary description for Reg.
	/// Window types
	/// int		32-bit signed integer
	/// DWORD	32-bit unsigned integer
	/// LPDWORD	reference to DWORD
	/// </summary>
	//[SuppressUnmanagedCodeSecurity]
	public class Reg
	{
		# region Fields
		public const int HKEY_CLASSES_ROOT		= int.MinValue + 0;//0x80000000;
		public const int HKEY_CURRENT_USER		= int.MinValue + 1;//0x80000001;
		public const int HKEY_LOCAL_MACHINE		= int.MinValue + 2;//0x80000002;
		public const int HKEY_USERS				= int.MinValue + 3;//0x80000003;
		public const int HKEY_CURRENT_CONFIG	= int.MinValue + 5;//0x80000005;
		
		public const int SUCCESS			= 0;
		public const int ACCESS_DENIED	= 5;
		public const int SHARING_VIOLATION= 32;
		public const int INVALID_PARAMETER= 87;
		public const int NO_MORE_ITEMS	= 259;
		public const int MORE_DATA		= 234;
		public const int NOT_REGISTRY_FILE= 1017;

		public const int KEY_READ				= 0x20019;
		internal static readonly IntPtr NULL;
    
		public const int REG_SZ					= 1;
		public const int REG_EXPAND_SZ			= 2;
		public const int REG_BINARY				= 3;
		public const int REG_MULTI_SZ			= 7; //V114

		# endregion
		# region External Methodes 
		[DllImport("advapi32.dll", CharSet=CharSet.Auto)]
		internal static extern int RegEnumKeyEx(int hKey, int dwIndex, StringBuilder lpName, out int lpcbName, int[] lpReserved, StringBuilder lpClass, int[] lpcbClass, out long lpftLastWriteTime);
		[DllImport("advapi32.dll", CharSet=CharSet.Auto)]
		internal static extern int RegOpenKeyEx(int hKey, string lpSubKey, int ulOptions, int samDesired, out int hkResult);
		[DllImport("advapi32.dll", CharSet=CharSet.Ansi)]
		internal static extern int RegEnumValue(int hKey, int dwIndex, StringBuilder lpValueName, ref int lpcbValueName, IntPtr lpReserved_MustBeZero, out int lpType, StringBuilder lpData, ref int lpcbData);
		[DllImport("advapi32.dll", CharSet=CharSet.Auto)]
		internal static extern int RegQueryInfoKey(int hKey, StringBuilder lpClass, int[] lpcbClass, IntPtr lpReserved_MustBeZero, out int lpcSubKeys, int[] lpcbMaxSubKeyLen, int[] lpcbMaxClassLen, out int lpcValues, int[] lpcbMaxValueNameLen, int[] lpcbMaxValueLen, int[] lpcbSecurityDescriptor, int[] lpftLastWriteTime);
		[DllImport("advapi32.dll", CharSet=CharSet.Auto)]
		internal static extern int RegCloseKey(int hKey);
		[DllImport("advapi32.dll", CharSet=CharSet.Auto)]
		internal static extern int RegLoadKey(int hKey, string lpSubKey, string lpFile);
		[DllImport("advapi32.dll", CharSet=CharSet.Auto)]
		internal static extern int RegUnLoadKey(int hKey, string lpSubKey);
		[DllImport("advapi32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		internal static extern bool AdjustTokenPrivileges(HandleRef TokenHandle, bool DisableAllPrivileges, Reg.TokenPrivileges NewState, int BufferLength, IntPtr PreviousState, IntPtr ReturnLength);
		[DllImport("advapi32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		public static extern bool LookupPrivilegeValue([MarshalAs(UnmanagedType.LPTStr)] string lpSystemName, [MarshalAs(UnmanagedType.LPTStr)] string lpName, out LUID lpLuid);
		[DllImport("advapi32.dll", CharSet=CharSet.Auto, SetLastError=true)]
		public static extern bool OpenProcessToken(HandleRef ProcessHandle, int DesiredAccess, out IntPtr TokenHandle);
		[DllImport("kernel32.dll", CharSet=CharSet.Ansi, SetLastError=true)]
		public static extern IntPtr GetCurrentProcess();
		[DllImport("kernel32.dll", CharSet=CharSet.Auto)]
		public static extern bool CloseHandle(HandleRef handle);
		[DllImport("kernel32.dll", CharSet=CharSet.Auto)]
		internal static extern int FormatMessage(int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, StringBuilder lpBuffer, int nSize, IntPtr va_list_arguments);
		[DllImport("advapi32.dll", CharSet=CharSet.Auto)]
		internal static extern int RegDeleteKey(int hKey, string lpSubKey);
		# endregion
		# region Internal Methodes
		//
		public static void SetPrivilege(string privilegeName, int attrib)
		{
			Reg.LUID luid1;
			IntPtr ptr1 = IntPtr.Zero;
			luid1 = new Reg.LUID();
			IntPtr ptr2 = Reg.GetCurrentProcess();
			if (!Reg.OpenProcessToken(new HandleRef(null, ptr2), 0x20, out ptr1))
			{
				throw new Win32Exception();
			}
			try
			{
				if (!Reg.LookupPrivilegeValue(null, privilegeName, out luid1))
				{
					throw new Win32Exception();
				}
				Reg.TokenPrivileges privileges1 = new Reg.TokenPrivileges();
				privileges1.Luid = luid1;
				privileges1.Attributes = attrib;
				Reg.AdjustTokenPrivileges(new HandleRef(null, ptr1), false, privileges1, 0, IntPtr.Zero, IntPtr.Zero);
				if (Marshal.GetLastWin32Error() != 0)
				{
					throw new Win32Exception();
				}
			}
			finally
			{
				Reg.CloseHandle(new HandleRef(null, ptr1));
			}
		}
		public static void SetBackupRestorePrivileges()
		{
			SetPrivilege("SeBackupPrivilege",2);
			SetPrivilege("SeRestorePrivilege",2);
		}
 
		public static string GetMessage(int errorCode)
		{
			StringBuilder builder1 = new StringBuilder(0x100);
			int num1 = FormatMessage(12800, NULL, errorCode, 0, builder1, builder1.Capacity + 1, NULL);
			if (num1 != 0)
			{
				return builder1.ToString();
			}
			return "Error" + errorCode.ToString();
		}
		# endregion
		# region Nested Types
		//
		[StructLayout(LayoutKind.Sequential)]
		public struct LUID
		{
			public int LowPart;
			public int HighPart;
		}
		[StructLayout(LayoutKind.Sequential)]
		internal class TokenPrivileges
		{
			public int PrivilegeCount;
			public Reg.LUID Luid;
			public int Attributes;
			public TokenPrivileges()
			{
				this.PrivilegeCount = 1;
				this.Attributes = 0;
			}
		}
		# endregion
	}
}
