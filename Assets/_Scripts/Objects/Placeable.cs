using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class Placeable : MonoBehaviour
{
    public string objectTag = "";

    [HideInInspector]
    public Transform holder;
    [HideInInspector]
    public ObjectHolder objectHolder;
    private ObjectHolder potentialHolder;

    [HideInInspector]
    public bool placed = false;
    [HideInInspector]
    public XRGrabInteractable interactable;

    private bool readyToBeHeld = true;

    private Transform initialParent;

    private GameObject previewObject;

    private void Awake()
    {
        readyToBeHeld = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<XRGrabInteractable>();
        initialParent = transform.parent;
    }

    // Object has found a potential holder
    private void OnTriggerEnter(Collider other)
    {

        if (readyToBeHeld)
        {
            ObjectHolder temp = other.GetComponentInParent<ObjectHolder>();
            if (temp != null && temp.readyToHold && temp.CanHold(objectTag))
                potentialHolder = temp;

            PreviewInPotentialHolder();

            // If not currently being selected by XR Interactor then attempt to place self
            if(!interactable.isSelected)
            {
                TryPlace();
            }
        }
    }

    // When object leaves its potential holder
    private void OnTriggerExit(Collider other)
    {
        if (potentialHolder != null && other.gameObject.Equals(potentialHolder.gameObject))
        {
            StopPreview();
            potentialHolder = null;
        }
    }

    public void TryPlace()
    {
        bool output = false;

        // If the Trigger is for a holder check its tag and make it hold the object
        if (potentialHolder != null && potentialHolder.CanHold(objectTag) && potentialHolder.readyToHold)
        {

            Rigidbody rig = GetComponent<Rigidbody>();
            rig.constraints = RigidbodyConstraints.FreezeAll;

            holder = potentialHolder.objPosition;
            objectHolder = potentialHolder;

            objectHolder.HoldObject(this);

            StopPreview();

            output = true;
        }

        readyToBeHeld = !output;
    }   

    // Remove from a holder if it is attached to one
    public void RemoveFromHolder()
    {
        if (placed && holder != null)
        {
            FreeHolder();

            Rigidbody rig = GetComponent<Rigidbody>();
            rig.constraints = RigidbodyConstraints.None;
            
            placed = false;
            readyToBeHeld = true;

            transform.parent = initialParent;

            objectHolder.heldObj = null;
            objectHolder = null;
            holder = null;
        }
    }

    // Frees holder to accept other placeables
    public bool FreeHolder()
    {
        if (objectHolder != null)
        {
            
            objectHolder.StopHolding();
            return true;
        }

        return false;
    }

    public void PreviewInPotentialHolder()
    {
        if (potentialHolder != null && previewObject == null)
        {
            //previewObject = new GameObject(); 
            previewObject = Instantiate(gameObject, potentialHolder.objPosition.position, potentialHolder.objPosition.rotation, potentialHolder.objPosition);

            //previewObject.AddComponent<MeshRenderer>();
            //MeshFilter mf = previewObject.AddComponent<MeshFilter>();

            //mf.mesh = gameObject.GetComponent<MeshFilter>().mesh;

            previewObject.layer = LayerMask.NameToLayer("PreviewHighlight");

            //previewObject.transform.position = potentialHolder.objPosition.position;
           // previewObject.transform.rotation = potentialHolder.objPosition.rotation;
            //previewObject.transform.parent = potentialHolder.objPosition;

        }
    }

    public void StopPreview()
    {
        if(previewObject != null)
        {
            GameObject.Destroy(previewObject);
            previewObject = null;
        }
    }
}