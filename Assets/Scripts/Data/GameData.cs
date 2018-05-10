using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Linq;


/// <summary>
/// Contains data that is specific to this GameType and is shared to all Trials.
/// </summary>
public class GameData
{
	public GameData(XmlElement elem)
	{
		ParseElement(elem);
	}

	public virtual void ParseElement(XmlElement elem) { }

	public virtual void WriteOutputData(ref XElement elem) { }
}


