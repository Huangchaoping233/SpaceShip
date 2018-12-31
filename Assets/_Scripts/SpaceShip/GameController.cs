using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUIFW;

namespace SpaceShip{
	public class GameController : MonoSingleton<GameController> {

		public Mode gameMode = Mode.none;
		private Mode oldMode;

		protected override void Awake ()
		{
			base.Awake ();
			Debuger.EnableLog = true;
			oldMode = gameMode;
		}

		// Use this for initialization
		void Start () {
			gameMode = Mode.start;
		}

		// Update is called once per frame
		void Update () {
			if (oldMode != gameMode) {
				if (oldMode == Mode.start) {
					UIManager.GetInstance ().CloseUIForms (ShipConst.StartMenuPanel);
				} else if(oldMode == Mode.edit){
				}
				oldMode = gameMode;
				if (gameMode == Mode.start) {
					UIManager.GetInstance ().ShowUIForms (ShipConst.StartMenuPanel);
				} else if (gameMode == Mode.main) {
					
				} else if (gameMode == Mode.fight) {
					
				} else if (gameMode == Mode.edit) {
					
				} 
			}
		}
	}

	public enum Mode
	{
		none,//默认
		start,//一开始
		edit,//编辑模式
		main,//主场景
		fight//战斗模式
	}
}