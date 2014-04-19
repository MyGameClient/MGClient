using UnityEngine;
using System.Collections;

public class FightData : MonoBehaviour
{
	public static FightData instance;

	void Awake ()
	{
		instance = this;
		PhotonClient.ProcessResult += ProcessResult;
	}

	public void OnDiable ()
	{
		PhotonClient.ProcessResult -= ProcessResult;
		instance = null;
		ObjectPool.Instance.Clean ();
	}

	void Start ()
	{
		AssetLoader.instance.loadAssetComplete = refresh;
	}

	public void ProcessResult (Bundle bundle)
	{
		if (bundle.figth != null)
		{
			initGO (bundle.figth.map, new Vector3 (0, 0, 1000));
			foreach (Troop player in bundle.figth.players)
			{
				initGO (player.assetbundle, new Vector3 (player.x, player.y, 0));
			}
			foreach (Troop enemy in bundle.figth.enemys)
			{
				initGO (enemy.assetbundle, new Vector3 (enemy.x, enemy.y, 0));
			}
		}
	}

	public void refresh ()
	{
//		initGO ("MP001", new Vector3 (0, 0, 1000));
//		initGO ("PY001", new Vector3 (100, 100, 100));
//
//		for (int i = 0; i <= 4; i++)
//		{
//			initGO ("MS001", new Vector3 (Random.Range (0, CameraController.instance.width), Random.Range (0, CameraController.instance.targetMaxHight), 0));
//			initGO ("MS002", new Vector3 (Random.Range (0, CameraController.instance.width), Random.Range (0, CameraController.instance.targetMaxHight), 0));
//			initGO ("MS003", new Vector3 (Random.Range (0, CameraController.instance.width), Random.Range (0, CameraController.instance.targetMaxHight), 0));
//		}
	}

	GameObject initGO (string id, Vector3 pos)
	{
		GameObject go = GameObject.Instantiate (AssetLoader.instance.getIdByPrefabs(id)) as GameObject;
		go.transform.localPosition = pos;
		return go;
	}
}
