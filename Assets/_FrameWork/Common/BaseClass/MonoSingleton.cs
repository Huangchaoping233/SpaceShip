using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>{

	#region 单例
	private static  T _instance;
	public static T Instance{
		get {
			if (_instance == null) {
				GameObject obj = new GameObject (typeof(T).Name);
				_instance = obj.AddComponent<T> ();
			}
			return _instance;
		}
	}
	#endregion

	// Use this for initialization
	protected virtual void Awake () {
		_instance = this as T;
	}
}
