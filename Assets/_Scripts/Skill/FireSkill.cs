using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamekit2D;
using System;
using UnityEngine.Events;

public class FireSkill : Damager {

	public ContactFilter2D filter;
	public Transform fireOrigin;//发射点
	public SpriteRenderer sprite;//技能显示
	public float distance = 1f;//发射距离
	public float minDistance = 0;
	public float fov = 60f;//扇形范围
	private float angle = 0f;


	// Use this for initialization
	void Start () {
		GenerateSprite (sprite, 100f*distance, fov, new Color (1f, 0, 0));

//		Debug.Log(Vector2.Angle (Vector2.left, Vector2.up));
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0)) {
			Vector2 pos = Input.mousePosition;
			Vector2 pos2 = Camera.main.WorldToScreenPoint (fireOrigin.position);
			Vector2 dir = pos - pos2;
			float a = Vector2.Angle (Vector2.up, pos - pos2);
			if (dir.x > 0) {
				a = -a;
			} 
			sprite.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, a));
			sprite.enabled = true;

			Vector2 originPos = fireOrigin.position;
			CheckFanHit (originPos, distance, dir , fov);
//			Vector2 b = dir.normalized;
//			Vector3 c = new Vector3 (b.x, b.y, 0);
//			CheckBoxHit(fireOrigin.position + c*distance,new Vector2(distance,distance*Mathf.Sin ( fov* Mathf.Deg2Rad)),a+90f);
		
		} else {
			sprite.enabled = false;
		}
	}

	private void CheckBoxHit(Vector2 center,Vector2 size,float angle){
		Collider2D[] result = new Collider2D[12];
		int hitCount = Physics2D.OverlapBoxNonAlloc (center, size, angle, result);

		for (int i = 0; i < hitCount; i++)
		{
			Damageable damageable = result[i].GetComponent<Damageable>();
			if (damageable)
			{
				OnDamageableHit.Invoke(this, damageable);
				damageable.TakeDamage(this, false);
				if (disableDamageAfterHit)
					DisableDamage();
			}
			else
			{
				OnNonDamageableHit.Invoke(this);
			}
		}
	}
	/// <summary>
	/// 扇形监测：判断1：是否在圆形内，是否在范围内（角度）
	/// </summary>
	/// <param name="center">Center.</param>
	/// <param name="size">Size.</param>
	/// <param name="angle">Angle.</param>
	private void CheckFanHit(Vector2 center,float size,Vector2 dir,float fov){
		Collider2D[] result = new Collider2D[12];
		int hitCount = Physics2D.OverlapCircleNonAlloc (center, size,result);

		for (int i = 0; i < hitCount; i++)
		{
			 
			Vector2 resultDir = result [i].bounds.ClosestPoint (new Vector3 (dir.x, dir.y, 0));
			if (Vector2.Angle (resultDir - center, dir) < fov * 0.5) {
				Damageable damageable = result[i].GetComponent<Damageable>();
				if (damageable)
				{
					OnDamageableHit.Invoke(this, damageable);
					damageable.TakeDamage(this, false);
					if (disableDamageAfterHit)
						DisableDamage();
				}
				else
				{
					OnNonDamageableHit.Invoke(this);
				}
			}
		}
	}

	/// <summary>
	/// Generates the sprite.
	/// </summary>
	/// <param name="sr">SpriteRenderer.</param>
	/// <param name="size">扇形的半径.</param>
	/// <param name="range">扇形的展开范围（角度）.</param>
	/// <param name="sectorColor">颜色.</param>
	void GenerateSprite(SpriteRenderer sr, float size, float range,Color sectorColor) {
		float radius = size / 2;
		Texture2D t = new Texture2D((int)size,(int)size);
		Vector2 center = new Vector2(radius, 0);
		for (int w = 0; w < size; w++){  
			for (int h = 0; h < size; h ++){

				Color c;
				Vector2 v = new Vector2(w, h) - center;
				float alpha = 0f;

				float angle = GetAngle(Vector2.zero ,v);

				float dis = Vector2.Distance(v, Vector2.zero);
				if(angle > 90-range/2 && angle < 90 + range / 2 &&
					dis >= minDistance){
					float rate = dis / size;
					alpha = Mathf.Min(0.8f - rate*rate*rate,1);

					if( Mathf.Abs(angle - (90 - range / 2)) < 1 || 
						Mathf.Abs(angle - (90 + range / 2)) < 1){
						alpha += 0.5f;
					}
					c = sectorColor * alpha * 0.8f;
				}else{
					c = new Color(0, 0, 0, 0);
				}


				t.SetPixel(w,h,c);  
			}  
		}  
		t.Apply();  

		Sprite pic = Sprite.Create(t, new Rect(0, 0, size, size), new Vector2(0.5f, -0.2f));  
		sr.sprite = pic;  
	}

	float GetAngle(Vector2 a,Vector2 b){
		return 180f - Vector2.Angle (Vector2.right, b);
	}


	public void onHitOther(){
		Debug.Log ("Hit");
	}
}
