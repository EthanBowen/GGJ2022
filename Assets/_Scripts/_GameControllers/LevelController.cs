/*
 * Code by Ethan Bowen
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> potentialRelics;
    private GameObject relic;

    [SerializeField]
    private List<Transform> locations;

    private XRGrabInteractable VictoryListener;

    [SerializeField]
    private AudioSource soundEffects; 
    [SerializeField]
    private AudioClip popperNoise;

    public bool won = false;

    [SerializeField]
    private List<GameObject> PartyTime;

    private GameController GC;

    // Start is called before the first frame update
    void Start()
    {
        //GameObject.Find("PlayerController").transform.position = new Vector3(0, 0, 0);
        GC = GameObject.Find("GameController").GetComponent<GameController>();

        GC.ReceneterPlayer();

        if (potentialRelics.Count > 0)
        {

            // Trim some items from the room for more replayability 
            int numToTrim = Mathf.FloorToInt(0.3f * potentialRelics.Count);
            for(int i = 0; i < numToTrim && i < potentialRelics.Count; i++)
            {
                int k = Random.Range(0, potentialRelics.Count);

                GameObject newLoc = new GameObject();
                GameObject TempRef = potentialRelics[k];

                newLoc.transform.position = TempRef.transform.position;
                newLoc.transform.rotation = TempRef.transform.rotation;

                locations.Add(newLoc.transform);
                potentialRelics.Remove(TempRef);

                Destroy(TempRef);
            }

            int rand = Random.Range(0, potentialRelics.Count);
            relic = potentialRelics[rand];
            ItemTeleporting IT = relic.AddComponent<ItemTeleporting>();
            IT.locations = locations;

            VictoryListener = relic.GetComponent<XRGrabInteractable>();

            GC.DisableCollection();

            GC.CurrentObjective = relic;
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
        won = true;
        DontDestroyOnLoad(relic);
        //relic.GetComponent<XRGrabInteractable>().se
        GC.AddRelicToCollection();
        relic.SetActive(false);
        soundEffects.Play();

        foreach(GameObject GO in PartyTime)
        {
            ParticleSystem LetMeTellYouALittleSomethingAboutUnityParticleSystems = GO.GetComponent<ParticleSystem>();
            LetMeTellYouALittleSomethingAboutUnityParticleSystems.Play();

            AudioSource ImVeryTiredAndJustWantToMessAroundAtThisPointPleaseDontHateMeForMyPoorCodingPractices = GO.GetComponent<AudioSource>();
            ImVeryTiredAndJustWantToMessAroundAtThisPointPleaseDontHateMeForMyPoorCodingPractices.PlayOneShot(popperNoise);
            ImVeryTiredAndJustWantToMessAroundAtThisPointPleaseDontHateMeForMyPoorCodingPractices.Play();
        }

        StartCoroutine(WaitThroughCelebration());
    }

    IEnumerator WaitThroughCelebration()
    {
        yield return new WaitForSeconds(5);

        LoadMainMenu();
    }    

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    
}
