using UnityEngine;
using System.Collections;

public class MGMath {

	public static bool attDistance (Unit att, Unit def, float disVal)
	{
		float x = Mathf.Abs(def.transform.position.x - att.transform.position.x);
		float y = Mathf.Abs(def.transform.position.y - att.transform.position.y);
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

	public static Vector3 getClampPos (Vector3 v)
	{
		float x = Mathf.Clamp (v.x, 0, CameraController.instance.width);
		float y = Mathf.Clamp (v.y, 0, CameraController.instance.targetMaxHight);
		return new Vector3 (x, y, y);
	}

}
