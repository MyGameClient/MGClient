using UnityEngine;
using System.Collections;

public class PlayerController : Unit {

	void Start () 
	{
		Init ();
	}

	void OnEnable ()
	{
		PlayerInput.inputAttDelegate += inputAttDelegate;
		PlayerInput.inputDirDelegate += inputDirDelegate;
	}

	void OnDisable ()
	{
		PlayerInput.inputAttDelegate -= inputAttDelegate;
		PlayerInput.inputDirDelegate -= inputDirDelegate;
	}

	void inputDirDelegate (Vector2 dir)
	{
		float x = dir.x;
		float y = dir.y;
		if (x != 0)
		{
			xDir = x > 0 ? Dir.Right : Dir.Left;
			Play (Clip.Walk);
		}
		else
		{
			Play(Clip.Stand);
		}
		Vector2 op = new Vector2 (x, y);
		transform.Translate (op + op * Main.Instance.hero.speed * Time.deltaTime);
	}

	void inputAttDelegate()
	{

	}
}
