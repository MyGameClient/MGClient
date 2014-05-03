using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour {

	public static List<EnemyController> enemys = new List<EnemyController> ();

	public Transform target;

	private NavMeshAgent nav;
	private AnimationPlayer animationPlayer;
	private CharacterController characterController;
	//private bool isBack = false;

	void Start () 
	{
		//animation["Hit"].speed = 2;

		nav = GetComponent<NavMeshAgent>();
		animationPlayer = GetComponent<AnimationPlayer>();
		characterController = GetComponent<CharacterController>();
		enemys.Add (this);
		animationPlayer.animationEventDelegate += animationEventDelegate;
	}

	void OnDisable ()
	{
		enemys.Remove (this);
		animationPlayer.animationEventDelegate += animationEventDelegate;
	}

	void animationEventDelegate (Clip clip)
	{

	}

	// Update is called once per frame
	void Update () 
	{
		if (target)
		{
			Vector3 forward = target.position - transform.position;
			if (forward.x == 0 && forward.z == 0)
			{
			}
			else if (animationPlayer.isFall == false)
			{
				transform.forward  =new Vector3 (forward.x, 0, forward.z);
			}
		}

		if (animationPlayer.dontMove)
		{
			nav.Stop ();
		}
		if (animationPlayer.dontMove)
		{
			return;
		}
		if (target == null)
		{
			animationPlayer.Play (Clip.Idle);
			return;
		}
		nav.destination = target.position;
		if (Vector3.Distance (transform.position, nav.destination) > nav.stoppingDistance)
		{
			animationPlayer.Play (Clip.Run);
		}
		else
		{
			animationPlayer.Play (Clip.Idle);
			//animationPlayer.Play (Clip.Attack1);
		}
	}

	private Vector3 dir;
	private float startTime = 0;
	public void HitTarget (PlayerController attacker)
	{
		if (animationPlayer.isFall)
		{
			return;
		}
		Clip c = Clip.Hit;
		float distance = 1;
		if (attacker.animationPlayer.clip == Clip.Attack3 || attacker.animationPlayer.clip == Clip.Spell1)
		{
			c = Clip.Fall;
			distance = 5;
		}
		//TODO:
		ObjectPool.Instance.LoadObject ("EF/EF001", transform.position);
		animationPlayer.Play (c);
		colorReset (Color.white);
		startTime = Time.time;
		dir = (transform.position - attacker.transform.position).normalized * distance + transform.position;
		CancelInvoke ("ResetBack");
		Invoke ("ResetColor", 0.1f);
		nav.Stop();
		InvokeRepeating ("UpdateBack", 0,  MGMath.UPDATE_RATE);
		Invoke ("ResetBack", 0.1f);
	}

	void UpdateBack ()
	{
		//characterController.SimpleMove (new Vector3 (dir.x, 0, dir.z));
		MGMath.UpdateMove (startTime, transform, dir);
		//transform.position = Vector3.Lerp (transform.position, dir, Time.time - startTime);
	}

	void ResetColor ()
	{
		colorReset (new Color (0.5f, 0.5f, 0.5f, 1.0f));
	}

	void ResetBack ()
	{
		CancelInvoke ("UpdateBack");
	}

	void colorReset (Color c)
	{
		foreach (SkinnedMeshRenderer smr in GetComponentsInChildren<SkinnedMeshRenderer>())
		{
			foreach (Material m in smr.materials)
			{
				m.color = c;
			}
		}
	}
}
