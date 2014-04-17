using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class AssetLoader : MonoBehaviour {


	public static readonly string PathURL = 
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
		Application.dataPath + "/MGRes/AD/";
#elif UNITY_ANDROID   //安卓  
		"mnt/sdcard/MGRes/";
#elif UNITY_IPHONE  //iPhone  
	Application.dataPath + "/MGRes/IOS/";
#else  
	string.Empty;  
#endif 



	private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject> ();
	public static AssetLoader instance; void Awake () { instance = this; }

	public delegate void LoadAssetComplete ();
	public LoadAssetComplete loadAssetComplete;

	public void Start ()
	{
		StartCoroutine (LoadAssetRes ());
	}

	List<string> GetPathName ()
	{
		List<string> paths = new List<string> ();

		foreach (string file in Directory.GetDirectories (PathURL))
		{
			foreach (string child in Directory.GetFiles (file))
			{
				if (child.Contains (".meta") == false)
				{
					paths.Add ("file://" + child);
					//Debug.Log ("file://" + child);
				}
			}
		}
		return paths;
	}

	private IEnumerator LoadAssetRes ()
	{
		float last = Time.time;
		DebugConsole.Log ("<----Start Init Resource--->");
		List<string> paths = GetPathName ();
		DebugConsole.Log (paths.Count);
		if (paths.Count > 0)
		{
			foreach (string p in paths)
			{
				WWW bundle = new WWW(p);  
				yield return bundle;
				if (bundle.error == null)
				{
					GameObject prefab = (GameObject) bundle.assetBundle.mainAsset;
					prefabs.Add (prefab.name, prefab);
				}
				else
				{
					Debug.LogError (p + "Load error<-------------->" + bundle.error );
					DebugConsole.LogError (p + "Load error");
				}
				bundle.assetBundle.Unload(false);
			}
			DebugConsole.Log ("<----Init Resource Success--->Cost Time" + (Time.time - last));
			if (loadAssetComplete != null)
			{
				loadAssetComplete ();
			}
		} 
	}


	public GameObject getIdByPrefabs (string id)
	{
		return prefabs[id];
	}

//	void OnGUI ()
//	{
//		if(GUILayout.Button("Main Assetbundle"))  
//		{  
////			StartCoroutine (LoadAssetRes ("file://" + Application.dataPath + "/MGRes/PC/PY/PY001.assetbundle"));
//		}  
//	}
}
