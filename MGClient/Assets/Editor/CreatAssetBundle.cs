using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;

public class CreatAssetBundle : Editor {

	public static readonly string PathURL =  
#if UNITY_ANDROID   //安卓  
	"AD";
#elif UNITY_IPHONE  //iPhone  
	"IOS";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
	"PC";
#else  
	string.Empty;  
#endif  

	[MenuItem("Custom Editor/Create AssetBunldes Common")]  
	static void CreateAssetBunldesMain ()  
	{  
		BuildTarget bt = BuildTarget.StandaloneWindows;
#if UNITY_ANDROID   //安卓  
		bt = BuildTarget.Android;
#elif UNITY_IPHONE  //iPhone  
		bt = BuildTarget.iPhone;
#endif
		
		Object[] SelectedAsset = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);  
		foreach (Object obj in SelectedAsset)   
		{  
			string file = obj.name.Substring (0,2);
			string targetPath = Application.dataPath + "/MGRes/" + PathURL + "/" + file;
			DirectoryInfo dict = new DirectoryInfo(targetPath);

			if (dict.Exists == false)
			{
				string path =  AssetDatabase.CreateFolder ("Assets/MGRes/" + PathURL, file);

			}
			targetPath += "/" + obj.name + ".assetbundle";  
			Debug.Log (bt);
			if (BuildPipeline.BuildAssetBundle (obj, null, targetPath, BuildAssetBundleOptions.CollectDependencies, bt)) {  
				Debug.Log(obj.name +"Creat success");  
			}   
			else   
			{  
				Debug.Log(obj.name +"Creat fail");  
			}  
		}  
		AssetDatabase.Refresh ();     
		
	}  
}
