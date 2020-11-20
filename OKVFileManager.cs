using System;
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace OOP_Laba13 {
	static class OKVFileManager {
		public static FileInfo[] GetFiles(string strDrive) {
			var drive = GetDrive(strDrive);
			if (drive == null) {
				OKVLog.Log($"Диск {drive} не найден", LogType.WARN);
				return null;
			}
			return new DirectoryInfo(drive.Name).GetFiles();
		}

		public static DirectoryInfo[] GetDirs(string strDrive) {
			var drive = GetDrive(strDrive);
			if (drive == null) {
				OKVLog.Log($"Диск {drive} не найден", LogType.WARN);
				return null;
			}
			return new DirectoryInfo(drive.Name).GetDirectories();
		}

		public static DriveInfo GetDrive(string drive) {
			foreach (var item in DriveInfo.GetDrives())
				if (item.Name.ToLower()[0] == drive.ToLower()[0])
					return item;
			return null;
		}

		public static void FilesAndDirs(string drive) {
			if (GetDrive(drive) == null) {
				OKVLog.Log($"Диск {drive} не найден", LogType.WARN);
				return;
			}
			var files = GetFiles(drive);
			var dirs = GetDirs(drive);
			Directory.CreateDirectory("OKVInspect");
			using (var sw = new StreamWriter("OKVInspect/okvdirinfo.txt")) {
				sw.WriteLine($"[Диск {drive.ToUpper()}]");
				sw.WriteLine("[Список файлов]");
				foreach (var item in files)
					sw.WriteLine(item.Name);
				sw.WriteLine("[Список директориев]");
				foreach (var item in dirs)
					sw.WriteLine(item.Name);
			}
			File.Copy("OKVInspect/okvdirinfo.txt", "OKVInspect/okvdirinfo_copy.txt", true);
			int failCounter = 0;
			try {
				File.Delete("OKVInspect/okvdirinfo.txt");
			}
			catch (IOException) {
				Delete();
			}


			void Delete() {
				try {
					File.Delete("OKVInspect/okvdirinfo.txt");
				}
				catch (IOException e) {
					Console.WriteLine("timeout");
					Thread.Sleep(100 + 50 * failCounter++);
					if (failCounter == 20) {
						Console.WriteLine("Файл удалён не был");
						OKVLog.Log("Файл удалён не был", new Exception("Превышено время ожидания", e));
						return;
					}
					Delete();
				}
			}
		}

		public static void FilesWithExtension(string path, string ext) {
			if (!Directory.Exists(path))
				throw new DirectoryNotFoundException($"Директория {path} не найдена");

			Directory.CreateDirectory("OKVFiles");
			foreach (var item in new DirectoryInfo("OKVFiles").GetFiles())
				File.Delete(item.FullName);

			var files = new DirectoryInfo(path).GetFiles();
			foreach (var item in files)
				if (item.Extension == "." + ext || item.Extension == ext)
					File.Copy(item.FullName, "OKVFiles/" + item.Name);

			Directory.CreateDirectory("OKVInspect");
			Directory.CreateDirectory("Files");
			foreach (var item in new DirectoryInfo("Files").GetFiles())
				File.Delete(item.FullName);

			Directory.Delete("OKVInspect/OKVFiles", true);
			Directory.Move("OKVFiles", "OKVInspect/OKVFiles");

			File.Delete("OKVInspect/archive.zip");
			ZipFile.CreateFromDirectory("OKVInspect/OKVFiles", "OKVInspect/archive.zip");
			ZipFile.ExtractToDirectory("OKVInspect/archive.zip", "Files");
		}
	}
}
