using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : Unit {

	public static List<EnemyController> enemys = new List<EnemyController> ();
	
	void Awake () {
		enemys.Add (this);
	}

	void OnDisable ()
	{
		enemys.Remove (this);
	}

	void Start ()
	{
		Init ();
	}

	public void Hitted()
	{
		Play (Clip.Hitted);
		CancelInvoke ("resetColor");
		Invoke ("resetColor", 0.1f);
		tkSp.color = Color.red;
	}

	public void resetColor ()
	{
		tkSp.color = Color.white;
	}
}
