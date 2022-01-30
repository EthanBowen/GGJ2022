/*
 * Code by Ethan Bowen
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<GameObject> CollectedItems;

    public GameObject CurrentObjective;

    public GameObject playerPrefab;
    private GameObject PlayerRef;

    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("GameController").Length > 1)
        {
            Destroy(gameObject);
            return;
        }
            

        CollectedItems = new List<GameObject>();
        DontDestroyOnLoad(gameObject);

        PlayerRef = Instantiate(playerPrefab);
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(GameObject.Find("XR Interaction Manager"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddRelicToCollection()
    {
        if (CurrentObjective != null)
        {
            CollectedItems.Add(CurrentObjective);
        }
    }

    public void EnableCollection()
    {
        foreach (GameObject obj in CollectedItems)
        {
            obj.SetActive(true);
        }
    }

    public void DisableCollection()
    {
        foreach (GameObject obj in CollectedItems)
        {
            obj.SetActive(false);
        }
    }

    public void ReceneterPlayer()
    {
        CharacterController CC = PlayerRef.GetComponent<CharacterController>();
        CC.enabled = false;
        PlayerRef.transform.position = Vector3.zero;
        CC.enabled = true;
    }
}
