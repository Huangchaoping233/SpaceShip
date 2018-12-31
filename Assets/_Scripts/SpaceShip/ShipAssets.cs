using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShip{
	public class ShipAssets : MonoBehaviour {
		//科技点：升级核心，升级武器（类似天赋点），扩充飞船需i*5,只能扩充10（充钱20)
		//			通过通关Boss获取
		public int stPoint;

		//资源点：扩充飞船，提供打Boss资源（每次打boss消耗同等的资源点），消耗品
		//			采集获取，缓慢增长，通关副本
		public int rPoint;



		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}
	}
}
