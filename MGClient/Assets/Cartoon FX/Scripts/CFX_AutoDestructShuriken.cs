using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class CFX_AutoDestructShuriken : MonoBehaviour
{
	void Start ()
	{
		transform.rotation = Quaternion.Euler (new Vector3 (0, Random.Range (0, 361), 0));
	}
	void Update ()
	{
		if(!particleSystem.IsAlive(true))
		{
			GameObject.Destroy(this.gameObject);
		}
	}
}
