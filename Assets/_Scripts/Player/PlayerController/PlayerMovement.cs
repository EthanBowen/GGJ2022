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
    private Transform HeadPos;

    public float movementSpeed = 10.0f;

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
        Vector3 vector = HeadPos.position;
        CC.center = new Vector3(vector.x, vector.y / 2, vector.z);


    }
}
