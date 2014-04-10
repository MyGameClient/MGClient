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
	public float attMoveDis = 15;//TODO: MS DATA


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
		if ((isFall == true || isHitted == true) && _state == State.Hit)
		{
			return;
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
		xDir = (target.x > transform.position.x) ? Dir.Right : Dir.Left;

		tweenPosition = TweenPosition.Begin (gameObject, MGMath.getDist2D (transform.position, MGMath.getClampPos (target)) / speed, MGMath.getClampPos (target));
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
			_state = State.Max;
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
	#endregion
}
