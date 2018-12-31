using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;

namespace SpaceShip{
	
	public enum ItemType{
		none,
		playerCore,
		playerWeapons,
		EnemyCore,
		EnemyWeapons
	}

	public class ShipItem : MonoBehaviour {

		public ItemType type = ItemType.none;
		public BulletPool bulletPool;
		public float time = 3f;
		private float timer = 0;


		// Use this for initialization
		void Start () {
			timer = time;
		}
		
		// Update is called once per frame
		void Update () {
			if (GameController.Instance.gameMode == Mode.fight) {
				if (type == ItemType.playerWeapons) {
					timer -= Time.deltaTime;
					if (timer < 0) {
						timer = time;
						if (bulletPool != null) {
							BulletObject bullet = bulletPool.Pop (transform.position);
							bullet.rigidbody2D.velocity = Vector2.up;
						}
					}
				}
			}
		}


	}


}
