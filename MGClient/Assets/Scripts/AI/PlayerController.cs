using UnityEngine;
using System.Collections;

public enum AttackerType
{
	INF,
	BOW,
}



[RequireComponent(typeof(SpellController))]
[RequireComponent(typeof(AnimationPlayer))]
[RequireComponent(typeof(FPSInputController))]
public class PlayerController : MonoBehaviour {

	public AttackerType attackType = AttackerType.INF;

	public float attDistance = 1.5f;

	public AnimationPlayer animationPlayer;
	public FPSInputController fpsController;
	public SpellController spellController;

	void Start () 
	{
		animationPlayer = GetComponent<AnimationPlayer>();
		fpsController = GetComponent<FPSInputController>();
		spellController = GetComponent<SpellController>();
		animationPlayer.animationEventDelegate += animationEventDelegate;
		spellController.calculateDmgDelegate += calculateDmgDelegate;
	}

	void OnDisable ()
	{
		animationPlayer.animationEventDelegate -= animationEventDelegate;
		spellController.calculateDmgDelegate -= calculateDmgDelegate;
	}

	private Vector3 dir;
	private float startTime = 0;
	void animationEventDelegate (Clip clip)
	{
		if (attackType == AttackerType.INF)
		{
			//startTime = Time.time;
			dir = transform.forward + transform.position;
			InvokeRepeating ("UpdateFront", 0,  MGMath.UPDATE_RATE);
			Invoke ("ResetFront", 0.1f);
			for (int i = 0; i< EnemyController.enemys.Count; i++)
			{
				EnemyController e = EnemyController.enemys[i];
				Vector3 forward = transform.forward;
				Vector3 toOther = e.transform.position - transform.position;
				if (Vector3.Distance (e.transform.position, transform.position) <= attDistance && Vector3.Dot (forward, toOther) >= 0)
				{
					e.HitTarget (this);
				}
			}
		}
		else if (attackType == AttackerType.BOW)
		{
			Debug.Log ("Shoot");
		}
	}

	void calculateDmgDelegate (Spell.SpellType spell)
	{
		if (spell == Spell.SpellType.Spell1)
		{
			for (int i = 0; i< EnemyController.enemys.Count; i++)
			{
				EnemyController e = EnemyController.enemys[i];
				if (Vector3.Distance (e.transform.position, transform.position) <= attDistance)
				{
					e.HitTarget (this);
				}
			}
		}
	}

	void UpdateFront ()
	{
		fpsController.motor.characterController.SimpleMove (new Vector3 (transform.forward.x, 0, transform.forward.z) * 0.5f);
		//MGMath.UpdateMove (startTime, transform, dir);
	}

	void ResetFront ()
	{
		CancelInvoke ("UpdateFront");
	}
	
	// Update is called once per frame
	void Update () {
		fpsController.canControl = !animationPlayer.isAttacking;
		fpsController.motor.enabled = !animationPlayer.isAttacking;
		//fpsController.motor.canControl = !animationPlayer.isAttacking;
		//Normal attack
		if (Input.GetMouseButton (0) || Input.GetKeyDown(KeyCode.J))
		{
			if (attackType == AttackerType.INF)
			{
				Attack ();
			}
			else if (attackType == AttackerType.BOW)
			{
				Shoot ();
			}
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

	void Shoot ()
	{
		animationPlayer.Play (Clip.Attack1);
	}

	private bool IsIng = false;
	private float startAttTime = 0;
	public int maxAttackCount = 2;
	private int attckId = -1;
	void Attack ()
	{
		if (IsIng == false)
		{
			attckId++;
			startAttTime = Time.time;
			IsIng = true;
			StartCoroutine (AttackQueue ());
			return;
		}
		if (Time.time - startAttTime > animationPlayer.length / 2)
		{
			startAttTime = Time.time;
			attckId++;
		}
	}

	IEnumerator AttackQueue ()
	{
		for (int i = (int)Clip.Attack1; i <= Mathf.Min (attckId, maxAttackCount - 1) + (int)Clip.Attack1; i++)
		{
			animationPlayer.Play ((Clip)i);
			yield return new WaitForSeconds (animationPlayer.length);
		}
		attckId = -1;
		IsIng = false;
	}

	void OnGUI ()
	{
		for (int i = 0; i < 3; i++)
		{
			if (GUILayout.Button ("spell" + i.ToString()))
			{
				animationPlayer.Play (Clip.Spell1);
				spellController.shootSpell1 (fpsController.motor);
			}
		}
	}
}
