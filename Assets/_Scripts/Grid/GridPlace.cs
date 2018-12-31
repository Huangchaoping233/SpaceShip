using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlace : MonoSingleton<GridPlace> {
	public Grid2D playerGrid;
	public Transform container;
//	[HideInInspector] public GridItem selectItem = null;
	Vector2 m_center{
		get { 
			return transform.position;
		}
	}
	float screenSpaceZ = 0f;


	protected override void Awake ()
	{
		base.Awake ();
//		playerGrid = new Grid2D (Vector2.zero,new Vector2(0.5f,0.5f),new Vector2(0.5f,0.5f));
		Debug.Log (playerGrid);
		playerGrid.InitArray ();
	}
	// Use this for initialization
	void Start () {
		screenSpaceZ = Camera.main.WorldToScreenPoint (transform.position).z;
	}
	
	// Update is called once per frame
//	void Update () {
//		if (selectItem != null) {
//			Vector3 pos = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenSpaceZ));
//			Vector3 offset = pos - transform.position;
//			if (playerGrid.GridContainer (offset) && !playerGrid.GetGridTrue (offset)) {
//				if (Input.GetMouseButtonDown (0)) {
//					selectItem.PlaceGrid (playerGrid.GetCellCenterByPos (offset) + m_center);
//					playerGrid.SetGridTrue (offset);
//				} else {
//					selectItem.MoveGrid (playerGrid.GetCellCenterByPos (offset) + m_center);
//				}
//			} else {
//				selectItem.HideGrid ();
//			}
//		}
//	}

	/// <summary>
	///  放置对象,成功返回true
	/// </summary>
	/// <param name="item">Item.</param>
	public bool PlaceWithMouse(GameObject item){
		Vector3 pos = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenSpaceZ));
		return Place (item, pos);
	}

	public bool Place(GameObject item,Vector3 pos){
		Vector3 offset = pos - transform.position;
		if (playerGrid.GridContainer (offset) && playerGrid.GetGridTrue (offset)==null) {
			item.transform.SetParent (container);
			item.transform.position = playerGrid.GetCellCenterByPos (offset) + m_center;
			playerGrid.SetGridTrue (item, offset);
			return true;
		} else {
			return false;
		}
	}

	public bool ContainerWithMouse(){
		Vector3 pos = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenSpaceZ));
		return Container(pos);
	}

	public bool Container(Vector3 pos){
		Vector3 offset = pos - transform.position;
		return playerGrid.GridContainer (offset);
	}

	public GameObject GetGridItemWithMouse(){
		Vector3 pos = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenSpaceZ));
		return GetGridItem (pos);
	}

	public GameObject GetGridItem(Vector3 pos){
		Vector3 offset = pos - transform.position;
		return playerGrid.GetGridTrue (offset);
	}

//	public GameObject GetGridItem(Vector3 pos){
//		Vector3 offset = pos - transform.position;
//		if (playerGrid.GridContainer (offset)) {
//			
//		} else {
//			return null;
//		}
//	}

	void OnDrawGizmosSelected(){
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube (m_center, playerGrid.size);
	}

}
