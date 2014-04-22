using UnityEngine;
using System.Collections;

public class MuchMissileObject : MonoBehaviour {

	public float interval = 0.5f;

	public void OnEnable ()
	{
		StartCoroutine (show ());
	}

	IEnumerator show ()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild (i).gameObject.SetActive (true);
			yield return new WaitForSeconds (interval);
		}
		yield return new WaitForSeconds (1.0f);
		gameObject.SetActive (false);
	}
}
