using System.Collections;
using System.Collections.Generic;

public abstract class Singleton<T>:System.IDisposable where T:new (){
	private static T _instance;
	public static T Instance{
		get {
			if (_instance == null) {
				_instance = new T ();
			}
			return _instance; 
		}
	}

	public virtual void Dispose(){
		
	}
}
