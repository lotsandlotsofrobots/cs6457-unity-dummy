using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float verticalInput;
    private float horizontalInput;

    private bool jumping;
    private bool isGrounded;
    private float rotationAngle;

    Rigidbody rigidBody;

    int coinsCollected;

    float cameraRotationAngle;

    RaycastHit hit;
    private float aimAdjustX;
    private float aimAdjustY;
    private float aimAdjustZ;
    
    Transform objectToLookAt;
    GameObject wolf;
    WolfAnimator wolfAnimator;

    [SerializeField] private Transform groundCheckTransform = null;
    [SerializeField] private LayerMask foo;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        isGrounded = true;
        jumping = false;
        coinsCollected = 0;
        aimAdjustX = 0.0f;
        aimAdjustY = 0.0f;
        aimAdjustZ = 0.0f;
        cameraRotationAngle = 0f;

        wolf = GameObject.Find("Wolf");
        if (wolf)
        {
            wolfAnimator = wolf.GetComponent<WolfAnimator>();
        }
    }

    void CalculateCameraPosition()
    {
        CheckRegionForCameraOverrides();

        // First transform direction

        Transform smoothedTransform = Camera.main.transform;

        transform.Rotate(0, horizontalInput, 0);

        float x = Mathf.Cos(cameraRotationAngle) * -3.5f;
        float z = Mathf.Sin(cameraRotationAngle) * -3.5f;

        Vector3 cameraOffset = new Vector3(x, 1.5f, z);
        Vector3 desiredPosition = (cameraOffset + transform.position);
        Vector3 smoothedPosition = Vector3.Lerp(Camera.main.transform.position, desiredPosition, 0.0125f);

        smoothedTransform.position = smoothedPosition;

        // First transform rotation
        
        Vector3 lookAtVector = objectToLookAt.position - smoothedPosition;
        
        Quaternion from = smoothedTransform.rotation;
        Quaternion to = Quaternion.LookRotation(lookAtVector, Vector3.up);

        Quaternion smoothedRotation = Quaternion.Lerp(from, to, 0.0125f/2);

        smoothedTransform.rotation = smoothedRotation;
    }


    void UpdateAnimations()
    {
        if (wolfAnimator != null)
        {
            wolfAnimator.Walking = (Mathf.Abs(verticalInput) > 0.0);
        }
    }


    void CheckInputs()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        //rotationAngle += 0.004f * horizontalInput;
        //Debug.Log("rotationAngle: " + rotationAngle);


        // jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumping = true;
            Debug.Log("Space Key was pressed down.");
        }

        // reset 
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = new Vector3(-0.01f, 2.14f, 0f);
        }

        // camera rotate
        if (Input.GetKey(KeyCode.E))
        {
            cameraRotationAngle += 0.002f;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            cameraRotationAngle -= 0.002f;
        }

        
    }

    void DoPhysicsChecks()
    {
        //if (Physics.OverlapSphere(groundCheckTransform.position, 0.1f, foo).Length == 0)
        if (Physics.OverlapBox(groundCheckTransform.position, new Vector3(0.01f, 0.01f, 0.01f)).Length == 0)
        {
            isGrounded = false;
        }
        else
        {
            isGrounded = true;
        }
    }

    void CheckRegionForCameraOverrides()
    {
        float reverseDirection = 1.0f;
        Ray ray = new Ray(transform.position, new Vector3(0, -1, 0));
        Debug.DrawRay(transform.position, Vector3.down, Color.red);

        if (Physics.Raycast(ray, out hit))
        {
            //Debug.Log("Ray hit!\n");
            if (hit.collider.tag == "LookUp")
            {
                objectToLookAt = GameObject.Find("LookatUpPosition").transform;
                //aimAdjustX = 0f;
            }
            else
            {
                objectToLookAt = transform;
                //aimAdjustX = 15.0f;
                aimAdjustX = 0.0f;
            }    
        }
        else
        {
            reverseDirection = 1f;
            // Debug.Log("No ray hit.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputs();
        UpdateAnimations();
        DoPhysicsChecks();
        CalculateCameraPosition();
    }

    private void FixedUpdate()
    {
        if (jumping && isGrounded == true)
        {
            rigidBody.AddForce(Vector3.up * 5, ForceMode.VelocityChange);
            jumping = false;
        }

        //rigidBody.velocity = new Vector3(verticalInput * (float)4, rigidBody.velocity.y, 0);

        /*
        Vector3 axis;
        float r;

        transform.rotation.ToAngleAxis(out r, out axis);
        Debug.Log("r: " + r);

        float x = Mathf.Cos(r) * (4 * verticalInput);
        float z = -1 * Mathf.Sin(r) * (4 * verticalInput);
        Debug.Log("x: " + x + ", z: " + z);

        Vector3 velocity = new Vector3(x, rigidBody.velocity.y, z);
        rigidBody.velocity = velocity;
        */

        float veritcalVelocity = rigidBody.velocity.y;
        rigidBody.velocity = transform.right * verticalInput * 4f + new Vector3(0, veritcalVelocity, 0);
        

        //rigidBody.velocity = new Vector3(verticalInput * (float)4, rigidBody.velocity.y, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        const int coinlayer = 9;
        if (other.gameObject.layer == coinlayer)
        {
            Destroy(other.gameObject);
            coinsCollected++;
        }
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
    */
}
