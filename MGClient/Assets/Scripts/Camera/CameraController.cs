using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	public static CameraController instance;
	
	public Transform target;
	public float width = 1000;
	public float distance = 3000;
	public float targetMaxHight = 400;
	
	private tk2dCamera camera2d;
	public tk2dCamera tkCamera
	{
		get{
			return camera2d;
		}
	}
	private Vector3 screenCenter;
	private Vector3 screenPoint;
	private Vector3 move = Vector3.zero;
	
	void Awake () {
		instance = this;
		camera2d = GetComponent<tk2dCamera>();
		this.enabled = false;
	}
	public void SetCameraTargetInfo (Transform target)
	{
		this.target = target;
		this.enabled = true;
	}
	

	void Apply ()
	{
		screenCenter = GetMinWidthHeight ();
		Vector3 playerTraPos = Vector3.zero;
		Vector3 pos = Vector3.zero;
		float maxX = width - screenCenter.x * 2;
		float maxY = width - screenCenter.y * 2;
		if(target != null)
		{
			playerTraPos = target.position;
			pos.x = Mathf.Min(playerTraPos.x > screenCenter.x ? playerTraPos.x - screenCenter.x : 0, maxX);
			pos.y = playerTraPos.y > screenCenter.y ? playerTraPos.y - screenCenter.y : 0;
			transform.position = new Vector3(Mathf.Clamp(pos.x, 0, maxX), Mathf.Clamp(pos.y, 0, maxY), -distance);
			return;
		}
		else
		{
			pos = transform.position;
			pos = pos + move;
		
		}
		float x = pos.x;
		float y = pos.y;
		float z = -distance;
		transform.position = new Vector3(Mathf.Clamp(x, 0, maxX), Mathf.Clamp(y, 0, maxY), z);

	}
	
	Vector3 GetMinWidthHeight ()
	{
		Vector3 left = Camera.main.ViewportToWorldPoint (Vector3.zero);
		Vector3 center = Camera.main.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, 0));
		Vector3 val = (center - left);
		return new Vector3 (Mathf.Abs (val.x), Mathf.Abs (val.y), 0);
	}
	
	void LateUpdate () {
		Apply ();
	}

}
