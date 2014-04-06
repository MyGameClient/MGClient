using UnityEngine;
using System.Collections;

public class Main
{
	private static Main _Instance;
	public static Main Instance { get{ if (_Instance == null) { _Instance = new Main(); } return _Instance; } }

	public Role hero
	{
		get{
			//TODO:
			Role h = new Role ();
			h.speed = 60.0f;
			return h;

		}
	}
}
