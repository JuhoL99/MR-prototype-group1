using System.Collections;
using UnityEngine;

public class RoomTagger : MonoBehaviour
{
    //quick shitty script since i couldnt find better way to tag room for hits
    private void Start()
    {
        StartCoroutine(TagWithDelay());
    }
    private IEnumerator TagWithDelay()
    {
        yield return new WaitForSeconds(1f);
        GameObject[] allObjects = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains("EffectMesh"))
            {
                obj.tag = "CanStick";
            }
        }
    }
    public void TagRoom()//doesnt work needs delay
    {
        GameObject[] allObjects = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains("EffectMesh"))
            {
                obj.tag = "CanStick";
            }
        }
    }
    private void Test()
    {
        return;
    }
}
