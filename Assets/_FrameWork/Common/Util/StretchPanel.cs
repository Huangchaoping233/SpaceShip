using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StretchPanel : MonoBehaviour{//,IPointerEnterHandler,IPointerExitHandler
	public RectTransform rect;

	//开始的时候需要初始化吗
	public float min = 300f;
	public float max = 700;

	public bool startMove = false;
	public float init = 540f;

	private bool isEnter = false;


	void OnMouseDrag(){
		Debug.Log (123131);
	}

	void OnMouseDown(){
		Debug.Log (111111);
	}

	void OnMouseUp(){
		Debug.Log (33333);
	}



//	void Update(){
//		if (isEnter) {
	//			if (Input.GetMouseButtonDown(0)) {
//				
//			}
//		}
//	}
//
//	public void OnPointerEnter(PointerEventData eventData){
//		isEnter = true;
//	}
//
//	public void OnPointerExit(PointerEventData eventData){
//		isEnter = false;
//	}

}
