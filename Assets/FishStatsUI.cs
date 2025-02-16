using UnityEngine;
using TMPro;

public class FishStatsUI : MonoBehaviour
{
    [SerializeField] private GameObject fishStatsPanel;
    [SerializeField] private TMP_Text speciesNameText;
    [SerializeField] private TMP_Text weightText;
    [SerializeField] private TMP_Text lengthText;
    private FishingSystem fishingSystem;
    private Transform targetFish;

    private void Start()
    {
        fishStatsPanel.SetActive(false);
        fishingSystem = FishingSystem.instance;
        fishingSystem.onFishGrabbed.AddListener(UpdateAndShowUI);
        fishingSystem.onFishReleased.AddListener(UpdateAndHideUI);
    }
    private void Update()
    {
        if (targetFish == null) return;
        Vector3 offset = targetFish.forward * 0.9f;
        transform.position = targetFish.position + offset;
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
    private void UpdateAndShowUI(FishManager fish)
    {
        targetFish = fish.transform;
        speciesNameText.text = fish.Species.name;
        weightText.text = fish.Weight.ToString("F2") + " kg";
        lengthText.text = fish.Length.ToString("F2") + " m";
        fishStatsPanel.SetActive(true);
    }
    private void UpdateAndHideUI()
    {
        if(fishingSystem.HeldFishCount == 0)
        {
            targetFish = null;
            fishStatsPanel.SetActive(false);
        }
    }
}
