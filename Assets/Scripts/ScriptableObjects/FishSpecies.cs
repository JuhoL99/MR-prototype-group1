using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "FishSpecies", menuName = "FishSpecies")]
public class FishSpecies : ScriptableObject
{
    public string speciesName;
    public string speciesDescription;
    public GameObject fishPrefab; //model
    public List<BaitType> preferredBait;

    [Header("Size")]
    public float minWeight;
    public float maxWeight;
    public float minLength;
    public float maxLength;
}
