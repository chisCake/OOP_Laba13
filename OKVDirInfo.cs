using System;
using System.IO;

namespace OOP_Laba13 {
	static class OKVDirInfo {
		public static void FilesAmt(string path) {
			if (!Directory.Exists(path))
				throw new DirectoryNotFoundException($"Директория {path} не найдена");

			Console.WriteLine($"Кол-во файлов: {new DirectoryInfo(path).GetFiles().Length}");
		}

		public static void CreationDate(string path) {
			if (!Directory.Exists(path))
				throw new DirectoryNotFoundException($"Директория {path} не найдена");

			Console.WriteLine($"Дата создания: {new DirectoryInfo(path).CreationTime:G}");
		}

		public static void SubDirsAmt(string path) {
			if (!Directory.Exists(path))
				throw new DirectoryNotFoundException($"Директория {path} не найдена");

			Console.WriteLine($"Кол-во поддиректориев: {new DirectoryInfo(path).GetDirectories().Length}");
		}

		public static void ParentDirs(string path) {
			if (!Directory.Exists(path))
				throw new DirectoryNotFoundException($"Директория {path} не найдена");

			Console.WriteLine("Список родительских директориев");
			Print(new DirectoryInfo(path).Parent);

			static void Print(DirectoryInfo directory) {
				if (directory == null)
					return;
				Console.WriteLine("  " + directory);
				Print(directory.Parent);
			}
		}
	}
}
