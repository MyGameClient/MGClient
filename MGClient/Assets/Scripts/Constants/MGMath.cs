using UnityEngine;
using System.Collections;
using RD = UnityEngine.Random;

using JsonFx.Json;

public class JsonConvert
{
	public static string SerializeObject (object o)
	{
		return JsonWriter.Serialize (o);
	}

	public static T DeserializeObject<T> (string json)
	{
		return (T)JsonReader.Deserialize (json, typeof (T));
	}
}

public class MGMath {

	/*public static Vector3 getClampPos (Vector3 v)
	{
		float x = Mathf.Clamp (v.x, 0, CameraController.instance.width);
		float y = Mathf.Clamp (v.y, 0, CameraController.instance.targetMaxHight);
		return new Vector3 (x, y, y);
	}

	public static float getDirNumber (Unit u)
	{
		return (u.xDir == Dir.Right) ? 1 : -1;
	}

	public static float getDist2D (Vector3 start, Vector3 end)
	{
		start.z = 0;
		end.z = 0;
		return Mathf.Sqrt ((start - end).sqrMagnitude);
	}

	public static Vector3 getRandom (Transform target, float randge)
	{
		Vector3 v = new Vector3 (target.position.x + RD.Range (-randge, randge), target.position.y + RD.Range (-randge, randge), 0);
		return getClampPos (v);
	}*/

}
