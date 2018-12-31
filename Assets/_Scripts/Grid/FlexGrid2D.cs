using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlexGrid2D : MonoBehaviour {

	public float cellX = 0.5f;
	public float cellY = 0.5f;
	public int top = 0;
	public int bottom = 0;
	public int left = 0;
	public int right = 0;

	private Dictionary<Vector2Int,GameObject> place = new Dictionary<Vector2Int, GameObject>();
	public Dictionary<Vector2Int,GameObject> GetPlace{
		get{
			return place;
		}
	}

	public GameObject GetGridItem(Vector2Int index){
		GameObject result;
		place.TryGetValue (index,out result);
		return result;
	}

	public GameObject GetGridItem(Vector2 offset){
		int x = Mathf.FloorToInt((offset.x + cellX * 0.5f) / cellX);
		int y = Mathf.FloorToInt((offset.y + cellY * 0.5f) / cellY);
		Vector2Int index = new Vector2Int (x, y);
		return GetGridItem (index);
	}

	public bool Container(Vector2Int index){
		if (index.x >= -left && index.x <= right && index.y >= -bottom && index.y <= top) {
			return true;
		}else{
			return false;
		}
	}

	public bool Container(Vector2 offset){
		int x = Mathf.FloorToInt((offset.x + cellX * 0.5f) / cellX);
		int y = Mathf.FloorToInt((offset.y + cellY * 0.5f) / cellY);
		Vector2Int index = new Vector2Int (x, y);
		return Container (index);
	}

	public bool SetGridItem(GameObject item,Vector2Int index){
		if (Container (index)) {
			if (place.ContainsKey (index)) {
				Destroy (place [index]);
				place [index] = item;
			} else {
				place.Add (index, item);
			}
			return true;
		} else {
			return false;
		}
	}

	public bool SetGridItem(GameObject item,Vector2 offset){
		int x = Mathf.FloorToInt((offset.x + cellX * 0.5f) / cellX);
		int y = Mathf.FloorToInt((offset.y + cellY * 0.5f) / cellY);
		Vector2Int index = new Vector2Int (x, y);
		return SetGridItem (item, index);
	}
		
	public Vector2 GetGridPos(Vector2Int index){
		return new Vector2 (index.x * cellX, index.y * cellY);
	}


	public Vector2 GetGridPos(Vector2 offset){
		int x = Mathf.FloorToInt((offset.x + cellX * 0.5f) / cellX);
		int y = Mathf.FloorToInt((offset.y + cellY * 0.5f) / cellY);
		Vector2Int index = new Vector2Int (x, y);
		return GetGridPos (index);
	}

	public void Clear(){
		foreach (var kv in place) {
			Destroy (kv.Value);
		}
		place.Clear ();
	}

	public void ChangePlaceBySize(){
		List<Vector2Int> del = new List<Vector2Int> ();
		foreach (var kv in place) {
			if (!Container (kv.Key)) {
				Destroy (kv.Value);
				del.Add (kv.Key);
			}
		}
		for (int i = 0; i < del.Count; i++) {
			place.Remove (del [i]);
		}
	}

	public void SaveData(string path){
		
	}


	void OnDrawGizmosSelected(){
		Vector2 center = transform.position;
		Gizmos.color = Color.blue;
		for (int i = -left; i <= right; i++) {
			for (int j = -bottom; j <= top; j++) {
				Gizmos.DrawWireCube (center + new Vector2(i*cellX,j*cellY), new Vector2(cellX,cellY));
			}
		}
		float screenSpaceZ = Camera.main.WorldToScreenPoint (transform.position).z;
		Vector3 pos = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenSpaceZ));
		Vector3 offset = pos - transform.position;
		int x = (int)Mathf.FloorToInt((offset.x + cellX * 0.5f) / cellX);
		int y = (int)Mathf.FloorToInt((offset.y + cellY * 0.5f) / cellY);
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube (center + new Vector2(x*cellX,y*cellY), new Vector2(cellX,cellY));
	}
		
}
