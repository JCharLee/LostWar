using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
// MoveBehaviour inherits from GenericBehaviour. This class corresponds to basic walk and run behaviour, it is the default behaviour.
public class MoveBehaviour : GenericBehaviour
{
	public float walkSpeed = 0.15f;                 // Default walk speed.
	public float runSpeed = 0.8f;                   // Default run speed.
	public float sprintSpeed = 1.5f;                // Default sprint speed.
	public float speedDampTime = 0.1f;              // Default damp time to change the animations based on current speed.       // Default horizontal inertial force when jumping.

	private float aimWalkSpeed = 2f;
	private float speed, speedSeeker;               // Moving speed.                           // Animator variable related to jumping.
	public string jumpButton = "Jump";
	public float jumpHeight = 1.5f;
	private bool jump;
	public float jumpIntertialForce = 10f;
	public Animator ani;
	private bool haveMotion = false;
	private bool rolling = false;
	
	public enum UsingWeapon {none, short_dist, long_dist};
	public UsingWeapon usingWeapon;
	public GameObject pistol;
	public GameObject sword;
	public GameObject axe;
	public GameObject revolver;
	public GameObject cur_short_weapon = null;
	public GameObject cur_long_weapon = null;

	[SerializeField]private RayShoot rayShoot;
	//public GameObject curweapon;


	// Start is always called after any Awake functions.

	public AudioSource playeraudio;
	private AudioClip[] footsounds;
	private AudioClip meleeattack;
	private AudioClip reloadsfx;
	private Health health;

	void Start()
	{
		behaviourManager.SubscribeBehaviour(this);
		behaviourManager.RegisterDefaultBehaviour(this.behaviourCode);
		rayShoot = GetComponent<RayShoot>();
		speedSeeker = runSpeed;
		usingWeapon = UsingWeapon.none;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		playeraudio = GetComponent<AudioSource>();
		footsounds = Resources.LoadAll<AudioClip>("FootSounds");
		meleeattack = Resources.Load<AudioClip>("Sound/Swing1-Free-1");
		reloadsfx = Resources.Load<AudioClip>("Sound/Pistol_ClipIn_05");

		health = GetComponent<Health>();
	}

