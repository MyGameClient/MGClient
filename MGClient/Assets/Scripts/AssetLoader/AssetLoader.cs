using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetLoader : MonoBehaviour {

	public static AssetLoader instance;
	public void Awake() { instance = this; }
	
	void Start ()
	{

	}

	private IEnumerator LoadAssetRes (string path)
	{
		Debug.Log (path);
		WWW bundle = new WWW(path);  
		yield return bundle;  
		//yield return Instantiate(bundle.assetBundle.mainAsset);  
		
		bundle.assetBundle.Unload(false); 
	}

//	void OnGUI ()
//	{
//		if(GUILayout.Button("Main Assetbundle"))  
//		{  
//			StartCoroutine (LoadAssetRes ("file://" + Application.dataPath + "/MGRes/PC/PY/PY001.assetbundle"));
//			//StartCoroutine (LoadAssetRes ("file://" + Application.dataPath + "/StreamingAssets/Cube.assetbundle"));
//		}  
//	}
}
