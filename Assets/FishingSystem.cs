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
    public FishingReel reel;

    public UnityEvent onAttachedToWater = new UnityEvent();
    public UnityEvent onDetachedFromWater = new UnityEvent();
    public UnityEvent<FishSpecies> onFishBite = new UnityEvent<FishSpecies>();
    public UnityEvent onFishCaught = new UnityEvent();
    public UnityEvent<FishManager> onFishGrabbed = new UnityEvent<FishManager>();
    public UnityEvent onFishReleased = new UnityEvent();
    public UnityEvent onBaitChanged = new UnityEvent();

    [SerializeField] private float waterLevel = 0f;
    public float WaterLevel => waterLevel;
    public FishSpecies[] AvailableFish => availableFish;
    [SerializeField] private FishSpecies[] availableFish;
    public bool RodHoldingFish => rodHoldingFish;
    private bool rodHoldingFish;
    public int HeldFishCount => heldFishCount;
    private int heldFishCount;
    public BaitType CurrentBait => currentBait;
    private BaitType currentBait;

    //
    [SerializeField] private float minBiteTime = 3f;
    [SerializeField] private float maxBiteTime = 13f;
    [SerializeField] private float catchChange = 0.5f;
    [SerializeField] private FishLine fishLine; //temporary
    private Transform hook; //temporary

    private GameManager gameManager;
    public bool IsRodGrabbed => isRodGrabbed;
    private bool isRodGrabbed = false;
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
        onFishCaught?.Invoke();
    }
    public void FishGrabbedEvent(FishManager fish)
    {
        rodHoldingFish = false;
        heldFishCount++;
        onFishGrabbed?.Invoke(fish);
    }
    public void FishDroppedEvent()
    {
        heldFishCount = Math.Max(0, heldFishCount - 1);
        onFishReleased?.Invoke();
    }
    public void RodGrabbedEvent(bool rodGrabbed)
    {
        isRodGrabbed = rodGrabbed;
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
        if (isHookInWater) return;
        currentBait = baitType;
        onBaitChanged?.Invoke();
    }

}
public enum BaitType
{
    Worm,
    Larva,
    Minnow,
    Lure
}
