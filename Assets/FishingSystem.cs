using System.Collections;
using UnityEngine;

public class FishingSystem : MonoBehaviour
{
    [Header("Catching")]
    [SerializeField] private float minBiteTime = 3f;
    [SerializeField] private float maxBiteTime = 13f;
    [SerializeField] private float catchChange = 0.5f;
    [SerializeField] private FishLine fishLine; //temporary
    [SerializeField] private GameObject fishPrefab; //temporary
    private Transform hook; //temporary

    private GameManager gameManager;
    private bool isHookInWater = false;
    private bool isFishingActive = false;
    private bool isMinigameActive = false;
    private float nextFishCheck;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
        ResetFishingTimer();
        gameManager.onAttachedToWater.AddListener(OnHookInWater);
        gameManager.onDetachedFromWater.AddListener(OnHookOutOfWater);
        StartCoroutine(DelayedStart());
    }
    private void Update()
    {
        if(!isHookInWater || !isFishingActive) return;
        if (!isMinigameActive) CheckForFish();
        else UpdateMinigame();
    }
    private void OnHookInWater()
    {
        isHookInWater = true;
        isFishingActive = true;
        ResetFishingTimer();
    }
    private void OnHookOutOfWater()
    {
        isHookInWater = false;
        isFishingActive = false;
        isMinigameActive = false;
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
        GameObject fishObject = Instantiate(fishPrefab, hook.position, Quaternion.identity);
        FishManager fishManager = fishObject.GetComponent<FishManager>();
        fishManager.SetHook(hook);
        gameManager.AttachEvent(false);
        gameManager.FishCaughtEvent();
    }
    private IEnumerator DelayedStart()
    {
        yield return null;
        hook = fishLine.GetHookTransform();
    }
}
