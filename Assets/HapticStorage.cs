using Oculus.Haptics;
using UnityEngine;

public class HapticStorage : MonoBehaviour
{
    private HapticSource source;
    [SerializeField] private HapticClip testClip;

    private void Start()
    {

    }
    public void PlayTestClip()
    {
        source.clip = testClip;
        source.Play();
    }

}
