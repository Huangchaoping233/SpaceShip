using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShip{
	[System.Serializable]
	public class ShipData{
		public GridConfig gridConfig;
		public List<ShipItemConfig> shipItemConfigs = new List<ShipItemConfig>();
	}
		
	[System.Serializable]
	public class ShipItemConfig{
		public string name;
		public int x;
		public int y;

		public ShipItemConfig(string name,int x,int y){
			this.name = name;
			this.x = x;
			this.y = y;
		}
	}

	[System.Serializable]
	public class GridConfig{
		public float cellX = 0.5f;
		public float cellY = 0.5f;
		public int top = 0;
		public int bottom = 0;
		public int left = 0;
		public int right = 0;

		public GridConfig(){
			
		}

		public GridConfig(float cellX,float cellY,int top,int bottom,int left,int right){
			this.cellX = 0.5f;
			this.cellY = 0.5f;
			this.top = 0;
			this.bottom = 0;
			this.left = 0;
			this.right = 0;
		}
	}


	public class ShipConfig : MonoSingleton<ShipConfig> {
		
		protected override void Awake ()
		{
			base.Awake ();
		}

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public bool SaveData(string path,FlexGrid2D grid){
			ShipData sd = new ShipData ();
			sd.gridConfig = new GridConfig (grid.cellX, grid.cellY, grid.top, grid.bottom, grid.left, grid.right);
			foreach (var kv in grid.GetPlace) {
				ShipItemConfig sic = new ShipItemConfig (kv.Value.name, kv.Key.x, kv.Key.y);
				sd.shipItemConfigs.Add (sic);
			}
			return SaveData (path, sd);
		}

		public ShipData GetData(string path){
			return MyTools.ReadJson<ShipData> (path);
		}

		public bool SaveData(string path,ShipData data){
			string json = JsonUtility.ToJson (data);
			return MyTools.WriteJson<ShipData> (path, json);
		}
	}
}