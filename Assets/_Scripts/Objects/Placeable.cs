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

    // Update is called once per frame
    //void Update()
    //{
    //    CheckGrab();
    //}

    private void OnTriggerEnter(Collider other)
    {

        if (readyToBeHeld)
        {
            ObjectHolder temp = other.GetComponentInParent<ObjectHolder>();
            if (temp != null && temp.readyToHold && temp.CanHold(objectTag))
                potentialHolder = temp;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.Equals(potentialHolder.gameObject))
        {
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

            output = true;
        }

        readyToBeHeld = !output;
    }

    public bool FreeHolder(GameObject other)
    {
        ObjectHolder temp = other.GetComponentInParent<ObjectHolder>();
        // Collision is a holder that isn't holding an object
        if (temp != null)
        {
            if (temp.heldObj == null && !temp.readyToHold)
            {
                temp.StopHolding();
                return true;
            }
        }

        return false;
    }

    // Remove from a holder if it is attached to one
    public void Grabbed()
    {
        if (placed && holder != null)// && interactable.attachedToHand)
        {
            Rigidbody rig = GetComponent<Rigidbody>();
            rig.constraints = RigidbodyConstraints.None;
            
            placed = false;
            transform.parent = initialParent;
            objectHolder.heldObj = null;
            holder = null;
        }
    }

    // Called from ObjectHolder
    public void AttemptRemoveFromHolder()
    {
        Rigidbody rig = GetComponent<Rigidbody>();
        rig.constraints = RigidbodyConstraints.None;

        placed = false;
        transform.parent = initialParent;
        objectHolder.heldObj = null;

        holder = null;
    }
}