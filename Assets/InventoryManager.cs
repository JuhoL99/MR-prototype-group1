using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    

    public static InventoryManager instance;

    public List<FishSpecies> fishSpecies = new List<FishSpecies>();

    public GameObject inventoryPrefab;



    void Start()
    {
        updateInventory();
    }

    public void AddFish(FishSpecies fish){
        fishSpecies.Add(fish);
        updateInventory();
    }


    void updateInventory(){
        foreach(GameObject gm in GameObject.FindGameObjectsWithTag("InventoryItem")){
            Destroy(gm);
        }
        foreach (FishSpecies fish in fishSpecies)
        {
            GameObject inventoryItem = Instantiate(inventoryPrefab, transform);
            inventoryItem.GetComponent<Image>().sprite = fish.fishImage;
        }

    }


}
