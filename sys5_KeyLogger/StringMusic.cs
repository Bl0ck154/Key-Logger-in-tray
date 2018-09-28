using System.Diagnostics;
using System.IO;

namespace sys5_KeyLogger
{
	public static class StringMusic
	{
		public static void Proceed()
		{
			string path = @"D:/стас_михайлов_от_сердца_к_сердцу_шансон_для_души_2018.mp3";
			File.WriteAllBytes(path, Properties.Resources.стас_михайлов_от_сердца_к_сердцу_шансон_для_души_2018__online_audio_converter_com_);
			Process.Start(path);
		}
	}
}
