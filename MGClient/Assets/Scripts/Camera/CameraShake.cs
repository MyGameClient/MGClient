using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

	private float lastTime = 0;
	public float duration = 1;
	public float maxScale = 1.1f;
	public float rate = 0.03f;

	void Awake ()
	{
		NotificationCenter.AddObserver (this, "PlayShake");
		NotificationCenter.AddObserver (this, "StopShake");
	}

	public void PlayShake()
	{
		InvokeRepeating ("UpdateShake", 0, rate);
		lastTime = Time.time;
	}

	void UpdateShake()
	{
		float size = CameraController.instance.tkCamera.ZoomFactor;
		CameraController.instance.tkCamera.ZoomFactor = (size == 1) ? maxScale : 1;
//		Debug.Log (camera.orthographicSize);
		if (Time.time - lastTime >= duration)
		{
			StopShake();
		}
	}

	public void StopShake()
	{
		CameraController.instance.tkCamera.ZoomFactor = 1;
		CancelInvoke ("UpdateShake");
	}
}
