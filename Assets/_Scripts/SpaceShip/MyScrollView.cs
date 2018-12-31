using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyScrollView : MonoBehaviour {

	[SerializeField]private RectTransform viewport;
	[SerializeField]private RectTransform content;
	private float curX;
	private float moveX;
	private float step;
	private float frame = 10f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (content.anchoredPosition.x != moveX) {
			step += 1f / frame;
			content.anchoredPosition = new Vector2(Mathf.Lerp(curX,moveX,step),0f);
		}
	}

	public void ShowItem(int index){
		if (index >= 0 && index < content.childCount) {
			step = 0;
			curX = content.anchoredPosition.x;
			moveX = -content.GetChild (index).localPosition.x;
		}
	}
}
