using UnityEngine;
using UnityEditor;
using System.Collections;

public class CreatAssetBundle : Editor {

	/*public static readonly string PathURL =  
	#if UNITY_ANDROID   //安卓  
		"jar:file://" + Application.dataPath + "!/assets/";  
	#elif UNITY_IPHONE  //iPhone  
	Application.dataPath + "/Raw/";  
	#elif UNITY_STANDALONE_WIN || UNITY_EDITOR  //windows平台和web平台  
	"file://" + Application.dataPath + "/StreamingAssets/";  
	#else  
	string.Empty;  
	#endif  */

	public static readonly string PathURL =  
#if UNITY_ANDROID   //安卓  
	Application.dataPath + "/MGRes/AD/";
#elif UNITY_IPHONE  //iPhone  
	Application.dataPath + "/MGRes/IOS/";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
	Application.dataPath + "/MGRes/PC/";
#else  
	string.Empty;  
#endif  


	[MenuItem("Custom Editor/Create AssetBunldes Common")]  
	static void CreateAssetBunldesMain ()  
	{  
		Object[] SelectedAsset = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);  
		foreach (Object obj in SelectedAsset)   
		{   
			string targetPath = Application.dataPath + "/StreamingAssets/" + obj.name + ".assetbundle";  

			if (BuildPipeline.BuildAssetBundle (obj, null, targetPath, BuildAssetBundleOptions.CollectDependencies)) 
			{  
				Debug.Log(obj.name +" Creat Success");  
			}   
			else   
			{  
				Debug.Log(obj.name +" Creat fail");  
			}  
		}  
		AssetDatabase.Refresh ();     
		
	}  
}
