using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (CharacterController))]
    [RequireComponent(typeof (AudioSource))]
    public class FirstPersonController : MonoBehaviour
    {
		public bool canClimb;
        [SerializeField] private bool m_IsWalking;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RunSpeed;
		[SerializeField] private float m_ClimbSpeed;
        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
        [SerializeField] private float m_JumpSpeed;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;
        [SerializeField] private MouseLook m_MouseLook;
        [SerializeField] private bool m_UseFovKick;
        [SerializeField] private FOVKick m_FovKick = new FOVKick();
        [SerializeField] private bool m_UseHeadBob;
        [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
        [SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
        [SerializeField] private float m_StepInterval;
        [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.

        private Camera m_Camera;
        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        private AudioSource m_AudioSource;
        private Animator anim;

        private float horizontal;
        private float vertical;
        private bool walking;
        private bool running;
        private bool strafingL;
        private bool strafingR;
        private bool idle;

        // Use this for initialization
        private void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_FovKick.Setup(m_Camera);
            m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle/2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
			m_MouseLook.Init(transform , m_Camera.transform);
            anim = GetComponent<Animator>();

            walking = false;
            running = false;
            strafingL = false;
            strafingR = false;
            idle = true;
            horizontal = 0f;
            vertical = 0f;
        }


        // Update is called once per frame
        private void Update()
        {
			m_Camera = Camera.main;
            RotateView();
            // the jump state needs to read here to make sure it is not missed
            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
                

            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                StartCoroutine(m_JumpBob.DoBobCycle());
                PlayLandingSound();
                m_MoveDir.y = 0f;
                m_Jumping = false;
                if (walking)
                    anim.SetBool("walking", true);
                else if (running)
                    anim.SetBool("running", true);
                else if (strafingL)
                    anim.SetBool("isStrafingL", true);
                else if (strafingR)
                    anim.SetBool("isStrafingR", true);
                else
                    anim.SetBool("idle", true);
            }
            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;

            if ((Math.Abs(horizontal) - Math.Abs(vertical) > 0) && horizontal < 0 && !strafingL)
            {    
                anim.SetBool("isStrafingR", false);
                anim.SetBool("walking", false);
                anim.SetBool("running", false);
                anim.SetBool("idle", false);
                anim.SetBool("isStrafingL", true);
                strafingR = false;
                walking = false;
                running = false;
                idle = false;
                strafingL = true;
            }
            else if ((Math.Abs(horizontal) - Math.Abs(vertical) > 0) && horizontal > 0 && !strafingR)
            {
                anim.SetBool("isStrafingL", false);
                anim.SetBool("walking", false);
                anim.SetBool("running", false);
                anim.SetBool("idle", false);
                anim.SetBool("isStrafingR", true);
                strafingL = false;
                walking = false;
                running = false;
                idle = false;
                strafingR = true;
            }
            else if (vertical != 0 && Input.GetKey(KeyCode.LeftShift) & !running)
            {
                anim.SetBool("isStrafingL", false);
                anim.SetBool("isStrafingR", false);
                anim.SetBool("walking", false);
                anim.SetBool("idle", false);
                anim.SetBool("running", true);
                strafingL = false;
                strafingR = false;
                walking = false;
                idle = false;
                running = true;
            }
            else if (vertical != 0 && !walking && !Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetBool("isStrafingL", false);
                anim.SetBool("isStrafingR", false);
                anim.SetBool("running", false);
                anim.SetBool("idle", false);
                anim.SetBool("walking", true);
                strafingL = false;
                strafingR = false;
                running = false;
                idle = false;
                walking = true;
            }
            else if (vertical == 0 && horizontal == 0 && !idle)
            {
                anim.SetBool("isStrafingL", false);
                anim.SetBool("isStrafingR", false);
                anim.SetBool("walking", false);
                anim.SetBool("running", false);
                anim.SetBool("idle", true);
                strafingL = false;
                strafingR = false;
                walking = false;
                running = false;
                idle = true;
            }
            else
            {
                anim.SetBool("isStrafingL", false);
                anim.SetBool("isStrafingR", false);
                anim.SetBool("walking", false);
                anim.SetBool("running", false);
                anim.SetBool("idle", false);
            }
        }


        private void PlayLandingSound()
        {
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }

		private void OnTriggerEnter (Collider col)
		{
			Debug.Log("Colliding");
			//bool Bean = col.gameObject.name == ("beanstalk");
			//col.gameObject.GetComponent ("Species")

			//var climb = plant.GetComponent ("PlantableObject");
			//Debug.Log ("Tada");
			if(col.gameObject.name == "beanstalk(Clone)")
			{
				Debug.Log("Climbing");
				canClimb = true;
				//Debug.Log(m_MoveDir);
			}
		}

		void OnTriggerExit (Collider col)
		{
			if(col.gameObject.tag == "Ladder")
			{
				canClimb = false;
				Debug.Log ("Disconnect");
			}
		}

        private void FixedUpdate()
        {
            float speed;
            GetInput(out speed);
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward*m_Input.y + transform.right*m_Input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height/2f);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x*speed;
            m_MoveDir.z = desiredMove.z*speed;
			if (canClimb) {

				//Debug.Log ("x dir =");
				//Debug.Log (m_MoveDir.x);

				m_MoveDir.y = m_MoveDir.magnitude - Math.Abs (m_MoveDir.y);
				//Debug.Log ("z dir =");
				//Debug.Log (m_MoveDir.z);

				if (Input.GetKeyDown (KeyCode.Space)) {
					canClimb = false;
					}
				}
				//Debug.Log ("y dir =");
				//Debug.Log (m_MoveDir.y);
		//	} else {
		//		m_StickToGroundForce;

		//	}


            if (m_CharacterController.isGrounded)
            {
				if (canClimb){
					//Debug.Log ("Shouldn't stick");
				}else {
					m_MoveDir.y = -m_StickToGroundForce;
					//Debug.Log("Should stick");
				}
                if (m_Jump)
                {
                    m_MoveDir.y = m_JumpSpeed;
                    PlayJumpSound();
                    m_Jump = false;
                    m_Jumping = true;
                    anim.SetTrigger("jump");
                }
            }
            else
            {
                m_MoveDir += Physics.gravity*m_GravityMultiplier*Time.fixedDeltaTime;
            }
            m_CollisionFlags = m_CharacterController.Move(m_MoveDir*Time.fixedDeltaTime);

            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);
        }


        private void PlayJumpSound()
        {
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }


        private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed*(m_IsWalking ? 1f : m_RunstepLenghten)))*
                             Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }


        private void PlayFootStepAudio()
        {
            if (!m_CharacterController.isGrounded)
            {
                return;
            }
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }


        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                      (speed*(m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }
            m_Camera.transform.localPosition = newCameraPosition;
        }


        private void GetInput(out float speed)
        {
            // Read input
            horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            vertical = CrossPlatformInputManager.GetAxis("Vertical");

            bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
#endif
            // set the desired speed to be walking or running
            speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }
        }


        private void RotateView()
        {
            m_MouseLook.LookRotation (transform, m_Camera.transform);
        }


        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
        }
    }
}
