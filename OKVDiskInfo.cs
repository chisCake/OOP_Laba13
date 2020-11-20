using System;
using System.IO;

namespace OOP_Laba13 {
	static class OKVDiskInfo {
		public static void FreeSpace() {
			long available = 0;
			foreach (var drive in DriveInfo.GetDrives()) {
				available += drive.AvailableFreeSpace;
				Console.WriteLine($"Диск {drive.Name}: {drive.AvailableFreeSpace.ToGB(), -5:F2}гб");
			}
			Console.WriteLine($"\nСвободно места: {available.ToGB(),-5:F2}гб");
		}

		public static void FileSystems() {
			foreach (var drive in DriveInfo.GetDrives())
				Console.WriteLine($"Диск {drive.Name}: {drive.DriveFormat}");
		}

		public static void DetailedInfo() {
			foreach (var drive in DriveInfo.GetDrives()) {
				Console.WriteLine(
					$"\n  Диск {drive.Name}" +
					$"\nВсего    : {drive.TotalSize.ToGB(),-5:F2}гб" +
					$"\nСвободно : {drive.AvailableFreeSpace.ToGB(),-5:F2}гб" +
					$"\nМетка    : {drive.VolumeLabel}"
					);
			}
		}
	}

	static class LongExtension {
		public static double ToGB(this long bytes) => bytes / Math.Pow(2, 30);
	}
}
