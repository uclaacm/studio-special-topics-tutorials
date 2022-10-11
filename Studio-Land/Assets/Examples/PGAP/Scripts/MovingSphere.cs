using UnityEngine;


// FOR TUTORIAL
// GIVE STUDENTS VERSION WITH REGULAR GRAVITY, EVERYTHING ELSE COMPLETED
// GO THROUGH STEPS TO CONVERT TO PLANETARY GRAVITY

// 2 approaches to controlling a character
// First is the rigidbody approach, which is to have the character
// behave like a regular physics object while indirectly controlling it,
// either by applying forces or changing its velocity.
// Second is the kinematic approach, which is to have direct control
// while only querying the physics engine to perform custom collision detection.
public class MovingSphere : MonoBehaviour
{
    // controlling movement
    [SerializeField, Range(0f, 100f)]
    float maxSpeed = 10f;
    [SerializeField, Range(0f, 100f)]
    float maxAcceleration = 10f, maxAirAcceleration = 1f;
    Vector3 velocity, desiredVelocity;

    // jumps
    bool jumpDesired;
    bool onGround;
    [SerializeField, Range(0f, 10f)]
    float jumpHeight = 2f;
    [SerializeField, Range(0, 5)]
    int maxAirJumps = 0;
    int jumpNumber;

    [SerializeField, Range(0f, 90f)]
    float maxGroundAngle = 25f;

    // given max ground angle, what is the minimum dot product between up vector and normal vector
    // as angle increases, dot product should decrease
    float minGroundDotProduct;

    Vector3 contactNormal;


    //camera stuff
    [SerializeField]
    Transform playerInputSpace = default;

    //custom gravity
    Vector3 upAxis, rightAxis, forwardAxis;

    // grab rigidybody component
    Rigidbody body;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.useGravity = false;
        OnValidate();
    }

    void OnValidate()
    {
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
    }

    void Update()
    {
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f); // cap move speed for diagonal movement
        // we want to control velocity and apply acceleration to that velocity until
        // we are at the desired velocity, this is like adjusting acceleration but with
        // more control since we are targeting velocity
       if (playerInputSpace) {
			rightAxis = ProjectDirectionOnPlane(playerInputSpace.right, upAxis);
			forwardAxis =
				ProjectDirectionOnPlane(playerInputSpace.forward, upAxis);
		}
		else {
			rightAxis = ProjectDirectionOnPlane(Vector3.right, upAxis);
			forwardAxis = ProjectDirectionOnPlane(Vector3.forward, upAxis);
		}
		desiredVelocity =
			new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
		//}
        jumpDesired |= Input.GetButtonDown("Jump"); // update can be called multiple times before fixed update is
        // we don't want the jump to be forgotten if we have stopped pressing the jump button
        // |= "saves" the jump until we use it
    }
    // Nvidia PhysX engine runs with fixed time step, we want to do the same
    void FixedUpdate()
    {
        Vector3 gravity = CustomGravity.GetGravity(body.position, out upAxis);
        UpdateState();
        AdjustVelocity();

        if (jumpDesired)
        {
            jumpDesired = false;
            Jump(gravity);
        }
        velocity += gravity * Time.deltaTime;
        // update velocity
        body.velocity = velocity;
        // physics step involves invoking FixedUpdate, then PhysX, then collision methods
        // so we want to use it for the jump but set it to false afterwards
        ClearState();
    }
    void ClearState()
    {
        onGround = false;
        contactNormal = Vector3.zero;
    }
    void UpdateState()
    {
        velocity = body.velocity;
        if (onGround)
        {
            jumpNumber = 0;
            contactNormal.Normalize();
        }
        else
        {
            contactNormal = upAxis;
        }
    }
    // when jumping, it is important to focus on the jump height, not the velocity.
    // the reason for this is, it makes more sense to jump a constant height
    // which is noticeable at high speeds. e.g. if the player is falling very fast
    // adding some velocity will not create a jump, it will only slow the player down

    // we can derive this from physics
    // when an object is falling, all potential energy is converted to kinetic energy
    // 1/2mv^2 = mgh
    // 1/2v^2 = gh
    // v^2 = 2gh
    // v = sqrt(2gh)
    // this tells us, given a height difference h, what is the speed we need to reach that height difference
    // add negative because gravity is defined as negative in Unity
    void Jump(Vector3 gravity)
    {
        if (onGround || jumpNumber < maxAirJumps)
        {
            jumpNumber += 1;
            float jumpSpeed = Mathf.Sqrt(2f * gravity.magnitude * jumpHeight);
            // capping velocity when jumping, with relation to contact normal vector
            float alignedSpeed = Vector3.Dot(velocity, contactNormal);
            if (alignedSpeed > 0f)
            {
                // don't want jump to slow down your speed
                jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
            }
            velocity += contactNormal * jumpSpeed;
        }
    }

    // these are for calculating direction of normal and whether the platform is a "ground".
    void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }

    void EvaluateCollision(Collision collision) {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            float upDot = Vector3.Dot(upAxis, normal);
            if (upDot >= minGroundDotProduct)
            {
                onGround = true;
                contactNormal += normal;
            }
        }
    }

    Vector3 ProjectDirectionOnPlane(Vector3 direction, Vector3 normal)
    {
        return (direction - normal * Vector3.Dot(direction, normal)).normalized;
    }

    void AdjustVelocity()
    {
        Vector3 xAxis = ProjectDirectionOnPlane(rightAxis, contactNormal);
        Vector3 zAxis = ProjectDirectionOnPlane(forwardAxis, contactNormal);

        float currentX = Vector3.Dot(velocity, xAxis);
        float currentZ = Vector3.Dot(velocity, zAxis);

        float acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        float maxSpeedChange = acceleration * Time.deltaTime;

        //if (velocity.x < desiredVelocity.x)
        //{
        //    velocity.x = Mathf.Min(velocity.x + maxSpeedChange, desiredVelocity.x);
        //}
        //else if (velocity.x > desiredVelocity.x)
        //{
        //    velocity.x =
        //        Mathf.Max(velocity.x - maxSpeedChange, desiredVelocity.x);
        //}
        // MoveTowards does the equivalent of the above
        float newX =
            Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
        float newZ =
            Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);

        velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
    }
}
