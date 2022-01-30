using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> potentialRelics;

    [SerializeField]
    private List<Transform> locations;

    private XRGrabInteractable VictoryListener;

    bool won = false;

    // Start is called before the first frame update
    void Start()
    {
        if(potentialRelics.Count > 0)
        {
            int rand = Random.Range(0, potentialRelics.Count);
            GameObject relic = potentialRelics[rand];
            ItemTeleporting IT = relic.AddComponent<ItemTeleporting>();
            IT.locations = locations;

            VictoryListener = relic.GetComponent<XRGrabInteractable>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!won && VictoryListener.isSelected)
        {
            Victory();
        }
    }

    public void Victory()
    {

    }
}
