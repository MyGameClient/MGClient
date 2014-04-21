using UnityEngine;
using System.Collections;

public class FightData : MonoBehaviour
{
	public Fight fight;

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
			fight = bundle.figth;
			CameraController.instance.width = bundle.figth.map.mapWidth;
			CameraController.instance.targetMaxHight = bundle.figth.map.mapHeigh;
			initGO (bundle.figth.map.mapName, bundle.figth.map.mapPos);
			foreach (Troop player in bundle.figth.players)
			{
				initGO (player.assetbundle, new Vector3 (250, 300, 300), player);
			}
			foreach (Troop enemy in bundle.figth.enemys)
			{
				//if (enemy.assetbundle == "MS004")//TODO:test
					initGO (enemy.assetbundle, new Vector3 (enemy.x, enemy.y, 0), enemy);
			}
		}
	}

	public void refresh ()
	{
		if (fight == null)
		{
			NetTest.FightTest ();
		}
	}

	GameObject initGO (string id, Vector3 pos)
	{
		return initGO (id, pos, null);
	}

	GameObject initGO (string id, Vector3 pos, Troop troop)
	{
		GameObject go = GameObject.Instantiate (AssetLoader.instance.getIdByPrefabs(id)) as GameObject;
		go.transform.localPosition = pos;
		if (troop != null)
		{
			go.GetComponent<Unit>().troop = troop;
		}
		return go;
	}
}
