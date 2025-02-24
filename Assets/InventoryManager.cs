using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    

    public static InventoryManager instance;

    public List<FishSpecies> fishSpecies = new List<FishSpecies>();
    private List<FishSpecies> caughtFish = new List<FishSpecies>();

    public GameObject inventoryPrefab;



    void Start()
    {
        updateInventory();
    }

    public void AddFish(FishSpecies fish){
        caughtFish.Add(fish);
        updateInventory();
    }


    void updateInventory(){

        foreach (FishSpecies fish in fishSpecies)
        {
            if(caughtFish.Contains(fish)){
                continue;
            }
            else{
                GameObject inventoryItem = Instantiate(inventoryPrefab, transform);
                inventoryItem.GetComponent<Image>().sprite = fish.fishImage;
            }
        }

    }


}
