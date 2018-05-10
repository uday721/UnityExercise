using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


/// <summary>
/// Looks for player Input and directs them to the GameLogic.cs
/// </summary>
public class InputController : MonoBehaviour
{
	/// <summary>
	/// 
	/// </summary>
	public delegate void OnResponse(KeyCode key, float timeElapsed);

	#region PRIVATE VARIABLES
	
	/// <summary>
	/// The time in Ticks when a trial was started.
	/// This is reset whenever a new trial begins for the ActiveGame.
	/// </summary>
	long ticksStarted = 0;
	/// <summary>
	/// The game that is currently being played.
	/// </summary>
	GameBase activeGame;
	
	#endregion

	
	public GameBase ActiveGame
	{
		set
		{
			activeGame = value;
			activeGame.OnStartInput -= Reset;
			activeGame.OnStartInput += Reset;
		}
	}


	void Update()
	{
		if (activeGame != null)
		{
			CheckForInput();
		}
	}


	/// <summary>
	/// Should be called at the start of each Trial.
	/// </summary>
	void Reset()
	{
		ticksStarted = DateTime.UtcNow.Ticks;
	}


	/// <summary>
	/// Checks for input and if found, notifies the ActiveGame.
	/// </summary>
	void CheckForInput()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			activeGame.PlayerResponded(KeyCode.Space, CalculateInputTime(ticksStarted, DateTime.UtcNow.Ticks));
		}
	}


	/// <summary>
	/// Converts the response ticks into seconds.
	/// </summary>
	public float CalculateInputTime(long start, long now)
	{
		long difference = now - start;
		return (float)TimeSpan.FromTicks(difference).TotalSeconds;
	}
}