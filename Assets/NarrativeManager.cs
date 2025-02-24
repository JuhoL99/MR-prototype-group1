using Convai.Scripts.Runtime.Core;
using System.Collections;
using UnityEngine;

public class NarrativeManager : MonoBehaviour
{
    [SerializeField] private ConvaiNPC npc;
    private FishingSystem fishingSystem;
    [Header("Visible here just for testing")]
    [SerializeField] private bool startedSequence = false;
    [SerializeField] private bool introCompleted = false;
    [SerializeField] private bool rodGrabbed = false;
    [SerializeField] private bool baitGrabbed = false;
    [SerializeField] private bool fishCaught = false;
    private const string INTRO_EVENT = "Intro trigger";
    private const string ROD_EVENT = "Rod trigger";
    private const string BAIT_EVENT = "Bait trigger";
    private const string FISH_EVENT = "Fish trigger";

    private void Start()
    {
        fishingSystem = FishingSystem.instance;
        //StartCoroutine(StartIntroSequence());
    }
    private IEnumerator StartIntroSequence()
    {
        yield return new WaitForSeconds(5f);
        TriggerIntro();
    }
    private void Update()
    {
        if(OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.LTouch) && !startedSequence) //change to proximity later
        {
            startedSequence = true;
            TriggerIntro();
            return;
        }
    }
    public void TriggerIntro()
    {
        if (!introCompleted)
        {
            if(npc != null) npc.TriggerEvent(INTRO_EVENT);
        }
    }
    public void OnIntroComplete()
    {
        introCompleted = true;
    }
    public void OnRodGrabbed()
    {
        if (introCompleted && !rodGrabbed)
        {
            rodGrabbed = true;
            if (npc != null) npc.TriggerEvent(ROD_EVENT);
        }
    }
    public void OnBaitSelected()
    {
        if (rodGrabbed && !baitGrabbed)
        {
            baitGrabbed = true;
            if (npc != null) npc.TriggerEvent(BAIT_EVENT);
        }
    }
    public void OnFishCaught()
    {
        if (baitGrabbed && !fishCaught)
        {
            fishCaught = true;
            if (npc != null) npc.TriggerEvent(FISH_EVENT);
        }
    }
}
