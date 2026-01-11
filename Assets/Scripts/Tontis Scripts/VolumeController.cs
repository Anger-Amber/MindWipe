using UnityEngine;

public class VolumeController : MonoBehaviour
{
    [SerializeField] MenuController volumeSetting;
    [SerializeField] AudioSource source;
    float volume;
    void Update()
    {
        volume = volumeSetting.musicVolume;
        source.volume = volume / 9;
    }
}
