using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Gamekit2D{
	
	[RequireComponent(typeof(CharacterController2D))]
	public class PlayerController : MonoBehaviour {

		static protected PlayerController s_PlayerInstance;
		static public PlayerController PlayerInstance { get { return s_PlayerInstance; } }

		public Damageable damageable;
		public Transform cameraFollowTarget;

		public float maxSpeed = 10f;
		public float groundAcceleration = 100f;
		public float groundDeceleration = 100f;

		[Range(0f, 1f)] public float airborneAccelProportion;
		[Range(0f, 1f)] public float airborneDecelProportion;
		public float gravity = 50f;
		public float jumpSpeed = 20f;
		public float jumpAbortSpeedReduction = 100f;

		public float cameraHorizontalFacingOffset;
		public float cameraHorizontalSpeedOffset;
		public float cameraVerticalInputOffset;
		public float maxHorizontalDeltaDampTime;
		public float maxVerticalDeltaDampTime;
		public float verticalCameraOffsetDelay;

		protected CharacterController2D m_CharacterController2D;

		protected BoxCollider2D m_Capsule;
		protected Transform m_Transform;
		protected Vector2 m_MoveVector;
		protected TileBase m_CurrentSurface;
		protected float m_CamFollowHorizontalSpeed;
		protected float m_CamFollowVerticalSpeed;
		protected float m_VerticalCameraOffsetTimer;

		protected Vector2 m_StartingPosition = Vector2.zero;

		protected bool m_InPause = false;

		protected const float k_GroundedStickingVelocityMultiplier = 3f;

		protected ContactPoint2D[] m_ContactsBuffer = new ContactPoint2D[16];

		void Awake()
		{
			s_PlayerInstance = this;

			m_CharacterController2D = GetComponent<CharacterController2D>();
			m_Capsule = GetComponent<BoxCollider2D>();
			m_Transform = transform;
		}

		void Start(){
			if (!Mathf.Approximately(maxHorizontalDeltaDampTime, 0f))
			{
				float maxHorizontalDelta = maxSpeed * cameraHorizontalSpeedOffset + cameraHorizontalFacingOffset;
				m_CamFollowHorizontalSpeed = maxHorizontalDelta / maxHorizontalDeltaDampTime;
			}

			if (!Mathf.Approximately(maxVerticalDeltaDampTime, 0f))
			{
				float maxVerticalDelta = cameraVerticalInputOffset;
				m_CamFollowVerticalSpeed = maxVerticalDelta / maxVerticalDeltaDampTime;
			}

			m_StartingPosition = transform.position;
		}
		
		void Update()
		{
			if (PlayerInput.Instance.Pause.Down)
			{
				if (!m_InPause)
				{
					if (ScreenFader.IsFading)
						return;

					PlayerInput.Instance.ReleaseControl(false);
					PlayerInput.Instance.Pause.GainControl();
					m_InPause = true;
					Time.timeScale = 0;
					UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("UIMenus", UnityEngine.SceneManagement.LoadSceneMode.Additive);
				}
				else
				{
					Unpause();
				}
			}
			if (CheckForGrounded ()) {
				GroundedHorizontalMovement (true);
				if (CheckForJumpInput ()) {
					SetVerticalMovement(jumpSpeed);
				}
			} else {
				AirborneHorizontalMovement ();
			}
				
		}

		void FixedUpdate()
		{ 
			m_CharacterController2D.Move(m_MoveVector * Time.deltaTime);
			if (CheckForGrounded ()) {
				
			} else {
				UpdateJump ();
				AirborneVerticalMovement ();
			}



		}

		public void Unpause()
		{
			//if the timescale is already > 0, we 
			if (Time.timeScale > 0)
				return;

			StartCoroutine(UnpauseCoroutine());
		}

		protected IEnumerator UnpauseCoroutine()
		{
			Time.timeScale = 1;
			UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("UIMenus");
			PlayerInput.Instance.GainControl();
			//we have to wait for a fixed update so the pause button state change, otherwise we can get in case were the update
			//of this script happen BEFORE the input is updated, leading to setting the game in pause once again
			yield return new WaitForFixedUpdate();
			yield return new WaitForEndOfFrame();
			m_InPause = false;
		}

		protected void UpdateCameraFollowTargetPosition()
		{
			float newLocalPosX;
			float newLocalPosY = 0f;

			float desiredLocalPosX = cameraHorizontalFacingOffset;
			desiredLocalPosX += m_MoveVector.x * cameraHorizontalSpeedOffset;
			if (Mathf.Approximately(m_CamFollowHorizontalSpeed, 0f))
				newLocalPosX = desiredLocalPosX;
			else
				newLocalPosX = Mathf.Lerp(cameraFollowTarget.localPosition.x, desiredLocalPosX, m_CamFollowHorizontalSpeed * Time.deltaTime);

			bool moveVertically = false;
			if (!Mathf.Approximately(PlayerInput.Instance.Vertical.Value, 0f))
			{
				m_VerticalCameraOffsetTimer += Time.deltaTime;

				if (m_VerticalCameraOffsetTimer >= verticalCameraOffsetDelay)
					moveVertically = true;
			}
			else
			{
				moveVertically = true;
				m_VerticalCameraOffsetTimer = 0f;
			}

			if (moveVertically)
			{
				float desiredLocalPosY = PlayerInput.Instance.Vertical.Value * cameraVerticalInputOffset;
				if (Mathf.Approximately(m_CamFollowVerticalSpeed, 0f))
					newLocalPosY = desiredLocalPosY;
				else
					newLocalPosY = Mathf.MoveTowards(cameraFollowTarget.localPosition.y, desiredLocalPosY, m_CamFollowVerticalSpeed * Time.deltaTime);
			}

			cameraFollowTarget.localPosition = new Vector2(newLocalPosX, newLocalPosY);
		}

		public void SetMoveVector(Vector2 newMoveVector)
		{
			m_MoveVector = newMoveVector;
		}

		public void SetHorizontalMovement(float newHorizontalMovement)
		{
			m_MoveVector.x = newHorizontalMovement;
		}

		public void SetVerticalMovement(float newVerticalMovement)
		{
			m_MoveVector.y = newVerticalMovement;
		}

		public void IncrementMovement(Vector2 additionalMovement)
		{
			m_MoveVector += additionalMovement;
		}

		public void IncrementHorizontalMovement(float additionalHorizontalMovement)
		{
			m_MoveVector.x += additionalHorizontalMovement;
		}

		public void IncrementVerticalMovement(float additionalVerticalMovement)
		{
			m_MoveVector.y += additionalVerticalMovement;
		}

		public void GroundedVerticalMovement()
		{
			m_MoveVector.y -= gravity * Time.deltaTime;

			if (m_MoveVector.y < -gravity * Time.deltaTime * k_GroundedStickingVelocityMultiplier)
			{
				m_MoveVector.y = -gravity * Time.deltaTime * k_GroundedStickingVelocityMultiplier;
			}
		}

		public Vector2 GetMoveVector()
		{
			return m_MoveVector;
		}

		public void GroundedHorizontalMovement(bool useInput, float speedScale = 1f)
		{
			float desiredSpeed = useInput ? PlayerInput.Instance.Horizontal.Value * maxSpeed * speedScale : 0f;
			float acceleration = useInput && PlayerInput.Instance.Horizontal.ReceivingInput ? groundAcceleration : groundDeceleration;
			m_MoveVector.x = Mathf.MoveTowards(m_MoveVector.x, desiredSpeed, acceleration * Time.deltaTime);
		}

		public bool CheckForGrounded()
		{
			bool grounded = m_CharacterController2D.IsGrounded;

			if (grounded)
			{
				FindCurrentSurface();

//				if (m_MoveVector.y < -1.0f)
//				{//only play the landing sound if falling "fast" enough (avoid small bump playing the landing sound)
//					landingAudioPlayer.PlayRandomSound(m_CurrentSurface);
//				}
			}
			else
				m_CurrentSurface = null;

//			m_Animator.SetBool(m_HashGroundedPara, grounded);

			return grounded;
		}

		public void FindCurrentSurface()
		{
			Collider2D groundCollider = m_CharacterController2D.GroundColliders[0];

			if (groundCollider == null)
				groundCollider = m_CharacterController2D.GroundColliders[1];

			if (groundCollider == null)
				return;

			TileBase b = PhysicsHelper.FindTileForOverride(groundCollider, transform.position, Vector2.down);
			if (b != null)
			{
				m_CurrentSurface = b;
			}
		}

		public void UpdateJump()
		{
			if (!PlayerInput.Instance.Jump.Held && m_MoveVector.y > 0.0f)
			{
				m_MoveVector.y -= jumpAbortSpeedReduction * Time.deltaTime;
			}
		}

		public void AirborneHorizontalMovement()
		{
			float desiredSpeed = PlayerInput.Instance.Horizontal.Value * maxSpeed;

			float acceleration;

			if (PlayerInput.Instance.Horizontal.ReceivingInput)
				acceleration = groundAcceleration * airborneAccelProportion;
			else
				acceleration = groundDeceleration * airborneDecelProportion;

			m_MoveVector.x = Mathf.MoveTowards(m_MoveVector.x, desiredSpeed, acceleration * Time.deltaTime);
		}

		public void AirborneVerticalMovement()
		{
			if (Mathf.Approximately(m_MoveVector.y, 0f) || m_CharacterController2D.IsCeilinged && m_MoveVector.y > 0f)
			{
				m_MoveVector.y = 0f;
			}
			m_MoveVector.y -= gravity * Time.deltaTime;
		}

		public bool CheckForJumpInput()
		{
			return PlayerInput.Instance.Jump.Down;
		}

		public void EnableInvulnerability()
		{
			damageable.EnableInvulnerability();
		}

		public void DisableInvulnerability()
		{
			damageable.DisableInvulnerability();
		}


		public void TeleportToColliderBottom()
		{
			Vector2 colliderBottom = m_CharacterController2D.Rigidbody2D.position + m_Capsule.offset + Vector2.down * m_Capsule.size.y * 0.5f;
			m_CharacterController2D.Teleport(colliderBottom);
		}
			
	}
}
