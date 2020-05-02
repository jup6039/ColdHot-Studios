using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

public class enemyChase : MonoBehaviour
{
    enum chaseState { chase, wander, howl};
    private GameObject thisGameObject;
    private CharacterController thisController;
    private Transform transform;
    private Vector3 moveDir;
    private Vector3 futurePosition;
    [SerializeField] private float speed;
    [SerializeField] private float m_StickToGroundForce;
    [SerializeField] private float m_GravityMultiplier;
    private chaseState enemyState = chaseState.wander;
    private GameObject player;
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController controller;
    private Animator drakeAnimation;


    // Start is called before the first frame update
    void Awake()
    {
        thisGameObject = this.gameObject;
        transform = thisGameObject.transform;
        thisController = thisGameObject.GetComponent<CharacterController>();
        futurePosition = Vector3.zero;
        player = GameObject.FindGameObjectWithTag("Player");
        controller = player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        drakeAnimation = thisGameObject.GetComponent<Animator>();
        drakeAnimation.SetBool("CanFollow", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 chaseDirection;
        if(enemyState == chaseState.wander)
        {
            chaseDirection = Wander();
            if((player.transform.position - transform.position).magnitude < controller.detectionRadius)
            {
                enemyState = chaseState.chase;
            }
        }
        else if(enemyState == chaseState.chase)
        {
            chaseDirection = Seek(player.transform.position);
        }
        else
        {
            chaseDirection = Vector3.zero;
            drakeAnimation.SetBool("CanFollow", false);
        }

        // always move along the camera forward as it is the direction that it being aimed at
        Vector3 desiredMove = chaseDirection;

        // get a normal for the surface that is being touched to move along it
        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, thisController.radius, Vector3.down, out hitInfo,
                           thisController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        moveDir.x = desiredMove.x * speed;
        moveDir.z = desiredMove.z * speed;

        if (thisController.isGrounded)
        {
            moveDir.y = -m_StickToGroundForce;
        }

        else
        {
            moveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
        }

        Vector3 toMove = moveDir * Time.fixedDeltaTime;

        Vector3 toMovenoY = new Vector3(toMove.x, 0, toMove.z);

        RaycastHit stopMovementHit;
        if(Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), toMovenoY, out stopMovementHit, 3))
        {
            enemyState = chaseState.howl;
            toMove.x = 0;
            toMove.z = 0;
            Debug.Log(stopMovementHit.collider.tag);
        }

        thisController.Move(toMove);

        if (toMovenoY != Vector3.zero)
        {
            transform.forward = toMovenoY.normalized;
        }
    }

    //seek method follows in a line towards sought thing
    public Vector3 Seek(Vector3 targetLocation)
    {
        Vector3 desiredVelocity = (targetLocation - transform.position);
        desiredVelocity.Normalize();
  
        return desiredVelocity;
    }

    //wanders in a random direction
    public Vector3 Wander()
    {
        if (futurePosition == Vector3.zero || (futurePosition - transform.position).magnitude < 5)
        {
            //get random futurepositions
            futurePosition = new Vector3(Random.Range(0, 50), 0, Random.Range(0, 50));
        }
        //offset the point being sought by a small amount
        Vector3 wanderPosition = futurePosition;
        wanderPosition.x += Random.Range(-5f, 5f) * Time.deltaTime;
        wanderPosition.z += Random.Range(-5f, 5f) * Time.deltaTime;
        futurePosition = wanderPosition;

        //seek the future position of the vehicle
        Vector3 desiredVelocity = (wanderPosition - transform.position);
        desiredVelocity.Normalize();

        Debug.Log(futurePosition);
        Debug.Log(desiredVelocity);
        return desiredVelocity;
    }
}

