using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Gamekit2D;
using SUIFW;
namespace SpaceShip{
	[RequireComponent(typeof(FlexGrid2D))]
	public class ShipController : MonoBehaviour {
		// Use this for initialization
		public string corePath;
		public Transform traContainer;
		public Transform traShowGrid;
		private SpriteRenderer srShowGrid;
		public FlexGrid2D playGrid;
		public BulletPool bulletPool;
		public GameObject preBullet;
		public int bulletCount = 50;

		float screenSpaceZ = 0f;
		GameObject selectEffect;
		Vector3 selectOffset;
		bool isSelect = false;
		ShipItem selectItem = null;

		private bool isHold = false;
		private Vector3 mouseOffset;
		private string path;

		void Start () {
			playGrid = GetComponent<FlexGrid2D> ();
			if (!string.IsNullOrEmpty(corePath)) {
				GameObject core = ResourcesMgr.GetInstance ().LoadAsset (corePath,false);
				SetGridItem (core, Vector2Int.zero);
			}
			screenSpaceZ = Camera.main.WorldToScreenPoint (transform.position).z;
			//选择效果
			selectEffect = ResourcesMgr.GetInstance ().LoadAsset (ShipConst.selectPath,false);
			MyTools.SetObjActive (selectEffect, false);
			srShowGrid = traShowGrid.GetComponent<SpriteRenderer> ();
			SetSize (1, 2, 4, 3);
			bulletPool = BulletPool.GetObjectPool (preBullet, bulletCount);

			path = Application.streamingAssetsPath + @"/Config/PlayerShip.json";
		}
		
		// Update is called once per frame
		void Update () {
			if (GameController.Instance.gameMode == Mode.edit) {
				if (Input.GetMouseButtonDown (0)) {
					#if IPHONE || ANDROID
					if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)){
					#else
					if (EventSystem.current.IsPointerOverGameObject ()){
					#endif
						Debuger.Log ("当前触摸在UI上");
					}
					else {
						Debuger.Log("当前没有触摸在UI上");
						Vector3 pos = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenSpaceZ));
						Vector3 offset = pos - transform.position;
						if (playGrid.Container(offset)) {
							isSelect = true;
							GameObject obj = playGrid.GetGridItem (offset);
							if (obj == null) {
								SetGridItem (selectEffect, offset, false);
								MyTools.SetObjActive (selectEffect, true);
								selectOffset = offset;
								MessageCenter.SendMessage("MainPanelCount","0",0);
							} else {
								selectItem = obj.GetComponent<ShipItem> ();
								MyTools.SetObjActive (selectEffect, false);
								MessageCenter.SendMessage("MainPanelCount","1",1);
							}
						} else {
							MyTools.SetObjActive (selectEffect, false);
							selectItem = null;
							isSelect = false;
							Debug.Log ("在飞船外面");

						}
					}
				}
			}else if(GameController.Instance.gameMode == Mode.fight){
				if(Input.GetMouseButtonUp(0)){
					isHold = false;
				}
				if(Input.GetMouseButtonDown(0)){
					Vector3 pos = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenSpaceZ));
					Vector3 offset = pos - transform.position;
					if (playGrid.Container(offset)) {
						GameObject obj = playGrid.GetGridItem (offset);
						if (obj != null) {
							selectItem = obj.GetComponent<ShipItem> ();
							if(selectItem.type == ItemType.playerCore){
								isHold = true;
								mouseOffset = offset;
							}
						}
					}
				}
				if(isHold){
					transform.position = -mouseOffset + Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenSpaceZ));
				}
			}

		}

		public void SetGridItem(GameObject item,Vector2Int index,bool isPutIn = true){
			Vector3 pos = playGrid.GetGridPos (index);
			item.transform.SetParent (traContainer);
			item.transform.position = traContainer.position + pos;
			if (isPutIn) {
				playGrid.SetGridItem (item, index);
			}
		}

		public void SetGridItem(GameObject item,Vector2 offset,bool isPutIn = true){
			Vector3 pos = playGrid.GetGridPos (offset);
			item.transform.SetParent (traContainer);
			item.transform.position = traContainer.position + pos;
			if (isPutIn) {
				playGrid.SetGridItem (item, offset);
				ShipItem shipItem = item.GetComponent<ShipItem> ();
				if (shipItem != null) {
					shipItem.bulletPool = bulletPool;
				}
			}
		}
			
		public void SetSelet(string path){
			if (isSelect) {
				GameObject item = ResourcesMgr.GetInstance().LoadAsset(path,true);
				SetSelet (item);
			}
		}

		public void SetSelet(GameObject item){
			if (isSelect) {
				SetGridItem (item, selectOffset);
				selectItem = item.GetComponent<ShipItem> ();
				MyTools.SetObjActive (selectEffect, false);
				MessageCenter.SendMessage("MainPanelCount","1",1);
			}
		}

		public void SetSize(int left,int right,int top,int bottom){
			playGrid.left = left;
			playGrid.right = right;
			playGrid.top = top;
			playGrid.bottom = bottom;
			playGrid.ChangePlaceBySize ();

			traShowGrid.localPosition = new Vector3 ((right - left - 0.5f) * 0.5f, (top - bottom -0.5f) * 0.5f, 0f);
			srShowGrid.size = new Vector2 ((left + right + 1) * playGrid.cellX, (top + bottom + 1) * playGrid.cellY);

		}

		public void SetEditBgActive(bool active){
			MyTools.SetObjActive (traShowGrid.gameObject, active);
		}

		public bool SaveData(){
			return ShipConfig.Instance.SaveData (path, playGrid);
		}

		public void OnDie(){
			Debuger.Log ("游戏结束");
		}
	}
}
