using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace OOP_Laba13 {
	class Program {
		static readonly List<Action> tasks = new List<Action> {
			new Action(DiskInfo),
			new Action(FileInfo),
			new Action(DirInfo),
			new Action(FilesAndDirsOfDrive),
			new Action(FilesWithExtension),
			new Action(Logs)
		};

		static void Main() {
			while (true) {
				try {
					Console.Write(
						"1 - диск" +
						"\n2 - файл" +
						"\n3 - папка" +
						"\n4 - файлы и директории диска" +
						"\n5 - файлы с расширением" +
						"\n6 - Просмотреть логи" +
						"\n0 - выход" +
						"\nВыберите действие: "
						);
					if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 0 || choice > tasks.Count) {
						Console.WriteLine("Нет такого действия");
						continue;
					}
					if (choice == 0) {
						Console.WriteLine("Выход...");
						Environment.Exit(0);
					}
					tasks[choice - 1]();
				}
				catch (FileNotFoundException e) {
					OKVLog.Log("Файл не найден", e);
				}
				catch (DirectoryNotFoundException e) {
					OKVLog.Log("Директорий не найден", e);
				}
				catch (Exception e) {
					OKVLog.Log("Необработанное исключение", e, LogType.ERROR);
				}
				finally {
					Console.ReadKey();
					Console.Clear();
				}
			}
		}


		static void DiskInfo() {
			OKVLog.Log("Просмотрена информация о дисках");
			Console.WriteLine("\n* Место на дисках");
			OKVDiskInfo.FreeSpace();
			Console.WriteLine("\n* Файловые системы дисков");
			OKVDiskInfo.FileSystems();
			Console.WriteLine("\n* Подробная информация");
			OKVDiskInfo.DetailedInfo();
		}

		static void FileInfo() {
			Console.Write("Введите путь до файла: ");
			string path = Console.ReadLine();
			if (!File.Exists(path)) {
				Console.WriteLine("Файл не найден");
				return;
			}
			OKVLog.Log($"Просмотрена информация о файле {OKVFileInfo.GetFullPath(path)}");

			Console.WriteLine($"Информация о файле " + OKVFileInfo.GetName(path));
			OKVFileInfo.FullPath(path);
			OKVFileInfo.Info(path);
			OKVFileInfo.DateInfo(path);
		}

		static void DirInfo() {
			Console.Write("Введите путь директория: ");
			string path = Console.ReadLine();
			if (!Directory.Exists(path)) {
				Console.WriteLine("Директорий не найден");
				return;
			}
			OKVLog.Log($"Просмотрена информация о директории {path}");

			OKVDirInfo.FilesAmt(path);
			OKVDirInfo.CreationDate(path);
			OKVDirInfo.SubDirsAmt(path);
			OKVDirInfo.ParentDirs(path);
		}

		static void FilesAndDirsOfDrive() {
			Console.Write("Введите букву диска: ");
			string drive = Console.ReadLine();
			if (OKVFileManager.GetDrive(drive) == null) {
				Console.WriteLine("Диск не найден");
				return;
			}
			OKVLog.Log("Считаны и записаны файлы и папки диска " + drive.ToUpper());
			OKVFileManager.FilesAndDirs(drive);
			Console.WriteLine("Всё считано и записано");
		}

		static void FilesWithExtension() {
			Console.Write("Введите путь директория: ");
			string path = Console.ReadLine();
			if (!Directory.Exists(path)) {
				Console.WriteLine("Директорий не найден");
				return;
			}
			Console.Write("Введите расширение файлов: ");
			string ext = Console.ReadLine();
			OKVLog.Log($"Поиск файлов в директории {path} с расширением .{ext}");

			OKVFileManager.FilesWithExtension(path, ext);
			Console.WriteLine("Всё считано и записано");
		}

		// TODO: Сделать фильтрацию записей лога
		static void Logs() {
			var logs = OKVLog.GetLogs();
			if (logs.Count == 0) {
				Console.WriteLine("Записей в логах не обнаружено");
				return;
			}
			Console.WriteLine($"\n{logs.Count} записей с {logs[0].Time:G}\n");
			foreach (var item in logs) {
				Console.WriteLine($"\n{item.Type, -7} | {item.Time:G} | {item.Msg}");
				if (item.InnerMsg != "")
					Console.WriteLine("\t" + item.InnerMsg);
				if (item.StackTrace.Count != 0)
					foreach (var stackItem in item.StackTrace)
						Console.WriteLine("\t" + stackItem);
			}
		}
	}
}
