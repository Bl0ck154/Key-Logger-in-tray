using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sys5_KeyLogger
{
	public static class Log
	{
		/// <summary>
		/// Log to file function
		/// </summary>
		/// <param name="text">text to append to the file</param>
		/// <returns>return error message or null if success</returns>
		public static string ToFile(string text)
		{
			try
			{
				string FilePath = $"D:\\some_{DateTime.Now.ToShortDateString()}.txt";
				using (FileStream fs = new FileStream(FilePath, FileMode.Append))
				{
					using (StreamWriter sw = new StreamWriter(fs))
					{
						sw.Write(text);
					}
				}
			}
			catch (Exception ex)
			{
				return ex.Message;
			}

			return null;
		}
	}
}
