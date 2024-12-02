using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip exploClip;
    public AudioClip explo1Clip;
    public AudioClip explo2Clip;
    public AudioClip explo3Clip;
    private void Awake()
    {
        // Tạo hoặc lấy AudioSource từ GameObject.
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.PlayOneShot(clip);
        }
    }
}
