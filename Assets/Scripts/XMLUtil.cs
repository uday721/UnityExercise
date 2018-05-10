using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System;
using System.Linq;
using System.Xml.Linq;


/// <summary>
/// A utility class for loading/saving XML files specific to this project.
/// </summary>
public static class XMLUtil
{

	#region VARIOUS CONSTANTS
	
	const string OUTPUT_TIME = "HH:mm:ss";
	const string OUTPUT_TIMESTAMP = "{0}-{1}";
	const string OUTPUT_FILENAME = "{0}_{1}_stats.xml";

	#endregion


	#region ELEM CONSTANTS

	// Gametype elements.
	public const string ELEM_REACT = "react";	

	// General elements.
	const string ELEM_SESSIONS = "sessions";
	const string ELEM_SESSION = "session";
	const string ELEM_SETTINGS = "settings";
	const string ELEM_GENERAL = "general";
	const string ELEM_TRIALS = "trials";
	const string ELEM_TRIAL = "trial";
	const string ELEM_RESULTS = "results";
	const string ELEM_RESULT = "result";

	#endregion
	

	#region OUTPUT ATTRIBUTES

	// Trial & TrialResult attribute, added during WriteSessionLog().
	const string ATTRIBUTE_INDEX = "index";
	
	#endregion
		

	/// <summary>
	/// Loads the session file assigned to the given game.
	/// </summary>
	public static bool LoadSessionData(TextAsset sessionFile, ref SessionData sData)
	{
		XmlDocument doc = new XmlDocument();
		XmlNode settingsElem;
		XmlNode trialsElem;
		try
		{
			doc.LoadXml(sessionFile.text);
			settingsElem = doc.SelectSingleNode("/" + ELEM_SESSION + "/" + ELEM_SETTINGS);
			trialsElem = doc.SelectSingleNode("/" + ELEM_SESSION + "/" + ELEM_TRIALS);

			sData.fileName = sessionFile.name;
			ParseSessionSettings(settingsElem, ref sData);
			ParseSessionTrials(trialsElem, ref sData);

			if (sData.shuffleTrials)
			{
				SessionUtil.Shuffle(sData.trials);
			}

			return true;
		}
		catch(Exception e)
		{
			GUILog.Error("Failed to load Session file: {0}\n{1}", sessionFile.name, e.Message);
			return false;
		}
	}
	

	#region SESSION FILE PARSING

	/// <summary>
	/// Parses all the session file settings.
	/// </summary>
	private static void ParseSessionSettings(XmlNode settingsElement, ref SessionData sData)
	{
		if (settingsElement == null)
		{
			GUILog.Error("Session {0}, settings element not found.", sData.fileName);
			return;
		}
		foreach (XmlNode n in settingsElement.ChildNodes)
		{
			switch (n.Name)
			{
				case ELEM_GENERAL:
					sData.ParseElement(n as XmlElement);
					break;

				case ELEM_REACT:
					sData.gameData = new ReactData(n as XmlElement);
					break;

				default:
					break;
			}
		}
	}


	/// <summary>
	/// Parses all the Trials attributes and Trial elements.
	/// </summary>
	private static void ParseSessionTrials(XmlNode trialsNode, ref SessionData sData)
	{
		if (trialsNode == null)
		{
			GUILog.Error("Session {0}, Trials element not found.", sData.fileName);
			return;
		}

		sData.trials = new List<Trial>();
		foreach (XmlNode n in trialsNode.ChildNodes)
		{
			switch (n.Name)
			{
				case ELEM_TRIAL:
					ParseTrial(n as XmlElement, ref sData);
					break;
			}
		}
		GUILog.Log("Found {0} Trials defined in {1}", sData.trials.Count, sData.fileName);
	}
	

	/// <summary>
	/// Parses the trial in the given element node and adds it to the Trial list.
	/// </summary>
	private static void ParseTrial(XmlElement n, ref SessionData sData)
	{
		Trial t = SessionUtil.CreateGameTrial(sData, n);
		sData.trials.Add(t);
	}

	#endregion
	
		
	#region ATTRIBUTE PARSING

	/// <summary>
	/// Parses a bool attribute.
	/// And assigns it to the referenced variable.
	/// Does nothing if the attribute fails to parse.
	/// </summary>
	public static bool ParseAttribute(XmlNode n, string att, ref bool val, bool optional = false)
	{
		if (n.Attributes[att] != null && bool.TryParse(n.Attributes[att].Value, out val))
		{
			return true;
		}
		if (!optional)
		{
			GUILog.Error("Could not parse attribute {1} under node {0}", n.Name, att);
		}
		return false;
	}


	/// <summary>
	/// Parses a string attribute.
	/// And assigns it to the referenced variable.
	/// Does nothing if the attribute fails to parse.
	/// </summary>
	public static bool ParseAttribute(XmlNode n, string att, ref string val, bool optional = false)
	{
		if (n.Attributes[att] != null)
		{
			val = n.Attributes[att].Value;
			return true;
		}
		if (!optional)
		{
			GUILog.Error("Could not parse attribute {1} under node {0}", n.Name, att);
		}
		return false;
	}


	/// <summary>
	/// Parses an int attribute.
	/// And assigns it to the referenced variable.
	/// Does nothing if the attribute fails to parse.
	/// </summary>
	public static bool ParseAttribute(XmlNode n, string att, ref int val, bool optional = false)
	{
		if (n.Attributes[att] != null &&
			int.TryParse(n.Attributes[att].Value, out val))
		{
			return true;
		}
		if (!optional)
		{
			GUILog.Error("Could not parse attribute {1} under node {0}", n.Name, att);
		}
		return false;
	}