	// Update is used to set features regardless the active behaviour.
	void Update()
	{
		if (health.isdie)
        {
			AimBehaviourBasic.aim = false;
			return;
		}

		if (UIManager.instance.isAction || QuestManager.instance.IsStarting)
			return;

		if (!jump && Input.GetButtonDown(jumpButton) && behaviourManager.IsCurrentBehaviour(this.behaviourCode) && !behaviourManager.IsOverriding())
		{
			jump = true;
		}

        if (AimBehaviourBasic.aim)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.forward * Time.deltaTime * aimWalkSpeed);
            }

            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.forward * -1 * Time.deltaTime * aimWalkSpeed);
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.right * -1 * Time.deltaTime * aimWalkSpeed);
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * Time.deltaTime * aimWalkSpeed);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && !haveMotion)//원거리 무기
		{
			if (cur_long_weapon == null)
            {
				StartCoroutine(UIManager.instance.NoticeText(false, "무기를 먼저 장착해야 합니다."));
				return;
            }
			UIManager.instance.weaponTxt.text = "WEAPON\n1";
			if (usingWeapon == UsingWeapon.short_dist)//근거리 무기를 들고 있었다면
				cur_short_weapon.SetActive(false);//비활성화 한다.
			StartCoroutine(On(cur_long_weapon));
			usingWeapon = UsingWeapon.long_dist;
			ani.SetBool("HaveSword", false);
			ani.SetBool("HavePistol", true);
			rayShoot.enabled = true;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2) && !haveMotion)//근접 무기
		{
			if (cur_short_weapon == null)
            {
				StartCoroutine(UIManager.instance.NoticeText(false, "무기를 먼저 장착해야 합니다."));
				return;
			}
			UIManager.instance.weaponTxt.text = "WEAPON\n2";
			if (usingWeapon == UsingWeapon.long_dist)//원거리 무기를 들고 있었다면
				cur_long_weapon.SetActive(false);//비활성화 한다.
			StartCoroutine(On(cur_short_weapon));//근거리무기를 활성화
			usingWeapon = UsingWeapon.short_dist;
			ani.SetBool("HavePistol", false);
			ani.SetBool("HaveSword", true);
			rayShoot.enabled = false;
		}
		
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			UIManager.instance.weaponTxt.text = "NO\nWEAPON\n(1or2)";
			if (cur_short_weapon!=null)
			cur_short_weapon.SetActive(false);
			if(cur_long_weapon!=null)
			cur_long_weapon.SetActive(false);
			usingWeapon = UsingWeapon.none;
			ani.SetBool("HaveSword", false);
			ani.SetBool("HavePistol", false);
			rayShoot.enabled = false;
		}

		if (Input.GetKeyDown(KeyCode.Space)&&!rolling)
		{
			if (!AimBehaviourBasic.aim)
			{
				ani.SetTrigger("Roll");
				StartCoroutine(Roll());
			}
		}

		if (Input.GetKeyDown(KeyCode.R) && usingWeapon == UsingWeapon.long_dist)
		{
			if(!haveMotion)
			{
				ani.SetTrigger("Reload");
				StartCoroutine(Reload());
			}
		}
		
	}

	// LocalFixedUpdate overrides the virtual function of the base class.
	public override void LocalFixedUpdate()
	{
		if (UIManager.instance.isAction || QuestManager.instance.IsStarting)
			return;
		MovementManagement(behaviourManager.GetH, behaviourManager.GetV);
	}
	

	// Execute the idle and walk/run jump movements.


	// Deal with the basic player movement
	void MovementManagement(float horizontal, float vertical)
	{
		if (UIManager.instance.isAction || QuestManager.instance.IsStarting)
			return;
		// On ground, obey gravity.

		// Avoid takeoff when reached a slope end.


		// Call function that deals with player orientation.
		Rotating(horizontal, vertical);

		// Set proper speed.
		if (rolling)
		{
			speed = 3f;
			transform.Translate(Vector3.forward * speed * Time.deltaTime * 3);
			behaviourManager.GetAnim.SetFloat(speedFloat, speed, speedDampTime, Time.deltaTime);
			return;
		}
		Vector2 dir = new Vector2(horizontal, vertical);
		speed = Vector2.ClampMagnitude(dir, 1f).magnitude;
		// This is for PC only, gamepads control speed via analog stick.
		speedSeeker += Input.GetAxis("Mouse ScrollWheel");
		speedSeeker = Mathf.Clamp(speedSeeker, walkSpeed, runSpeed);
		speed *= speedSeeker;
		if (behaviourManager.IsSprinting())
		{
			speed = sprintSpeed;
		}
		transform.Translate(Vector3.forward * speed * 2.5f * Time.deltaTime*3);

		behaviourManager.GetAnim.SetFloat(speedFloat, speed, speedDampTime, Time.deltaTime);

		if(speed > 0 && !playeraudio.isPlaying)
        {
			playeraudio.clip = footsounds[Random.Range(0, footsounds.Length)];
			playeraudio.Play();
        }
	}


	

	// Rotate the player to match correct orientation, according to camera and key pressed.
	Vector3 Rotating(float horizontal, float vertical)
	{
		// Get camera forward direction, without vertical component.
		Vector3 forward = behaviourManager.playerCamera.TransformDirection(Vector3.forward);

		// Player is moving on ground, Y component of camera facing is not relevant.
		forward.y = 0.0f;
		forward = forward.normalized;

		// Calculate target direction based on camera forward and direction key.
		Vector3 right = new Vector3(forward.z, 0, -forward.x);
		Vector3 targetDirection;
		targetDirection = forward * vertical + right * horizontal;

		// Lerp current direction to calculated target direction.
		if ((behaviourManager.IsMoving() && targetDirection != Vector3.zero))
		{
			Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

			Quaternion newRotation = Quaternion.Slerp(behaviourManager.GetRigidBody.rotation, targetRotation, behaviourManager.turnSmoothing);
			behaviourManager.GetRigidBody.MoveRotation(newRotation);
			behaviourManager.SetLastDirection(targetDirection);
		}
		// If idle, Ignore current camera facing and consider last moving direction.
		if (!(Mathf.Abs(horizontal) > 0.9 || Mathf.Abs(vertical) > 0.9))
		{
			behaviourManager.Repositioning();
		}

		return targetDirection;
	}

	// Collision detection.
	private void OnCollisionStay(Collision collision)
	{
		// Slide on vertical obstacles
		if (behaviourManager.IsCurrentBehaviour(this.GetBehaviourCode()) && collision.GetContact(0).normal.y <= 0.1f)
		{
			GetComponent<CapsuleCollider>().material.dynamicFriction = 0f;
			GetComponent<CapsuleCollider>().material.staticFriction = 0f;
		}
	}
	private void OnCollisionExit(Collision collision)
	{
		GetComponent<CapsuleCollider>().material.dynamicFriction = 0.6f;
		GetComponent<CapsuleCollider>().material.staticFriction = 0.6f;
	}
	
