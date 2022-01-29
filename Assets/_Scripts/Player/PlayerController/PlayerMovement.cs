/* 
 * 
 * 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private InputDevice MovementController;

    private CharacterController CC;

    [SerializeField]
    private Transform HeadTransform;

    public float movementSpeed = 1.0f;

    public float gravity = -9.81f;
    public float fallingSpeed = 0f;

    public LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
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

        if(CheckIfGrounded())
        {
            fallingSpeed = 0;
        }
        else
        {
            fallingSpeed += gravity * Time.deltaTime;
        }
        
        CC.Move(headYaw * new Vector3(moveVector.x, fallingSpeed, moveVector.y) * movementSpeed * Time.deltaTime);
    }

    private void CapsuleAdjustToHeadset()
    {
        Vector3 headPos = HeadTransform.localPosition;
        CC.center = new Vector3(headPos.x, headPos.y / 2, headPos.z);
        CC.height = headPos.y;
    }

    // Uses Spherecast to determine if player is in the ground
    private bool CheckIfGrounded()
    {
        Vector3 rayStart = transform.TransformPoint(CC.center);
        float rayLength = CC.center.y + 0.01f;
        bool hasHit = Physics.SphereCast(rayStart, CC.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
        return hasHit;
    }
}
