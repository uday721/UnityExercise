using UnityEngine;
using System.Collections;
using System.Xml.Linq;


public class TrialResult
{
	const string ATTRIBUTE_SUCCESS = "success";
	const string ATTRIBUTE_RESPONSE_TIME = "responseTime";
	const string ATTRIBUTE_ACCURACY = "accuracy";


	/// <summary>
	/// A reference to the Trial data.
	/// </summary>
	public Trial trial = null;
	/// <summary>
	/// True if the player responded successfully.
	/// This includes NOT responding during noGo trials.
	/// e.g. Returns True if a player did nothing during a trial that has isGo="False"
	/// </summary>
	public bool success = false;
	/// <summary>
	/// Response time in seconds.
	/// The context of this will vary according to the gametype.
	/// </summary>
	public float responseTime = 0;

	/// <summary>
	/// The accuracy of the player's response. 
	/// The calculation for this will vary according to the gametype.
	/// Measured from [0.0 - 1.0]
	/// </summary>
	public float accuracy = 0;


	#region ACCESSORS

	/// <summary>
	/// Returns True if the player responded during the trial.
	/// </summary>
	public bool PlayerResponded
	{
		get
		{
			return responseTime != 0;
		}
	}

	#endregion


	public TrialResult(Trial t)
	{
		trial = t;
	}


	public virtual void WriteOutputData(ref XElement elem)
	{
		XMLUtil.CreateAttribute(ATTRIBUTE_SUCCESS, success.ToString(), ref elem);
		XMLUtil.CreateAttribute(ATTRIBUTE_RESPONSE_TIME, responseTime.ToString(), ref elem);
		XMLUtil.CreateAttribute(ATTRIBUTE_ACCURACY, accuracy.ToString(), ref elem);
	}
}
