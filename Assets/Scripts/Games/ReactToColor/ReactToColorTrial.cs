using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

public class ReactToColorTrial : Trial
{
    public float duration = 0;

    #region ACCESSORS

    public float Duration
    {
        get
        {
            return duration;
        }
    }

    #endregion

    public ReactToColorTrial(SessionData data, XmlElement n = null)
        : base(data, n)
    {
    }

    public override void ParseGameSpecificVars(XmlNode n, SessionData session)
    {
        base.ParseGameSpecificVars(n, session);

        ReactData data = (ReactData)(session.gameData);
        if (!XMLUtil.ParseAttribute(n, ReactData.ATTRIBUTE_DURATION, ref duration, true))
        {
            duration = data.GeneratedDuration;
        }
    }

    public override void WriteOutputData(ref XElement elem)
    {
        base.WriteOutputData(ref elem);
        XMLUtil.CreateAttribute(ReactData.ATTRIBUTE_DURATION, duration.ToString(), ref elem);
    }
}
