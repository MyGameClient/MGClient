using UnityEngine;
using System.Collections;

public class MGMath {

	public static bool attDistance (Unit att, Unit def, float disVal)
	{
		float x = Mathf.Abs(def.transform.position.x - att.transform.position.x);
		float y = Mathf.Abs(def.transform.position.y - att.transform.position.y);
//		Debug.Log (x + "__x__" + disVal / 2);
//		Debug.Log (y + "__x__" + disVal / 4);
		return x <= disVal / 2 && y <= disVal / 10;
	}

	public static bool isFront (Unit att, Unit def)
	{
		if (att.xDir == Unit.Dir.Right)
		{
			return def.transform.position.x >= att.transform.position.x;
		}
		else if (att.xDir == Unit.Dir.Left)
		{
			return def.transform.position.x < att.transform.position.x;
		}
		return false;
	}

}
