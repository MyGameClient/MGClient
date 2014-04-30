using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Vector3 startPos;
	public Transform target;

	void Start ()
	{
		startPos = transform.position - target.position;
	}

	void LateUpdate ()
	{
		float x = target.position.x;
		float z = target.position.z;

		transform.position = startPos + target.position;
	}
}
