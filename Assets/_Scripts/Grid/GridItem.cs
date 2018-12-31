using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridItem : MonoBehaviour {

	public string path=string.Empty;
	private GameObject _item;
	private GameObject item{
		get{ 
			if (_item == null) {
				_item = Instantiate (Resources.Load<GameObject> (path));
			}
			return _item;
		}
	}

	private bool isSelect = false;


//	public void OnPointerDown(PointerEventData eventData){
//		if (isSelect) {
//			GridPlace.Instance.selectItem = null;
//			isSelect = false;
//		} else {
//			if (GridPlace.Instance.selectItem != null) {
//				GridPlace.Instance.selectItem.isSelect = false;
//			}
//			GridPlace.Instance.selectItem = this;
//			isSelect = true;
//		}
//	}
		
	public void PlaceGrid(Vector2 pos){
		if (isSelect) {
			isSelect = false;
			item.transform.position = pos;
			item.transform.SetParent (GridPlace.Instance.container);
			_item = null;
		}
	}


	public void MoveGrid(Vector2 pos){
		if (isSelect) {
			SetObjActive (item, true);
			item.transform.position = pos;
		}
	}

	public void HideGrid(){
		if (isSelect) {
			SetObjActive (item, false);
		}
	}

	private void SetObjActive(GameObject obj,bool active){
		if (obj != null && obj.activeSelf != active) {
			obj.SetActive (active);
		}
	}
}
