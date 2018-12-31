using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class MyTools : MonoBehaviour {

	public static void SetObjActive(GameObject obj,bool active){
		if (obj != null && obj.activeSelf != active) {
			obj.SetActive (active);
		}
	}

	public static void SetBehaviourEnable<T> (T script,bool enabled) where T : Behaviour{
		if (script != null && script.enabled != enabled) {
			script.enabled = enabled;
		}
	}

	public static T ParseEnum<T>(string value)
	{
		return (T) Enum.Parse(typeof(T), value, true);
	}

	public static T ReadJson<T>(string path){
		if (File.Exists (path)) {
			StreamReader sr = new StreamReader (path);
			if (sr != null) {
				string json = sr.ReadToEnd ();
				sr.Close ();
				if (json.Length > 0) {
					return JsonUtility.FromJson<T> (json);
				}
			}
		}
		return default(T);
	}

	public static bool WriteJson<T>(string path,string json){
		try{
			File.WriteAllText (path, json, System.Text.Encoding.UTF8);
			#if UNITY_EDITOR
			UnityEditor.AssetDatabase.Refresh();
			#endif
			return true;
		}
		catch(Exception e){
			Debug.Log (e.Message);
			return false;
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
