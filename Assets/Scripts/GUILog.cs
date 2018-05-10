using UnityEngine;
using System.IO;
using System.Collections.Generic;


/// <summary>
/// A class to write a log file for the modules with events useful for debugging and administration.
/// The file produced is a trace file, not to be confused with the log files that modules produce
/// with their output data.
/// </summary>
public class GUILog
{
	public const string GENERAL_EXCEPTION_ERROR = "Unhandled exception occurred during trace log saving";
	public const string LOG_FILENAME_SUFFIX = "_trace.txt";
	
	/// <summary>
	/// A list of all the log messages that have occurred.
	/// </summary>
	private static List<string> logMessages = new List<string>();


	/// <summary>
	/// Returns the filename that will be used when saving out a trace file.
	/// </summary>
	public static string LogFilename
	{
		get
		{
			return Path.Combine(Folders.TraceFilesPath, System.Environment.MachineName + LOG_FILENAME_SUFFIX);
		}
	}
	
	
	/// <summary>
	/// Logs a message with a timestamp and label.
	/// </summary>
	private static void _Log(string format, params System.Object[] args)
	{
		string msg = string.Format("{0:G}: MODULE: {1}", System.DateTime.Now, string.Format(format, args));

		// Only call Debug.Log when in the editor because of performance reasons.
		if (Application.isEditor)
		{
			Debug.Log(msg);
		}
		logMessages.Add(msg);
	}


	/// <summary>
	/// If there are any log messages, this saves the trace log out to disk.
	/// On save, the current log messages are cleared from memory.
	/// </summary>
	public static void SaveLog()
	{
		if (logMessages.Count > 0)
		{
			try
			{
				using (StreamWriter sw = new StreamWriter(LogFilename, true))
				{
					foreach (string logMessage in logMessages)
					{
						sw.WriteLine(logMessage);
					}
					
					// Clear our messages each time we successfully save the log to disk.
					logMessages.Clear();
				}
			}
			catch (System.Exception e)
			{
					// If it wasn't an IOException error, log a Unity error.
					Debug.LogError(string.Format("{0}: {1}", GENERAL_EXCEPTION_ERROR, e.Message));
			}
		}
	}
	

	/// <summary>
	/// Logs a normal message.
	/// </summary>
	public static void Log(string format, params System.Object[] args)
	{
		_Log("INFO: " + format, args);
	}


	/// <summary>
	/// Logs an error message.
	/// </summary>
	public static void Error(string format, params System.Object[] args)
	{
		_Log("ERROR: " + format, args);
		throw new UnityException(string.Format("ERROR: " + format, args));
	}
}