public void FixedUpdate()
	{
		if (health.isdie)
		{
			AimBehaviourBasic.aim = false;
			return;
		}

		if (UIManager.instance.isAction || QuestManager.instance.IsStarting)
			return;
		if (EventSystem.current.IsPointerOverGameObject()) return;
		if (Input.GetMouseButton(0)&&!Input.GetKey(KeyCode.LeftShift))
        {
			if (!haveMotion)
			{
				if (usingWeapon == UsingWeapon.short_dist)
				{
					QuickRotating();
					StartCoroutine(Attack());
				}
			}
		}
		
	}

	void QuickRotating()
	{
		if (UIManager.instance.isAction || QuestManager.instance.IsStarting)
			return;
		Vector3 forward = behaviourManager.playerCamera.TransformDirection(Vector3.forward);
		// Player is moving on ground, Y component of camera facing is not relevant.
		forward.y = 0.0f;
		forward = forward.normalized;

		// Always rotates the player according to the camera horizontal rotation in aim mode.
		Quaternion targetRotation =  Quaternion.Euler(0, behaviourManager.GetCamScript.GetH, 0);

		// Rotate entire player to face camera.
		behaviourManager.SetLastDirection(forward);
		transform.rotation = targetRotation;

	}
	IEnumerator Attack()
    {
		ani.SetTrigger("Attack");
		haveMotion = true;
		sword.GetComponent<SwordAttack>().canAttack = true;
		axe.GetComponent<SwordAttack>().canAttack = true;
		playeraudio.clip = meleeattack;
		playeraudio.PlayDelayed(0.3f);
        yield return new WaitForSeconds(1.3f);
		sword.GetComponent<SwordAttack>().canAttack = false;
		axe.GetComponent<SwordAttack>().canAttack = false;
		haveMotion = false;
    }
	IEnumerator Reload()
	{
		haveMotion = true;
		playeraudio.clip = reloadsfx;
		playeraudio.PlayDelayed(1.8f);
		yield return new WaitForSeconds(2.5f);
		haveMotion = false;
	}
	IEnumerator On(GameObject weapon)
	{
		yield return new WaitForSeconds(0.2f);
		Debug.Log("debug");
		if (weapon != null)
			weapon.SetActive(true);
		haveMotion = true;
			yield return new WaitForSeconds(1f);
		haveMotion = false;

	}
	IEnumerator Roll()
	{
		rolling = haveMotion = true;
		speed = 2;
		yield return new WaitForSeconds(0.3f);
		speed = 1;
		yield return new WaitForSeconds(0.3f);
		speed = 0;
		rolling = haveMotion = false;

	}
	public void ChangeItemObject(string name)
	{
		switch (name)
		{
			case "Axe":
				if (usingWeapon == UsingWeapon.short_dist)
				{
					cur_short_weapon.SetActive(false);
					axe.SetActive(true);
				}
				cur_short_weapon = axe;
				break;
			case "Sword":
				if (usingWeapon == UsingWeapon.short_dist)
				{
					cur_short_weapon.SetActive(false);
					sword.SetActive(true);
				}
				cur_short_weapon = sword;
				break;
			case "Pistol":
				if (usingWeapon == UsingWeapon.long_dist)
				{
					cur_long_weapon.SetActive(false);
					pistol.SetActive(true);
				}
				cur_long_weapon = pistol;
				break;
			case "Revolver":
				if (usingWeapon == UsingWeapon.long_dist)
				{
					cur_long_weapon.SetActive(false);
					revolver.SetActive(true);
				}
				cur_long_weapon = revolver;
				break;
		}
	}

}