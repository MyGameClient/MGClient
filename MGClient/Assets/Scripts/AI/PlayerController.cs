using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float attDistance = 1.5f;

	private AnimationPlayer animationPlayer;
	private FPSInputController fpsController;
	private SpellController spellController;

	private bool isBack = false;
	private bool isAttMove = false;


	void Start () 
	{
		animationPlayer = GetComponent<AnimationPlayer>();
		fpsController = GetComponent<FPSInputController>();
		spellController = GetComponent<SpellController>();
		animationPlayer.animationEventDelegate += animationEventDelegate;
	}

	void OnDisable ()
	{
		animationPlayer.animationEventDelegate -= animationEventDelegate;
	}

	private Vector3 dir;
	private float startTime = 0;
	void animationEventDelegate (Clip clip)
	{
		startTime = Time.time;
		dir = transform.forward.normalized + transform.position;
		InvokeRepeating ("UpdateFront", 0,  MGMath.UPDATE_RATE);
		Invoke ("ResetFront", 0.1f);
		foreach (EnemyController e in EnemyController.enemys)
		{

			Vector3 forward = transform.forward;
			Vector3 toOther = e.transform.position - transform.position;
			//Debug.Log (Vector3.Distance (e.transform.position, transform.position) + "____" + Vector3.Dot (forward, toOther));
			if (Vector3.Distance (e.transform.position, transform.position) <= attDistance && Vector3.Dot (forward, toOther) >= 0)
			{
				e.HitTarget (transform);
			}
		}
	}

	void UpdateFront ()
	{
		MGMath.UpdateMove (startTime, transform, dir);
	}

	void ResetFront ()
	{
		CancelInvoke ("UpdateFront");
	}
	
	// Update is called once per frame
	void Update () {
		fpsController.canControl = !animationPlayer.isAttacking;
		fpsController.motor.canControl = !animationPlayer.isAttacking;
		//Normal attack
		if (Input.GetMouseButton (0))
		{
			Attack ();
		}
		if (animationPlayer.dontMove)
		{
			return;
		}
		//Move
		Move ();
	}

	void Move ()
	{
		//Move
		if (fpsController.directionVector.x != 0 || fpsController.directionVector.z != 0)
		{
			animationPlayer.Play (Clip.Run);
		}
		else
		{
			animationPlayer.Play (Clip.Idle);
		}
	}

	private bool IsIng = false;
	private bool canResponse = true;
	private int maxAttackCount = 2;
	private int attckId = 0;
	void Attack ()
	{
		if (canResponse == false)
		{
			canResponse = true;
			attckId++;
		}
		if (IsIng == false)
		{
			IsIng = true;
			StartCoroutine (AttackQueue ());
		}
	}

	IEnumerator AttackQueue ()
	{
		for (int i = (int)Clip.Attack1; i <= Mathf.Min (attckId, maxAttackCount - 1) + (int)Clip.Attack1; i++)
		{
			animationPlayer.Play ((Clip)i);
			isAttMove = true;
			yield return new WaitForSeconds (animationPlayer.length / 2);
			canResponse = false;
			yield return new WaitForSeconds (animationPlayer.length / 2);
		}
		attckId = 0;
		IsIng = false;
		canResponse = true;
	}

	void OnGUI ()
	{
		for (int i = 0; i < 3; i++)
		{
			if (GUILayout.Button ("spell" + i.ToString()))
			{
				spellController.shootSpell1 (fpsController.motor);
			}
		}
	}
}
