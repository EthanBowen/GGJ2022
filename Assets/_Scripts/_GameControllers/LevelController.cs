using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

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

    // Start is called before the first frame update
    void Start()
    {
        if(potentialRelics.Count > 0)
        {

            int rand = Random.Range(0, potentialRelics.Count);
            relic = potentialRelics[rand];
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
        won = true;
        DontDestroyOnLoad(relic);
        soundEffects.Play();

        foreach(GameObject GO in PartyTime)
        {
            ParticleSystem LetMeTellYouALittleSomethingAboutUnityParticleSystems = GO.GetComponent<ParticleSystem>();
            LetMeTellYouALittleSomethingAboutUnityParticleSystems.Play();

            AudioSource ImVeryTiredAndJustWantToMessAroundAtThisPointPleaseDontHateMeForMyPoorCodingPractices = GO.GetComponent<AudioSource>();
            ImVeryTiredAndJustWantToMessAroundAtThisPointPleaseDontHateMeForMyPoorCodingPractices.PlayOneShot(popperNoise);
            ImVeryTiredAndJustWantToMessAroundAtThisPointPleaseDontHateMeForMyPoorCodingPractices.Play();
        }
    }

    
}
