using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Unity.Mathematics;
using System;
using Random = UnityEngine.Random;

public class FishingSystem : MonoBehaviour
{
    public static FishingSystem instance;

    public UnityEvent onAttachedToWater = new UnityEvent();
    public UnityEvent onDetachedFromWater = new UnityEvent();
    public UnityEvent<FishSpecies> onFishBite = new UnityEvent<FishSpecies>();
    public UnityEvent<FishSpecies> onFishCaught = new UnityEvent<FishSpecies>();
    public UnityEvent<FishManager> onFishGrabbed = new UnityEvent<FishManager>();
    public UnityEvent onFishReleased = new UnityEvent();

    [SerializeField] private float waterLevel = 0f;
    public float WaterLevel => waterLevel;
    [SerializeField] private FishSpecies[] availableFish;
    public bool RodHoldingFish => rodHoldingFish;
    private bool rodHoldingFish;

    private BaitType currentBait;

    //
    [SerializeField] private float minBiteTime = 3f;
    [SerializeField] private float maxBiteTime = 13f;
    [SerializeField] private float catchChange = 0.5f;
    [SerializeField] private FishLine fishLine; //temporary
    private Transform hook; //temporary

    private GameManager gameManager;
    private bool isHookInWater = false;
    private bool isFishingActive = false;
    private bool isMinigameActive = false;
    private float nextFishCheck;

    private void Awake()
    {
        if(instance  == null) instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        currentBait = BaitType.Worm;
        gameManager = GetComponent<GameManager>();
        ResetFishingTimer();
        onAttachedToWater.AddListener(OnHookInWater);
        onDetachedFromWater.AddListener(OnHookOutOfWater);
        StartCoroutine(DelayedStart());
    }
    private void Update()
    {
        if(!isHookInWater || !isFishingActive) return;
        if (!isMinigameActive) CheckForFish();
        else UpdateMinigame();
    }
    public void AttachEvent(bool attached)
    {
        if (attached) onAttachedToWater?.Invoke();
        else onDetachedFromWater?.Invoke();
    }
    private void OnHookInWater()
    {
        isHookInWater = true;
        isFishingActive = true;
        ResetFishingTimer();
    }
    public void FishCaughtEvent()
    {
        rodHoldingFish = true;
    }
    public void FishGrabbedEvent(FishManager fish)
    {
        rodHoldingFish = false;
        onFishGrabbed?.Invoke(fish);
    }
    public void FishDroppedEvent()
    {
        onFishReleased?.Invoke();
    }
    private void OnHookOutOfWater()
    {
        isHookInWater = false;
        isFishingActive = false;
        isMinigameActive = false;
    }
    private FishSpecies SelectFishSpecies()
    {
        List<FishSpecies> selectedFish = new List<FishSpecies>();
        foreach(FishSpecies species in availableFish)
        {
            if(species.preferredBait.Contains(currentBait))
            {
                selectedFish.Add(species);
            }
        }
        if (selectedFish.Count == 0)
        {
            selectedFish.AddRange(availableFish);
        }
        return selectedFish[Random.Range(0,selectedFish.Count-1)];
    }
    private void CheckForFish()
    {
        if(Time.time >= nextFishCheck)
        {
            if(Random.value < catchChange)
            {
                StartMinigame();
            }
            ResetFishingTimer();
        }
    }
    private void ResetFishingTimer()
    {
        nextFishCheck = Time.time + Random.Range(minBiteTime, maxBiteTime);
    }
    private void StartMinigame()
    {
        isMinigameActive = true;
    }
    private void UpdateMinigame()
    {
        Debug.Log("we in the game");
        MinigamePassed();
    }
    private void MinigamePassed()
    {
        FishSpecies caughtSpecies = SelectFishSpecies();
        GameObject fishObject = Instantiate(caughtSpecies.fishPrefab, hook.position, Quaternion.identity);
        FishManager fishManager = fishObject.GetComponent<FishManager>();
        fishManager.GenerateStats(caughtSpecies);
        fishManager.SetHook(hook);
        AttachEvent(false);
        FishCaughtEvent();
    }
    private IEnumerator DelayedStart()
    {
        yield return null;
        hook = fishLine.GetHookTransform();
    }
    public void SetCurrentBait(BaitType baitType)
    {
        currentBait = baitType;
    }

}
public enum BaitType
{
    Worm,
    Larva,
    Minnow,
    Lure
}
