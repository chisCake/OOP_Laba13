using System;
using System.Collections.Generic;
using System.IO;

namespace OOP_Laba13 {
	static class OKVLog {
		public static string File { get; } = "okvlogfile.txt";

		public static void Log(string msg, LogType logType = LogType.INFO) {
			using var sw = new StreamWriter(File, true);
			sw.WriteLineAsync($"{logType}/{DateTime.Now:G}/{msg}");
		}

		public static void Log(string msg, Exception exception, LogType logType = LogType.WARN) {
			try {
				using var sw = new StreamWriter(File, true);
				sw.WriteLineAsync($"{logType}/{DateTime.Now:G}/{msg}");
				if (logType == LogType.WARN)
					sw.WriteLine("\t" + exception.Message);
				else {
					sw.WriteLine("\t" + exception.Message);
					sw.WriteLineAsync(exception.StackTrace);
				}
			}
			catch (Exception e) {
				Log("Что-то очень плохое", e, LogType.ERROR);
			}
		}

		public static List<LogItem> GetLogs() {
			using var sr = new StreamReader(File);
			var logs = new List<LogItem>();

			string line;
			bool firstLog = true;

			LogType type = LogType.UNKNOWN;
			DateTime time = DateTime.Now;
			string msg = "", innerMsg = "";
			var stackTrace = new List<string>();

			while ((line = sr.ReadLine()) != null) {
				if (line[0] == ' ')
					stackTrace.Add(line.Trim());
				else if (line[0] == '\t')
					innerMsg = line.Trim();
				else {
					if (!firstLog) {
						logs.Add(new LogItem(type, time, msg, innerMsg, stackTrace));
						type = LogType.UNKNOWN;
						time = DateTime.Now;
						msg = "";
						innerMsg = "";
						stackTrace = new List<string>();
					}
					else
						firstLog = false;
					var items = line.Split('/');
					type = items[0] switch {
						"INFO" => LogType.INFO,
						"WARN" => LogType.WARN,
						"ERROR" => LogType.ERROR,
						_ => LogType.UNKNOWN
					};
					time = DateTime.Parse(items[1]);
					for (int i = 2; i < items.Length; i++) {
						msg += items[i];
					}
				}
			}
			return logs;
		}
	}

	class LogItem {
		public LogType Type { get; }
		public DateTime Time { get; }
		public string Msg { get; }
		public string InnerMsg { get; }
		public List<string> StackTrace { get; }

		public LogItem(LogType type, DateTime time, string msg, string innerMsg, List<string> stackTrace) {
			Type = type;
			Time = time;
			Msg = msg;
			InnerMsg = innerMsg;
			StackTrace = stackTrace;
		}

		public void PrintStackTrace() {
			foreach (var item in StackTrace)
				Console.WriteLine(item);
		}
	}

	enum LogType {
		INFO,
		WARN,
		ERROR,
		UNKNOWN
	}
}
