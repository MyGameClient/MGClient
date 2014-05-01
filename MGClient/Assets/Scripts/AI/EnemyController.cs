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
//		if (isBack)
//		{
//			transform.position = Vector3.Lerp (transform.position, dir, Time.time - startTime);
//		}
		if (target)
		{
			Vector3 forward = target.position - transform.position;
			transform.forward  =new Vector3 (forward.x, 0, forward.z);
		}

		nav.enabled = !animationPlayer.dontMove;
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
		}
	}

	private Vector3 dir;
	private float startTime = 0;
	public void HitTarget (Transform attacker)
	{
		animationPlayer.Play (Clip.Hit);
		colorReset (Color.white);
		startTime = Time.time;
		dir = (transform.position - attacker.position).normalized;// * 2 + transform.position;
//		Debug.Log (dir);
		CancelInvoke ("ResetBack");
		Invoke ("ResetColor", 0.1f);
		//InvokeRepeating ("UpdateBack", 0,  MGMath.UPDATE_RATE);
		Invoke ("ResetBack", 0.1f);
	}

	void UpdateBack ()
	{
		//characterController.SimpleMove (new Vector3 (dir.x, 0, dir.z));
		//MGMath.UpdateMove (startTime, transform, dir);
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
