using UnityEngine;
using System.Collections;

public class FightData : MonoBehaviour
{
	public static FightData instance;

	void Awake ()
	{
		instance = this;
	}

	public void OnDiable ()
	{
		instance = null;
		ObjectPool.Instance.Clean ();
	}

	void Start ()
	{
		AssetLoader.instance.loadAssetComplete = refresh;
	}

	public void refresh ()
	{
		initGO ("MP001", new Vector3 (0, 0, 1000));
		initGO ("PY001", new Vector3 (100, 100, 100));

		for (int i = 0; i <= 4; i++)
		{
			initGO ("MS001", new Vector3 (Random.Range (0, CameraController.instance.width), Random.Range (0, CameraController.instance.targetMaxHight), 0));
			initGO ("MS002", new Vector3 (Random.Range (0, CameraController.instance.width), Random.Range (0, CameraController.instance.targetMaxHight), 0));
			initGO ("MS003", new Vector3 (Random.Range (0, CameraController.instance.width), Random.Range (0, CameraController.instance.targetMaxHight), 0));
		}
	}

	GameObject initGO (string id, Vector3 pos)
	{
		GameObject go = GameObject.Instantiate (AssetLoader.instance.getIdByPrefabs(id)) as GameObject;
		go.transform.localPosition = pos;
		return go;
	}
}
