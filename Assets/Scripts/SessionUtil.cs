using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;


/// <summary>
/// A utility class for all Session related functions.
/// </summary>
public static class SessionUtil
{
	/// <summary>
	/// Returns a new Trial specific to the given GameType.
	/// </summary>
	public static Trial CreateGameTrial(SessionData data, XmlElement elem = null)
	{
		switch (data.gameType)
		{
			case GameType.React:
				return new ReactTrial(data, elem);

			default:
				return new Trial(data, elem);
		}
	}


	/// <summary>
	/// Returns the appropriate Xml Element name of the given Session gameType.
	/// </summary>
	public static string GetSessionGameElement(SessionData sData)
	{
		switch (sData.gameType)
		{
			case GameType.React:
				return XMLUtil.ELEM_REACT;

			default:
				return string.Empty;
		}
	}


	/// <summary>
	/// Shuffles the given List.
	/// </summary>
	public static void Shuffle<T>(this IList<T> list)
	{
		System.Random rnd = new System.Random((int)System.DateTime.Now.Ticks);
		for (var i = 0; i < list.Count; i++)
		{
			Swap(list, i, rnd.Next(i, list.Count));
		}
	}


	/// <summary>
	/// Helper for the list Shuffle function.
	/// </summary>
	public static void Swap<T>(this IList<T> list, int i, int j)
	{
		var temp = list[i];
		list[i] = list[j];
		list[j] = temp;
	}
}