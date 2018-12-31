using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUIFW;

namespace SpaceShip{
	public class WeaponBtn : MonoBehaviour {

		public string path;
		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void SetGridItem(){
			if (!string.IsNullOrEmpty (path)) {
				MessageCenter.SendMessage ("MpSetPlayGridItem", "path", path);
			}
		}
	}
}
