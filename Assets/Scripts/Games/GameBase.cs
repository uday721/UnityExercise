using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using System.ComponentModel;


public abstract class GameBase : MonoBehaviour
{
	public delegate void GameEvent();

	/// <summary>
	/// A list of session files that can be played by this Game.
	/// </summary>
	public TextAsset[] sessionFiles;

	public event GameEvent OnStart = delegate { };
	public event GameEvent OnFinished = delegate { };
	public event GameEvent OnPlayerResponse = delegate { };
	public event GameEvent OnStartInput = delegate { };
	public event GameEvent OnEndInput = delegate { };
	public event GameEvent OnStartTrial = delegate { };
	public event GameEvent OnEndTrial = delegate { };

	/// <summary>
	/// Returns true if the player has responded at least once while listenForInput=True.
	/// </summary>
	protected bool playerResponded = false;
	/// <summary>
	/// Becomes True on StartInput and False on EndInput.
	/// </summary>
	protected bool listenForInput = false;
	/// <summary>
	/// Becomes True on StartTrial and False on EndTrial.
	/// </summary>
	protected bool trialInProgress = false;

	protected SessionData sessionData = null;
	
	private Trial currentTrial = null;


	#region ACCESSORS

	public GameType GameType
	{
		get
		{
			return sessionData.gameType;
		}
	}
	public SessionData SessionData
	{
		get
		{
			return sessionData;
		}
	}
	public Trial CurrentTrial
	{
		get
		{
			return currentTrial;
		}
	}
	public bool TrialInProgress
	{
		get
		{
			return trialInProgress;
		}
	}

	public bool WaitingForInput
	{
		get
		{
			return listenForInput;
		}
	}

	#endregion
	

	/// <summary>
	/// Adds a result to the SessionData for the given trial.
	/// </summary>
	protected abstract void AddResult(Trial t, float time);
	
	
	/// <summary>
	/// Called when the game session has started.
	/// </summary>
	public virtual GameBase StartSession(TextAsset sessionFile)
	{
		// Create and load a SessionData object to give to the active game.
		sessionData = new SessionData();
		if (XMLUtil.LoadSessionData(sessionFile, ref sessionData))
		{
			GUILog.Log("Game {0} starting Session {1}", this.gameObject.name, sessionFile.name);
			OnStart();
		}
		else
		{
			GUILog.Error("Game {0} failed to load session file {1}", this.gameObject.name, sessionFile.name);
		}
		return this;
	}


	/// <summary>
	/// Called when the game session is finished.
	/// e.g. All session trials have been completed.
	/// </summary>
	protected virtual void FinishedSession()
	{
		GUILog.SaveLog();
		sessionData.completed = true;
		XMLUtil.WriteSessionLog(ref sessionData);
		OnFinished();
	}


	/// <summary>
	/// Called when the player makes a response during a Trial.
	/// StartInput needs to be called for this to execute, or override the function.
	/// </summary>
	public virtual void PlayerResponded(KeyCode key, float time)
	{
		if (!listenForInput)
		{
			return;
		}
		playerResponded = true;
		OnPlayerResponse();
	}


	/// <summary>
	/// Called when we want to start listening for Input during a Trial.
	/// </summary>
	protected virtual void StartInput()
	{
		listenForInput = true;
		OnStartInput();
	}


	/// <summary>
	/// Called when we want to end listening for Input during a Trial.
	/// </summary>
	protected virtual void EndInput()
	{
		if (!listenForInput)
		{
			return;
		}
		// Only execute if we were already listening for Input.
		listenForInput = false;
		OnEndInput();
	}

	/// <summary>
	/// Called at the beginning of each Trial.
	/// </summary>
	protected virtual void StartTrial(Trial t)
	{
		currentTrial = t;
		trialInProgress = true;
		playerResponded = false;
		OnStartTrial();
	}


	/// <summary>
	/// Called at the end of each Trial.
	/// </summary>
	protected virtual void EndTrial(Trial t)
	{
		currentTrial = null;
		trialInProgress = false;
		if (!playerResponded)
		{
			AddResult(t, 0);
			playerResponded = true;
		}
		OnEndTrial();
	}
}
