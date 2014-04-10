using UnityEngine;
using System.Collections;

public class FightData
{
	private static FightData _instance;
	public static FightData instance
	{
		get{
			if (_instance == null)
			{
				_instance = new FightData ();
			}
			return _instance;
		}
	}

	public void clear ()
	{
		_instance = null;
	}
	public void refresh ()
	{
		//add level add MS
	}
}
