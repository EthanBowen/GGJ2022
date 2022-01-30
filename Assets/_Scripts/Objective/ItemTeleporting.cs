using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[SelectionBase]
public class ItemTeleporting : MonoBehaviour
{
    public List<Transform> locations = new List<Transform>();

    private Camera playerCamera;
    
    private Renderer objectRenderer;

    private Rigidbody rb;

    private XRGrabInteractable interactable;

    private float lastMove;
    private float nextMove;

    private void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        if (!objectRenderer)
            objectRenderer = GetComponentInChildren<Renderer>();
        
        interactable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        lastMove = Time.time;
        nextMove = 0f;
        TryMove();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastMove >= nextMove)
            TryMove();
    }

    private void OnBecameInvisible()
    {
        //TryMove();
    }

    private void TryMove()
    {
        if (locations.Count > 0 && !objectRenderer.isVisible && !interactable.isSelected)
        {
            List<Transform> validSpots = new List<Transform>();
            foreach (Transform loc in locations)
            {
                Vector3 pos = playerCamera.WorldToViewportPoint(loc.position);

                if (!((pos.x <= 1 && pos.x >= 0) && (pos.y <= 1 && pos.y >= 0) && pos.z > 0))
                {
                    validSpots.Add(loc);
                }
            }

            if (validSpots.Count > 0)
            {
                Transform newPos = validSpots[(int)Random.Range(0.0f, validSpots.Count - 0.00001f)];

                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                transform.position = newPos.position;
                transform.rotation = newPos.rotation;
            }
            lastMove = Time.time;
            nextMove = Random.Range(0f, 15f);
        }  
    }
}
