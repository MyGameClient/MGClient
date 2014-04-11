﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spell
{
	public float distance = 400;
	public float spd = 1500;
	public float attDistance = 200;
}

public class PlayerController : Unit {

	//spell
	Spell spell = new Spell ();

	public int maxAtt = 2;
	//TODO: need data
	public float distanceTest = 400;
	public float attMoveDis = 15;

	public static List<PlayerController> players = new List<PlayerController> ();
	
	void Awake () {
		players.Add (this);
	}
	
	void Start () 
	{
		CameraController.instance.SetCameraTargetInfo (transform);
		Init ();
	}

	void OnEnable ()
	{
		PlayerInput.inputAttDelegate += inputAttDelegate;
		PlayerInput.inputDirDelegate += inputDirDelegate;
	}

	void OnDisable ()
	{
		players.Remove (this);
		PlayerInput.inputAttDelegate -= inputAttDelegate;
		PlayerInput.inputDirDelegate -= inputDirDelegate;
	}
	
	void inputDirDelegate (Vector2 dir)
	{
		if (isAttack == true || isFall == true)
		{
			return;
		}
		float x = dir.x;
		float y = dir.y;	
		if (x != 0 || y != 0)
		{
			if (x != 0)
			{
				xDir = x > 0 ? Dir.Right : Dir.Left;
			}
		}
		if (isHitted == false)
		{
			Play((x != 0 || y != 0) ? Clip.Walk : Clip.Stand);
		}
		Vector2 op = new Vector2 (x, y);
		transform.Translate (op + op * Main.Instance.hero.speed * Time.deltaTime);
		Vector3 pos = transform.position;
		pos.x = Mathf.Clamp (pos.x, 0, CameraController.instance.width);
		pos.y = Mathf.Clamp (pos.y, 0, CameraController.instance.targetMaxHight);
		pos.z = pos.y;
		transform.position = pos;
	}

	private bool isIng = false;
	private int atInx = 0;
	void inputAttDelegate()
	{
		atInx++;
		atInx = Mathf.Min (atInx, maxAtt);

		if (isIng == false)
		{
			isIng = true;
			StartCoroutine (attInterver ());
		}
	}

	IEnumerator attInterver ()
	{
		for (int i = (int) Clip.Hit; i < (int) Clip.Hit + atInx; i++)
		{
			if (isHitted == false && isFall == false)
			{
				Play ((Clip) i, null, AnimationEventTriggeredAtt);
				yield return new WaitForSeconds (currentClipTime);
			}
		}
		isIng = false;
		atInx = 0;
	}

	void AnimationEventTriggeredAtt (tk2dSpriteAnimator a, tk2dSpriteAnimationClip b, int c)
	{
		//TODO:
		HitTarget ();
		MoveForwrd ();
	}


	#region Hit Method
	private void MoveForwrd ()
	{
		tweenPosition = TweenPosition.Begin (gameObject, 0.1f, MGMath.getClampPos(new Vector3 (attMoveDis * MGMath.getDirNumber (this), 0, 0) + transform.position));
	}

	private void HitTarget ()
	{
		foreach(EnemyController ec in EnemyController.enemys)
		{
			if (Unit.isFront (this, ec) && Unit.attDistance (this, ec, distanceTest))
			{
				if (ec.isFall == false)
				{
					AddEF ("EF001", ec);//TODO:"EF001" need data
					ec.Hitted (currentClip == Clip.AttackLast || currentClip == Clip.spell1 ? Clip.Fall : Clip.Hitted);
					ec.HittedMove (attMoveDis * MGMath.getDirNumber (this), this);
				}
			}
		}
	}
	#endregion

	#region Hitted Method
	#endregion
	public override void ExtraInfo ()
	{}

	#region spell
	public void assault ()
	{
		float dir = MGMath.getDirNumber (this);
		MoveToTarget (transform.position + new Vector3 (dir * spell.distance, 0, 0), spell.spd);
		Play (Clip.spell0);
		tweenPosition.onUpdate = onUpdateAssault;
		tweenPosition.onFinished = onFinishAssault;
	}
	void onUpdateAssault()
	{
		foreach(EnemyController ec in EnemyController.enemys)
		{
			if (Unit.isFront (this, ec) && Unit.attDistance (this, ec, distanceTest))
			{
				if (ec.isFall == false)
				{
					ec.Hitted (Clip.Hitted);
					ec.HittedMove (Mathf.Abs(transform.position.x - tweenPosition.to.x) * MGMath.getDirNumber (this), this);
				}
			}
		}
	}
	void onFinishAssault (UITweener u)
	{
		tweenPosition.onUpdate = null;
	}

	public void JumpAtt ()
	{
		MoveForwrd ();
		Play (Clip.spell1, null, AnimationEventTriggeredAtt);
	}
	#endregion


	#region DEBUG
	public const int max = 2;
	void OnGUI ()
	{
		for (int i = 0; i < max; i++)
		{
			if (GUI.Button (new Rect (100 * i,Screen.height - 100, 100, 100), "spell" + i.ToString()))
			{
				test (i);
			}
		}
	}
	void test(int i)
	{
		if (i == 0)
		{
			assault ();
		}
		else if (1 == 1)
		{
			JumpAtt ();
		}
	}
	#endregion
}
