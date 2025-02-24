using TMPro;
using UnityEngine;

public class RodUI : MonoBehaviour
{
    private FishingSystem fishingSystem;
    [SerializeField] private TMP_Text currentBaitText;
    void Start()
    {
        fishingSystem = FishingSystem.instance;
        fishingSystem.onBaitChanged.AddListener(CurrentBaitUI);
        currentBaitText.text = fishingSystem.CurrentBait.ToString();
    }
    private void CurrentBaitUI()
    {
        currentBaitText.text = fishingSystem.CurrentBait.ToString();
    }

}