	/// <summary>
	/// Parses a float attribute.
	/// And assigns it to the referenced variable.
	/// Does nothing if the attribute fails to parse.
	/// </summary>
	public static bool ParseAttribute(XmlNode n, string att, ref float val, bool optional = false)
	{
		if (n.Attributes[att] != null &&
			float.TryParse(n.Attributes[att].Value, out val))
		{
			return true;
		}
		if (!optional)
		{
			GUILog.Error("Could not parse attribute {1} under node {0}", n.Name, att);
		}
		return false;
	}


	/// <summary>
	/// Parses a float attribute.
	/// And assigns it to the referenced variable.
	/// Does nothing if the attribute fails to parse.
	/// </summary>
	public static bool ParseAttribute(XmlNode n, string att, ref float? val, bool optional = false)
	{
		float tempVal;
		if (n.Attributes[att] != null &&
			float.TryParse(n.Attributes[att].Value, out tempVal))
		{
			val = tempVal;
			return true;
		}
		if (!optional)
		{
			GUILog.Error("Could not parse attribute {1} under node {0}", n.Name, att);
		}
		return false;
	}


	/// <summary>
	/// Parses a GameType enum attribute.
	/// And assigns it to the referenced variable.
	/// Does nothing if the attribute fails to parse.
	/// </summary>
	public static bool ParseAttribute(XmlNode n, string att, ref GameType val, bool optional = false)
	{
		try
		{
			val = (GameType)Enum.Parse(typeof(GameType), n.Attributes[att].Value, true);
			return true;
		}
		catch
		{
			if (!optional)
			{
				GUILog.Error("Could not parse attribute {1} under node {0}", n.Name, att);
			}
			return false;
		}
	}

	#endregion


	#region WRITE SESSION LOG

	/// <summary>
	/// Creates a log file of the given session data.
	/// </summary>
	public static void WriteSessionLog(ref SessionData sData)
	{
		XDocument doc = new XDocument();
		XElement sessionElem = new XElement(ELEM_SESSION);

		// Session Elem children.
		XElement resultsElem = new XElement(ELEM_RESULTS);
		XElement settingsElem = new XElement(ELEM_SETTINGS);
		XElement trialsElem = new XElement(ELEM_TRIALS);

		// Populate the elements with data.
		WriteResultsElement(sData, ref resultsElem);
		WriteSettingsElement(sData, ref settingsElem);
		WriteTrialsElement(sData, ref trialsElem);

		// Add all the Elements to the document in the proper order.
		doc.Add(sessionElem);
		sessionElem.Add(resultsElem, settingsElem, trialsElem);

		// Save the document.
		string fileName = GetOutputFilepath(sData);
		GUILog.Log("Using output file {0} for module {1}", fileName, sData.GameName);
		try
		{
			doc.Save(fileName);
			sData.outputWritten = true;
		}
		catch (Exception e)
		{
			GUILog.Error("XMLUtil::WriteSessionLog:: Failed to Save output document {0} \n{1}", fileName, e.Message.ToString());
		}
	}


	private static void WriteResultsElement(SessionData sData, ref XElement resultsElem)
	{
		// Results Element children.
		XElement rElem;
		int count = 0;
		foreach (TrialResult r in sData.results)
		{
			rElem = new XElement(ELEM_RESULT);
			CreateAttribute(ATTRIBUTE_INDEX, count.ToString(), ref rElem);
			r.WriteOutputData(ref rElem);
			resultsElem.Add(rElem);
			count++;
		}
	}


	private static void WriteSettingsElement(SessionData sData, ref XElement settingsElem)
	{
		// Settings Elem children.
		XElement generalElem = new XElement(ELEM_GENERAL);
		XElement gameElem = new XElement(SessionUtil.GetSessionGameElement(sData));

		// General Element.
		sData.WriteOutputData(ref generalElem);
		// Game Element.
		sData.gameData.WriteOutputData(ref gameElem);

		settingsElem.Add(generalElem, gameElem);
	}


	private static void WriteTrialsElement(SessionData sData, ref XElement trialsElem)
	{
		// Trials Element children.
		XElement tElem;
		int count = 0;
		foreach (Trial t in sData.trials)
		{
			tElem = new XElement(ELEM_TRIAL);
			CreateAttribute(ATTRIBUTE_INDEX, count.ToString(), ref tElem);
			t.WriteOutputData(ref tElem);
			trialsElem.Add(tElem);
			count++;
		}
	}

	#endregion


	#region CREATE XML ATTRIBUTE

	/// <summary>
	/// Creates an Attribute and assigns a value, to the specified XmlDocument and XmlElement.
	/// </summary>
	public static void CreateAttribute(string attName, string val, ref XElement elem)
	{
		XAttribute att = new XAttribute(attName, val);
		elem.Add(att);
	}

	#endregion


	#region OTHER

	/// <summary>
	/// Returns an output log file path for the current session.
	/// </summary>
	public static string GetOutputFilepath(SessionData sData)
	{
		DateTime Now = DateTime.UtcNow;
		string date = Now.ToShortDateString().Replace('/', '-');
		string time = Now.ToString(OUTPUT_TIME).Replace(':', '-');
		string stamp = string.Format(OUTPUT_TIMESTAMP, date, time);
		string filename = string.Format(OUTPUT_FILENAME, sData.GameName, stamp);

		filename = sData.completed ?
			Path.Combine(Folders.LogFilesPath, filename) :
			Path.Combine(Folders.UnfinishedSessionsPath, filename);
		return filename;
	}

	#endregion

}
