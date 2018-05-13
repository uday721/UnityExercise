using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

public class ReactToColorData : GameData
{
    const string ATTRIBUTE_GUESS_TIMELIMIT = "guessTimeLimit";
    const string ATTRIBUTE_RESPONSE_TIMELIMIT = "responseTimeLimit";
    public const string ATTRIBUTE_DURATION = "duration";

    private float guessTimeLimit = 0;
    private float responseTimeLimit = 0;
    private float duration = 0;

    #region ACCESSORS

    public float GuessTimeLimit
    {
        get
        {
            return guessTimeLimit;
        }
    }
    public float ResponseTimeLimit
    {
        get
        {
            return responseTimeLimit;
        }
    }
    public float GeneratedDuration
    {
        get
        {
            return duration;
        }
    }

    #endregion

    public ReactToColorData(XmlElement elem)
        : base(elem)
    {
    }

    public override void ParseElement(XmlElement elem)
    {
        base.ParseElement(elem);
        XMLUtil.ParseAttribute(elem, ATTRIBUTE_DURATION, ref duration);
        XMLUtil.ParseAttribute(elem, ATTRIBUTE_RESPONSE_TIMELIMIT, ref responseTimeLimit);
        XMLUtil.ParseAttribute(elem, ATTRIBUTE_GUESS_TIMELIMIT, ref guessTimeLimit);
    }

    public override void WriteOutputData(ref XElement elem)
    {
        base.WriteOutputData(ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_GUESS_TIMELIMIT, guessTimeLimit.ToString(), ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_RESPONSE_TIMELIMIT, responseTimeLimit.ToString(), ref elem);
        XMLUtil.CreateAttribute(ATTRIBUTE_DURATION, duration.ToString(), ref elem);
    }
}
