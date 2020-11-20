using System;
using System.IO;

namespace OOP_Laba13 {
	static class OKVFileInfo {
		public static void FullPath(string path) {
			if (!File.Exists(path))
				throw new FileNotFoundException($"Файл {path} не найден");

			Console.WriteLine("Полный путь до файла:\n" + GetFullPath(path));
		}

		public static void Info(string path) {
			if (!File.Exists(path))
				throw new FileNotFoundException($"Файл {path} не найден");

			FileInfo file = new FileInfo(path);
			Console.WriteLine(
				$"Имя файла  : {file.Name}" +
				$"\nРазмер     : {file.Length}б" +
				$"\nРасширение : {file.Extension}"
				);
		}

		public static void DateInfo(string path) {
			if (!File.Exists(path))
				throw new FileNotFoundException($"Файл {path} не найден");

			FileInfo file = new FileInfo(path);
			Console.WriteLine(
				$"Создан  : {file.CreationTime:G}" +
				$"\nИзменён : {file.LastWriteTime:G}"
				);
		}

		public static string GetFullPath(string path) => Path.GetFullPath(path);
		public static string GetName(string path) => new FileInfo(path).Name;
	}
}
