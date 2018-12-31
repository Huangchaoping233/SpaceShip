using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Grid2D{
	public Vector2 center;//表格中心是固定的
	public Vector2 size;
	public Vector2 cell;

	private GameObject[,] place;
	private int rowSize;
	private int colSize;

	public Grid2D(Vector2 center,Vector2 size,Vector2 cell){
		this.center = center;
		this.size = size;
		this.cell = cell;
		InitArray();
	}

	//表格占位
	public void InitArray(bool isClear = true){
		if (isClear) {
			this.rowSize = GetSizeXCount ();
			this.colSize = GetSizeYCount ();
			this.place = new GameObject[this.rowSize, this.colSize];
		} else {
			int x = GetSizeXCount ();
			int y = GetSizeYCount ();
			GameObject[,] temp = new GameObject[x, y];
			for (int i = Mathf.Max (0, x / 2 - this.rowSize / 2); i < Mathf.Max (0, x / 2 - this.rowSize / 2) + Mathf.Min (this.rowSize, x); i++) {
				for (int j = Mathf.Max (0, y / 2 - this.colSize / 2); j < Mathf.Max (0, y / 2 - this.colSize / 2) + Mathf.Min (this.colSize, y); j++) {
					temp[i,j] = this.place[i+this.rowSize/2-x/2,j+this.colSize/2 - y/2];
				}
			}
			this.rowSize = x;
			this.colSize = y;
			this.place = temp;
		}
	}

	public void  SetGridTrue(GameObject obj,Vector2 pos){
		this.place [Mathf.RoundToInt ((pos.x - this.center.x) / this.cell.x) + rowSize / 2, Mathf.RoundToInt ((pos.y - this.center.y) / this.cell.y) + this.colSize / 2] = obj;
	}

	public GameObject GetGridTrue(Vector2 pos){
		//			Debug.Log (this.place.Length);
		return this.place [Mathf.RoundToInt ((pos.x - this.center.x) / this.cell.x) + rowSize / 2, Mathf.RoundToInt ((pos.y - this.center.y) / this.cell.y) + this.colSize / 2];
	}

	public bool CheckGridSize(){
		return this.rowSize != GetSizeXCount () || this.colSize != GetSizeYCount ();
	}
	/// <summary>
	/// 获得当前表格当前有几个cell.x的长
	/// </summary>
	private int GetSizeXCount(){
		return Mathf.FloorToInt ((this.size.x * 0.5f - this.cell.x * 0.5f) / this.cell.x + 0.00001f) * 2 + 1;
	}
	/// <summary>
	/// 获得当前表格当前有几个cell.y的宽
	/// </summary>
	private int GetSizeYCount(){
		return Mathf.FloorToInt ((this.size.y * 0.5f - this.cell.y * 0.5f) / this.cell.y + 0.00001f) * 2 + 1;
	}

	/// <summary>
	/// 表格是否包含该点
	/// </summary>
	/// <param name="pos">位置是相对于表格中心.</param>
	public bool GridContainer(Vector2 pos){
		//			Vector2 offset = pos - this.center;
		float bigGridSizeX = GetSizeXCount () * this.cell.x;
		float bigGridSizeY = GetSizeYCount () * this.cell.y;
		return BoxContainer (this.center, new Vector2 (bigGridSizeX, bigGridSizeY), pos);
	}

	/// <summary>
	/// 矩形是否包含该点
	/// </summary>
	private bool BoxContainer(Vector2 center,Vector2 size,Vector2 pos){
		return pos.x >= center.x - size.x * 0.5f && pos.x <= center.x + size.x * 0.5f
			&& pos.y >= center.y - size.y * 0.5f && pos.y <= center.y + size.y * 0.5f;
	}

	/// <summary>
	/// 根据位置获得表格元素的位置
	/// </summary>
	/// <param name="pos">位置是相对于表格中心.</param>
	public Vector2 GetCellCenterByPos(Vector2 pos){
		Vector2 offset = pos - center;
		return this.center + new Vector2 (Mathf.RoundToInt (offset.x / cell.x) * cell.x, Mathf.RoundToInt (offset.y / cell.y) * cell.y);
	}
}
