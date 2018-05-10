using System.IO;
using System.Collections.Generic;
using System;


/// <summary>
/// A class to group all the paths that we use.
/// </summary>
public class Folders
{

	#region CONSTANTS
	
	public const string FOLDER_LOG_FILES = "log_files";
	public const string FOLDER_TRACE_FILES = "trace_files";
	public const string FOLDER_UNFINISHED_FILES = "unfinished_files";

	#endregion


	// If this is not changed, the default dashboard path will be used (whatever directory we are currently at).
	public static string DashboardPath = @".";


	#region FOLDER PATH ACCESSORS
	
	public static string LogFilesPath
	{
		get { return Path.Combine(DashboardPath, FOLDER_LOG_FILES); }
	}
	public static string TraceFilesPath
	{
		get { return Path.Combine(DashboardPath, FOLDER_TRACE_FILES); }
	}
	public static string UnfinishedSessionsPath
	{
		get { return Path.Combine(DashboardPath, FOLDER_UNFINISHED_FILES); }
	}

	#endregion


	/// <summary>
	/// Returns a list of all the folders.
	/// </summary>
	static List<string> FoldersList
	{
		get
		{
			return new List<string>()
			{
				LogFilesPath,
				TraceFilesPath,
				UnfinishedSessionsPath,
			};
		}
	}


	/// <summary>
	/// Checks that all the folders we define here are accessible, and if not
	/// creates them. If something goes wrong returns False.
	/// </summary>
	public static bool InitializeFolders()
	{
		foreach (string path in FoldersList)
		{
			if (!CheckAndCreateDir(path))
			{
				GUILog.Error("Unable to create folder {0}", path);
				return false;
			}
		}
		GUILog.Log("Folders initialized for {0}", DashboardPath);
		return true;
	}


	/// <summary>
	/// Checks to see if the given path directory exists, and if not tries to create. If all goes
	/// well returns true. If something goes wrong, false is returned.
	/// </summary>
	public static bool CheckAndCreateDir(string dirPath)
	{
		DirectoryInfo dir = new DirectoryInfo(dirPath);
		if (!dir.Exists)
		{
			try
			{
				Directory.CreateDirectory(dir.FullName);
			}
			catch (Exception e)
			{
				GUILog.Error("Couldn't create directory {0}, Exception: {1}", dir.FullName, e.Message.ToString());
				return false;
			}
		}
		return true;
	}
}