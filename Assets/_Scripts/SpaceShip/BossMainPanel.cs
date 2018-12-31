using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SUIFW;
using DG.Tweening;

namespace SpaceShip{

	public class BossMainPanel : BaseUIForm {

		public GameObject play;
		private ShipController shipCtr;
		public GameObject[] goArray;
		public bool isShow = false;
		public Text text;
		public MyScrollView scroll;

		public enum BossMainPanelType{
			selectWeapon,
			weapon,
			fight,
			scene,
			setting
		}

		void Awake(){
			play = Instantiate (play);
			play.transform.position = Vector3.zero;
			play.transform.localScale = Vector3.one;
			shipCtr = play.GetComponent<ShipController> ();
			shipCtr.SetEditBgActive (false);
			SetHideOrShow (isShow);

			//BMP:Boss Main Panel
			ReceiveMessage ("BMPCount", delegate(KeyValuesUpdate kv) {
				int type = (int)kv.Values;
				scroll.ShowItem (type);
			});


			ReceiveMessage ("BMPActive",delegate(KeyValuesUpdate kv) {
				bool active = ( bool)kv.Values;
				SetHideOrShow(active);
			});

			ReceiveMessage ("BMPSetPlayGridItem",delegate(KeyValuesUpdate kv) {
				string path = (string)kv.Values;

				shipCtr.SetSelet(path);

			});

			ReceiveMessage ("BMPSaveShip",delegate(KeyValuesUpdate kv) {
				//				string path = (string)kv.Values;
				shipCtr.SaveData();

			});


		}

		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {

		}

		public void SetHideOrShowBtn(){
			SetHideOrShow (!isShow);

		}

		public void SetHideOrShow(bool active){
			isShow = active;
			for (int i = 0; i < goArray.Length; i++) {
				MyTools.SetObjActive (goArray [i], active);
			}

			if (active) {
				GameController.Instance.gameMode = Mode.edit;
				text.text = "收起";
				play.transform.DOMove (Vector3.up, 1f);
				if (GameController.Instance.gameMode == Mode.fight) {
					GameController.Instance.gameMode = Mode.edit;
				}
			} else {
				if (GameController.Instance.gameMode == Mode.fight) {
					text.text = "退出";
					play.transform.DOMove (new Vector3(0,-2f,0), 1f);
				}else if(GameController.Instance.gameMode == Mode.main) {
					text.text = "展开";
					play.transform.DOMove (new Vector3(0,-2f,0), 1f);
				}
			}
			shipCtr.SetEditBgActive (active);
		}

		public void SetGameMode(string mode){
			GameController.Instance.gameMode = MyTools.ParseEnum<Mode>(mode);
		}

		public void SaveShipData(){
			shipCtr.SaveData ();
		}
	}
}
