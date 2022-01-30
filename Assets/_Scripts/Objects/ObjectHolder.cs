/*
 * Code by Ethan Bowen
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    public string[] objectTags;

    public Transform objPosition;

    [HideInInspector]
    public GameObject heldObj = null;

    [HideInInspector]
    public bool readyToHold = true;

    private Transform heldObjOriginalParent;

    

    void Start()
    {
        if (objPosition == null)
            objPosition = transform;

        if (heldObj != null)
            readyToHold = false;
    }

    public void HoldObject(Placeable other)
    {
        if (readyToHold)
        {
            readyToHold = false;
            heldObj = other.gameObject;

            heldObj.transform.position = objPosition.position;
            heldObj.transform.rotation = objPosition.rotation;

            heldObjOriginalParent = objPosition.parent;
            heldObj.transform.parent = objPosition;

            other.placed = true;
        }
    }

    // When release comes from placeable
    public void StopHolding()
    {
        if (heldObj)
        {
            heldObj.transform.parent = heldObjOriginalParent;
            heldObjOriginalParent = null;

            heldObj = null;
            readyToHold = true;
        }
    }

    public bool CanHold(string name)
    {
        foreach(string test in objectTags)
        {
            if(test.Equals(name))
                return true;
        }

        return false;
    }

    // When release comes from ObjectHolder
    public void ReleaseItem()
    {
        if (heldObj)
            heldObj.GetComponent<Placeable>().RemoveFromHolder();
    }
}
