using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Valve.VR.InteractionSystem;

[SelectionBase]
public class ItemTeleporting : MonoBehaviour
{
    [SerializeField]
    private List<Transform> locations = new List<Transform>();

    private Camera playerCamera;
    
    private Renderer ichiRenderer;

    private Rigidbody rb;

    //private Interactable interactable;

    private float lastMove;
    private float nextMove;

    private void Awake()
    {
        ichiRenderer = GetComponent<Renderer>();
        if (!ichiRenderer)
            ichiRenderer = GetComponentInChildren<Renderer>();
        
        //interactable = GetComponent<Interactable>();
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
        if (!ichiRenderer.isVisible) //&& //!interactable.attachedToHand)
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

            Transform newPos = validSpots[(int)Random.Range(0.0f, validSpots.Count - 0.00001f)];

            /*
            List<GameObject> validSpots = new List<GameObject>();
            foreach (GameObject loc in locations)
            {
                Renderer ren = loc.GetComponent<Renderer>();

                if(!ren.isVisible)
                {
                    validSpots.Add(loc);
                }
            }

            Transform newPos = validSpots[(int)Random.Range(0.0f, validSpots.Count - 0.00001f)].transform;
            */

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.position = newPos.position;
            transform.rotation = newPos.rotation;

            lastMove = Time.time;
            nextMove = Random.Range(0f, 15f);
        }  
    }
}
