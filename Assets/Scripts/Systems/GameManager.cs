using UnityEngine;
using UnityEngine.Events;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private UnityEvent onAttachedToWater = new UnityEvent();
    private UnityEvent onDetachedFromWater = new UnityEvent();

    [Header("Flags")]
    public bool holdingFish = false;

    [Header("Variables")]
    public float waterLevel;

    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        onAttachedToWater.AddListener(TestFunc);
    }
    public void AttachEvent(bool attached)
    {
        if(attached) onAttachedToWater?.Invoke();
        else onDetachedFromWater?.Invoke();
    }
    private void TestFunc()
    {
        Debug.Log("ATTACHED TO WATER");
    }
}
