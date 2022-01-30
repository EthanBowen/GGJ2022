/* 
 * Based on code by Valem found here: https://www.youtube.com/watch?v=5NRTT8Tbmoc
 * 
 * Edit by Ethan Bowen
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{

    private InputDevice MovementController;

    private CharacterController CC;

    [SerializeField]
    private Transform HeadTransform;

    // Layer references for player collision detection
    [Header("Layer References")]
    public LayerMask groundLayer;
    public LayerMask wallLayer;

    // Variables to ajdust Movement
    [Header("Movement Variables")]
    public float movementSpeed = 1.0f;
    public float gravity = -9.81f;
    public float fallingSpeed = 0f;

    // Used for collision handling
    Vector3 lastPosBeforeCollision = Vector3.zero;

    public XRDirectInteractor LeftHand;
    public XRDirectInteractor RightHand;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        // Create a list of viable controllers used for movement inputs
        List<InputDevice> Controllers = new List<InputDevice>();
        InputDeviceCharacteristics ControllerDeviceCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(ControllerDeviceCharacteristics, Controllers);


        // Assume the first one in the list is good enough
        if(Controllers.Count > 0)
        {
            MovementController = Controllers[0];
        }

        CC = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        CapsuleAdjustToHeadset();

        Vector2 moveVector;
        MovementController.TryGetFeatureValue(CommonUsages.primary2DAxis, out moveVector);

        Quaternion headYaw = Quaternion.Euler(0, HeadTransform.rotation.eulerAngles.y, 0);

        // Apply gravity if needed
        if(CheckIfGrounded())
        {
            fallingSpeed = 0;
        }
        else
        {
            fallingSpeed += gravity * Time.deltaTime;
        }

        // Keep players out of walls
        if(CheckIfInWall())
        {
            Vector3 adjustVector = new Vector3(CC.center.x - lastPosBeforeCollision.x, 0, CC.center.z - lastPosBeforeCollision.z);
            //print(adjustVector);
            //transform.position += adjustVector;
            CC.Move(adjustVector);
            //print("Wall collision");
        }
        else
        {
            lastPosBeforeCollision = CC.center;
        }
        
        CC.Move(headYaw * new Vector3(moveVector.x, fallingSpeed, moveVector.y) * movementSpeed * Time.deltaTime);
    }

    // Adjusts player collosion to match where their head is
    private void CapsuleAdjustToHeadset()
    {
        Vector3 headPos = HeadTransform.localPosition;

        // This if-else prevents players from clipping through the floor
        if (headPos.y > 0.1f)
        {
            CC.center = new Vector3(headPos.x, headPos.y / 2, headPos.z);
            CC.height = headPos.y;
        }
        else
        {
            CC.center = new Vector3(headPos.x, 0.1f / 2, headPos.z);
            CC.height = 0.1f;
        }
    }

    // Uses Spherecast to determine if player is in the ground
    private bool CheckIfGrounded()
    {
        Vector3 rayStart = transform.TransformPoint(CC.center);
        float rayLength = CC.center.y + 0.001f;
        bool hasHit = Physics.SphereCast(rayStart, CC.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
        return hasHit;
    }

    private bool CheckIfInWall()
    {
        Vector3 rayStart = transform.TransformPoint(CC.center + (Vector3.up * CC.center.y));
        float rayLength = CC.center.y*2;  
        bool hasHit = Physics.SphereCast(rayStart, CC.radius, Vector3.down, out RaycastHit hitInfo, rayLength, wallLayer);
        return hasHit;
    }

    private void OnDestroy()
    {
       // RightHand.se
    }

}
