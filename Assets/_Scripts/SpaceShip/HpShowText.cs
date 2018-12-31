using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

namespace SpaceShip{
	public class HpShowText : MonoBehaviour {
		[SerializeField]private TextMesh textMesh;

		void Awake(){
			if (textMesh == null) {
				textMesh = GetComponent<TextMesh> ();
			}
		}

		public void OnHpChange(Damageable damageable){
			SetText (damageable.CurrentHealth);
		}

		public void SetText(float val){
			if (val < 0) {
				textMesh.text = "0";
			} else {
				textMesh.text = val.ToString ();
			}
		}
	}
}
