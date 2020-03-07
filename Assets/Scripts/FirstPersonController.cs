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
        enum moveState { walking, running, jumping, crouching, sliding, climbing}
        [SerializeField] private moveState movementState = moveState.walking;
        //[SerializeField] private bool m_IsWalking;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_ClimbSpeed;
        private bool canClimb;
        private bool hitClimbable;
        [SerializeField] private float m_RunSpeed;
        [SerializeField] private float m_slideTime;
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
        [SerializeField] private Tool activeTool;

        private Camera m_Camera;
        //private bool m_Jump;
        private float timeSlid = 0;
        private bool runJump = false;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        //private bool m_Jumping;
        private AudioSource m_AudioSource;

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
            //m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
			m_MouseLook.Init(transform , m_Camera.transform);
        }


        // Update is called once per frame
        private void Update()
        {
            if (movementState == moveState.sliding)
            {
                //tick up slide timer
                timeSlid += Time.deltaTime;
            }
            RotateView();
            // the jump state needs to read here to make sure it is not missed
            /*if (movementState != moveState.jumping)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }*/

            
            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                StartCoroutine(m_JumpBob.DoBobCycle());
                //PlayLandingSound();
                m_MoveDir.y = 0f;
                if (movementState != moveState.sliding)
                {
                    movementState = moveState.walking;
                }
            }
            if (!m_CharacterController.isGrounded && movementState != moveState.jumping && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;
        }


        private void PlayLandingSound()
        {
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }


        private void FixedUpdate()
        {
            checkClimb();

            float speed;
            GetInput(out speed);

            if (movementState == moveState.crouching || movementState == moveState.sliding)
            {
                m_CharacterController.transform.localScale = new Vector3(1.0f, 0.5f, 1.0f);
            }
            else
            {
                m_CharacterController.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }

            //if climbing do climb movement
            if(movementState == moveState.climbing)
            {
                float vertical = CrossPlatformInputManager.GetAxis("Vertical");
                if(vertical > 0)
                {
                    m_MoveDir.y = m_ClimbSpeed;
                }
                else if (vertical < 0)
                {
                        m_MoveDir.y = -m_ClimbSpeed;
                }
                else
                {
                    m_MoveDir.y = 0;
                }
            }

            //else perform normal movement
            else
            {  
                // always move along the camera forward as it is the direction that it being aimed at
                Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

                // get a normal for the surface that is being touched to move along it
                RaycastHit hitInfo;
                Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                                   m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
                desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

                m_MoveDir.x = desiredMove.x * speed;
                m_MoveDir.z = desiredMove.z * speed;

                if (m_CharacterController.isGrounded)
                {
                    m_MoveDir.y = -m_StickToGroundForce;

                    if (movementState == moveState.jumping)
                    {
                        m_MoveDir.y = m_JumpSpeed;
                        PlayJumpSound();
                    }
                }
                else
                {
                    m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
                }
            }             

            m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);

            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);

            m_MouseLook.UpdateCursorLock();

            //if tool active use tool
            if(Input.GetMouseButton(0))
            {
                activeTool.UsedThisUpdate = true;
                activeTool.Use();
            }
            else
            {
                activeTool.UsedThisUpdate = false;
            }
        }


        private void PlayJumpSound()
        {
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }


        private void ProgressStepCycle(float speed)
        {
            if (movementState != moveState.sliding || movementState != moveState.climbing)
            {
                if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
                {
                    m_StepCycle += (m_CharacterController.velocity.magnitude + (speed * (movementState == moveState.walking ? 1f : m_RunstepLenghten))) *
                                 Time.fixedDeltaTime;
                }

                if (!(m_StepCycle > m_NextStep))
                {
                    return;
                }

                m_NextStep = m_StepCycle + m_StepInterval;

                PlayFootStepAudio();
            }
        }


        private void PlayFootStepAudio()
        {
            if (!m_CharacterController.isGrounded || movementState == moveState.sliding)
            {
                return;
            }
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            if(movementState == moveState.crouching)
            {
                m_AudioSource.volume = 0.5f;
            }
            else
            {
                m_AudioSource.volume = 1;
            }
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
                                      (speed*(movementState == moveState.walking ? 1f : m_RunstepLenghten)));
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
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");


            // keep track of characters movement state
            moveState waswalking = movementState;

            //if crouching cannot stand if something above head
            if (waswalking == moveState.crouching)
            {
                if (canStand())
                {
                    if (CrossPlatformInputManager.GetButtonDown("Jump"))
                    {
                        Debug.Log("jump");
                        movementState = moveState.jumping;
                        runJump = false;
                    }
                    else if(!Input.GetKey(KeyCode.LeftControl))
                    {
                        Debug.Log("walk");
                        movementState = moveState.walking;
                    }
                    else
                    {
                        Debug.Log("standing crouch");
                        movementState = moveState.crouching;
                    }
                }
                else
                {
                    Debug.Log("crouching crouch");
                    movementState = moveState.crouching;
                }
                
            }
            //if character is not sliding or jumping alter state based on key press
            else if (waswalking != moveState.sliding && waswalking != moveState.jumping && waswalking != moveState.climbing)
            {
                //if holding down left shift switch to running, if left shift and other keys are held, prioritize other motion
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if (movementState != moveState.crouching)
                    {
                        movementState = moveState.running;
                    }
                }

                //if holding down left control switch to slide or crouch if walking or running
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    if (waswalking == moveState.running)
                    {
                        timeSlid = 0;
                        movementState = moveState.sliding;
                    }
                    else if (waswalking == moveState.walking)
                    {
                        movementState = moveState.crouching;
                    }
                }

                //if movement is not held, this resets to walking when not moving but allows you to keep running without holding down shift
                if ((horizontal == 0 && vertical == 0) || (!Input.GetKey(KeyCode.LeftControl) && movementState != moveState.running))
                {
                    movementState = moveState.walking;
                }

                //if can climb and vertical motion then climb
                if(canClimb)
                {
                    if (vertical > 0 || (vertical < 0 && !m_CharacterController.isGrounded))
                    {
                        movementState = moveState.climbing;
                    }
                }
                //if press jump button jump
                if (CrossPlatformInputManager.GetButtonDown("Jump"))
                {
                    movementState = moveState.jumping;
                    //check if running jump
                    if (waswalking == moveState.running)
                    {
                        runJump = true;
                    }
                    else
                    {
                        runJump = false;
                    }
                }
            }
            //if sliding case
            else if (waswalking != moveState.jumping && waswalking != moveState.climbing)
            {
                //if jump cancel slide to go to jump
                if (CrossPlatformInputManager.GetButtonDown("Jump"))
                {
                    timeSlid = 0;
                    movementState = moveState.jumping;
                    runJump = true;
                }

                //if time complete stop sliding
                else if (timeSlid >= m_slideTime)
                {
                    timeSlid = 0;


                    //if holding control when complete change to crouch
                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        movementState = moveState.crouching;
                    }
                    else
                    {
                        movementState = moveState.walking;
                    }

                    //if cannot stand defaults to crouch
                    if(!canStand())
                    {
                        movementState = moveState.crouching;
                    }
                }
            }
            //if climbing exit climb on jump or backwards movement on ground
            else if (waswalking != moveState.jumping)
            {
                if (CrossPlatformInputManager.GetButtonDown("Jump"))
                {
                    movementState = moveState.jumping;
                    runJump = false;
                }
                else if (m_CharacterController.isGrounded && vertical < 0)
                {
                    movementState = moveState.walking;
                }
                else if (!canClimb)
                {
                    movementState = moveState.walking;
                }
                
            }
            //jump case, only switch if move to climb
            else
            {
                if (canClimb)
                {
                    if (vertical != 0)
                    {
                        movementState = moveState.climbing;
                    }
                }
            }

            // set the desired speed to be walking or running
            speed = m_WalkSpeed;
            switch (movementState)
            {
                case moveState.walking:
                    speed = m_WalkSpeed;
                    break;
                case moveState.running:
                    speed = m_RunSpeed;
                    break;
                case moveState.crouching:
                    speed = m_WalkSpeed / 2.0f;
                    break;
                case moveState.jumping:
                    //decide speed based on runjump
                    if(runJump)
                    {
                        speed = m_RunSpeed * 0.8f;
                    }
                    else
                    {
                        speed = m_WalkSpeed * 0.8f;
                    }
                    break;
                case moveState.climbing:
                    speed = 0;
                    break;
                case moveState.sliding:
                    speed = m_RunSpeed * 2.0f;
                    break;
            }

            if (movementState == moveState.sliding)
            {
                m_Input = new Vector2(0, 1);
            }
            else
            {
                m_Input = new Vector2(horizontal, vertical);
            }

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (movementState != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(movementState != moveState.walking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }
        }


        private void RotateView()
        {
            m_MouseLook.LookRotation (transform, m_Camera.transform);
        }

        //sets canclimb to true if collided with a climbable object this fixedupdate
        private void checkClimb()
        {
            canClimb = false;
            if(hitClimbable)
            {
                canClimb = true;
                hitClimbable = false;
            }

        }
        
        //checks to see if there is something above the players crouched state
        private bool canStand()
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, 1.5f))
            {
                Debug.Log("can not stand");
                return false;
            }
            Debug.Log("can stand");
            return true;
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {


            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            //if climbable set climb to true
            if (hit.gameObject.tag == "climbable" && m_CollisionFlags == CollisionFlags.Sides)
            {
                hitClimbable = true;

                //exit slide on hits
                if (movementState == moveState.sliding)
                {
                    movementState = moveState.walking;
                }
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);

            
        }
    }
}
