using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;


/// <summary>
/// Assigns a game to be played by selecting a GameBase from the gamesList and then
/// starts the session using one of it's assigned SessionFiles as a source for the SessionData.
/// </summary>
public class GameController : MonoBehaviour
{
	public InputController inputCtrl;
	public GameBase[] gamesList;
	
	GameBase activeGame;
	

	void Awake()
	{
		// Initialize the logging folders. Required for GUILog usage.
		Folders.InitializeFolders();
	}


	void Start()
	{
		// Assign the game we want to play.
		activeGame = gamesList[0];
		// Start the game session by giving it a Session file.
		activeGame.StartSession(activeGame.sessionFiles[0]);
		// Assign the active game to the Input controller.
		inputCtrl.ActiveGame = activeGame;
	}


	void OnApplicationQuit()
	{
		if (activeGame == null)
		{
			return;
		}
		SessionData sData = activeGame.SessionData;
		// Write the log file for any sessions that haven't been completed but have started.
		if (!sData.outputWritten && sData.SessionStarted)
		{
			XMLUtil.WriteSessionLog(ref sData);
		}
		GUILog.SaveLog();
	}
}
