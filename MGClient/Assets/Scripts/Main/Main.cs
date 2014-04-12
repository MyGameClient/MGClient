using UnityEngine;
using System.Collections;

public class Main
{
	private static Main _Instance;
	public static Main Instance { get{ if (_Instance == null) { _Instance = new Main(); } return _Instance; } }

	public Account account;

	public Role hero
	{
		get{
			//TODO:
			Role h = new Role ();
			h.speed = 100.0f;
			return h;

		}
	}
}
