using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class LureboxUI : MonoBehaviour
{
    private Transform cameraTransform;
    private FishingSystem fishingSystem;
    [SerializeField] private GameObject baitPanel;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TMP_Text baitTypeText;
    [SerializeField] private TMP_Text fishTypeText;
    void Start()
    {
        cameraTransform = Camera.main.transform;
        fishingSystem = FishingSystem.instance;
        baitPanel.SetActive(false);
        infoPanel.SetActive(true);
    }
    void Update()
    {
        transform.LookAt(cameraTransform);
        transform.Rotate(0, 180, 0);
    }
    private void ShowBaitUI(BaitType bait)
    {
        infoPanel.SetActive(false);
        baitPanel.SetActive(true);
        baitTypeText.text = bait.ToString();
        List<string> fish = new List<string>();
        foreach(FishSpecies species in fishingSystem.AvailableFish)
        {
            if(species.preferredBait.Contains(bait))
            {
                fish.Add(species.name);
            }
        }
        if(fish.Count > 1)
        {
            fishTypeText.text = string.Join(",\n", fish);
        }
        else if(fish.Count == 1)
        {
            fishTypeText.text = fish[0];
        }
        else
        {
            fishTypeText.text = "All";
        }
    }
    public void HideBaitUI()
    {
        baitPanel.SetActive(false);
        infoPanel.SetActive(true);
    }

    //shitty workaround but different function for each bait cuz of the pointable unity event wrapper, could make custom one that allows passing variables?
    public void WormBaitType()
    {
        ShowBaitUI(BaitType.Worm);
    }
    public void LarvaBaitType()
    {
        ShowBaitUI(BaitType.Larva);
    }
    public void MinnowBaitType()
    {
        ShowBaitUI(BaitType.Minnow);
    }
    public void LureBaitType()
    {
        ShowBaitUI(BaitType.Lure);
    }
}
