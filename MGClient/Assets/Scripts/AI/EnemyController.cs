using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum State
{
	Walk = 0,
	Stand,
	Hit,
	Max = 3
}

public class EnemyController : Unit {

	public float MSRate = 2.0f;//TODO: LEVEL DATA
	public float speed = 100.0f;//TODO: MS DATA
	public float randge = 100.0f;//TODO: MS DATA
	public float distanceTest = 400;//TODO: MS DATA
	public float attMoveDis = 30;//TODO: MS DATA
	public float hp = 5000;//TODO: MS DATA
	public float hpMax = 5000;//TODO: MS DATA


	public static List<EnemyController> enemys = new List<EnemyController> ();


	private int MAX = 0;
	private State _state = State.Max;
	public State state
	{
		get{
			return _state;
		}
	}
	private PlayerController target
	{
		get{
			//TODO: Should find hate max
			return PlayerController.players[0];
		}
	}

	void Awake () {
		MAX = (int) State.Max;
		enemys.Add (this);
	}

	void OnDisable ()
	{
		CancelAI ();
		enemys.Remove (this);
	}

	void Start ()
	{
		Init ();
		tkAnt.AnimationEventTriggered = AnimationEventTriggeredAtt;
		StartAI ();
	}

	#region Hit States Method
	void StartAI ()
	{
		InvokeRepeating ("UpdatAI", 0, MSRate);
	}
	void CancelAI ()
	{
		CancelInvoke ("UpdatAI");
	}
	void UpdatAI ()
	{
		if ((isFall == true || isHitted == true))
		{
			return;
		}
		if (_state == State.Hit)
		{
			if (isAttack == true)
			{
				return;
			}
		}
		_state = (State) Random.Range (0, MAX);
		switch (_state)
		{
			case State.Stand:
				Stand ();
				break;
			case State.Walk:
				randomWalk (MGMath.getRandom (transform, randge));
				break;
			case State.Hit:
				if (target != null)
				{
					randomWalk (target.transform.position);
				}
				else
				{
					_state = State.Walk;
					randomWalk (MGMath.getRandom (transform, randge));
				}
			break;
		}
	}

	//stand
	void Stand ()
	{
		stop ();
		CompletedPalyStand (null, null);
	}

	//walk
	void randomWalk (Vector3 target)
	{
		MoveToTarget (target, speed);
		Play (Clip.Walk);
		tweenPosition.onUpdate = onUpdate;
		tweenPosition.onFinished = onFinished;
	}
	void onUpdate ()
	{
		//TODO:
		if (state == State.Hit)
		{
			Hit ();
		}
	}
	void onFinished (UITweener t)
	{
		if (_state == State.Hit)
		{
			Hit ();
			//_state = State.Max;
		}
		CompletedPalyStand (null, null);
	}

	void Hit ()
	{
		if (isFall == true)
		{
			return;
		}
		if (Unit.attDistance (this, target, distanceTest) == true)
		{
			if (Unit.isFront (this, target) == false)
			{
				xDir = (xDir == Dir.Left) ? Dir.Right : Dir.Left;
			}
			stop ();
			Play (Clip.Hit);
		}
	}
	void AnimationEventTriggeredAtt (tk2dSpriteAnimator a, tk2dSpriteAnimationClip b, int c)
	{
		if (Unit.attDistance (this, target, distanceTest) == true)
		{
			target.AddEF ("EF001", target);
			target.Hitted (Clip.Hitted);
			target.HittedMove (attMoveDis * MGMath.getDirNumber (this), this);
		}
	}
	#endregion


	#region Hitted Method
	public override void ApplyDmg (float dmg, bool isSlider)
	{
		hp -= dmg;
		//if (isSlider == true)
		{
			BloodSlider.instance.Refresh (hp / hpMax);
		}
		if (hp <= 0)
		{
			Play (Clip.Die, DieOnComplete, null);
			OnDisable ();
		}
	}
	void DieOnComplete (tk2dSpriteAnimator a, tk2dSpriteAnimationClip b)
	{
		TweenTk2dColor.Begin (gameObject, 0.5f, new Color (tkSp.color.r, tkSp.color.g, tkSp.color.b, 0)).onFinished = DestoryDone;
	}
	void DestoryDone (UITweener u)
	{
		gameObject.SetActive (false);
	}
	#endregion
	public override void ExtraInfo ()
	{
		//_state = State.Max;
	}
}
