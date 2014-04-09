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


	//hited method
	public void HittedMove (float target, Unit att)
	{
		if (!MGMath.isFront (this, att))
		{
			xDir = (xDir == Dir.Left) ? Dir.Right : Dir.Left;
		}
		TweenPosition.Begin (gameObject, 0.1f, MGMath.getClampPos (transform.position + new Vector3 (target, 0, 0)));
	}

	public void Hitted(Clip c)
	{
		Play (c);
		CancelInvoke ("resetColor");
		Invoke ("resetColor", 0.1f);
		tkSp.color = Color.red;
	}

	public void resetColor ()
	{
		tkSp.color = Color.white;
	}
}
