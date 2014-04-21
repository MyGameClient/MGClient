using UnityEngine;
using System.Collections;

public class MissileObject : MonoBehaviour {

	public float speed = 100;
	public float scale = 1;
	private float x = 0;
	private float dir = 0;
	private float max = 0;
	private Vector3 startPos;

	public GameObject child;

	private MeshRenderer _mr;
	private MeshRenderer mr
	{
		get{
			if (_mr == null)
			{
				_mr = GetComponent<MeshRenderer>();
			}
			return _mr;
		}
	}

	private tk2dSprite _tkSp;
	private tk2dSprite tkSp
	{
		get{
			if (_tkSp == null)
			{
				_tkSp = GetComponent<tk2dSprite>();
			}
			return _tkSp;
		}
	}

	void Start () 
	{
	
	}

	public delegate void HitTargetEvent (Vector3 pos, MissileObject mObj);
	public HitTargetEvent hitTargetEvent;

	void Update () 
	{
		Vector2 op = new Vector2 (dir, 0);
		transform.Translate (op + op * speed * Time.deltaTime);
		if (hitTargetEvent != null)
		{
			Vector3 pos = transform.position;
//			pos.y = startPos.y;
//			pos.z = pos.y;
			hitTargetEvent (pos, this);
		}
		if (Mathf.Abs(transform.position.x - x) >= max)
		{
			gameObject.SetActive (false);
			//ShowMissile ();
		}
	}

	public void ShowMissile ()
	{
		this.enabled = false;
		mr.enabled = false;
		child.SetActive (true);
		hitTargetEvent = null;
	}

	public void Refresh (Vector3 startPos, float dir, float max, HitTargetEvent hitTargetEvent)
	{
		this.enabled = true;
		mr.enabled = true;
		x = transform.position.x;
		this.startPos = startPos;
		this.hitTargetEvent = hitTargetEvent;
		this.dir = dir;
		this.max = max;
		gameObject.SetActive (true);
		transform.localScale = new Vector3 (dir * scale, scale, 1);
	}
}
