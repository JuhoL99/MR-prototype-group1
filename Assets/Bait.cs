using UnityEngine;

public class Bait : MonoBehaviour
{
    private FishingSystem fishingSystem;
    public BaitType ThisBait => thisBait;
    [SerializeField] BaitType thisBait;
    private void Start()
    {
        fishingSystem = FishingSystem.instance;
    }
    public void ChooseBait()
    {
        fishingSystem.SetCurrentBait(thisBait); //select event doesnt allow passing of values so every bait script separate
    }
}
