using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubController : MonoBehaviour
{
    [SerializeField]
    private List<Transform> locations;

    // Start is called before the first frame update
    void Start()
    {
        GameController GC = GameObject.Find("GameController").GetComponent<GameController>();

        GC.ReceneterPlayer();

        GC.CurrentObjective = null;

        List<GameObject> relics = GC.CollectedItems;
        foreach(GameObject obj in relics)
        {
            obj.GetComponent<ItemTeleporting>().locations = locations;
        }

        GC.EnableCollection();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("L1_Kitchen");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("L2_Bedroom");
    }
}
