using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> potentialRelics;

    [SerializeField]
    private List<Transform> locations;

    // Start is called before the first frame update
    void Start()
    {
        if(potentialRelics.Count > 0)
        {
            int rand = Random.Range(0, potentialRelics.Count);
            GameObject relic = potentialRelics[rand];
            ItemTeleporting IT = relic.AddComponent<ItemTeleporting>();
            IT.locations = locations;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
