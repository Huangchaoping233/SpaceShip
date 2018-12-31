using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUIFW;
using UnityEngine.UI;

namespace SpaceShip{

	public class StartMenuPanel : BaseUIForm {
		
		[SerializeField]private Button StartBtn;
		[SerializeField]private Button BossBtn;

//		void Awake(){
//			
//		}

		// Use this for initialization
		void Start () {
			StartBtn.onClick.AddListener (delegate {
				GameController.Instance.gameMode = Mode.edit;
				UIManager.GetInstance ().ShowUIForms (ShipConst.MainPanel);
			});

			BossBtn.onClick.AddListener (delegate {
				GameController.Instance.gameMode = Mode.edit;
				UIManager.GetInstance ().ShowUIForms (ShipConst.BossMainPanel);
			});
		}
		
		// Update is called once per frame
//		void Update () {
//			
//		}
	}
}
