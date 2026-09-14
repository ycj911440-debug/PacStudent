using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource intro;
    public AudioSource ghostNormal;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        intro.Play();
        Invoke(nameof(StartNormal), 3f);
    }

    void StartNormal()
    {
        intro.Stop();
        ghostNormal.Play();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
