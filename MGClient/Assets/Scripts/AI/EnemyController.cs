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
}
