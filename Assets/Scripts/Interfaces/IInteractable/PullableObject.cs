using UnityEngine;

public class PullableObject : MonoBehaviour, IInteractable
{
    PlayerInteraction playerInteraction;
    Rigidbody rb;
    [SerializeField] AudioSource audioSource;
    [SerializeField] float maxSpeed = 5f;   // hızı normalize etmek için
    [SerializeField] float minPitch = 0.8f;
    [SerializeField] float maxPitch = 1.2f;
    
    void Start()
    {
        playerInteraction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Rigidbody hızı
        float speed = rb.linearVelocity.magnitude;

        if(speed > 0.1f) // hareket ediyorsa
        {
            if(!audioSource.isPlaying)
                audioSource.Play();

            // pitch hıza göre
            audioSource.pitch = Mathf.Lerp(minPitch, maxPitch, speed / maxSpeed);
            // volume da hıza göre ayarlanabilir
            audioSource.volume = Mathf.Clamp01(speed / maxSpeed);
        }
        else
        {
            if(audioSource.isPlaying)
                audioSource.Stop();
        }
    }

    public void Interact()
    {
        Vector3 pullDir = (transform.position - playerInteraction.transform.position).normalized;

        rb.AddForce(-pullDir * playerInteraction.pullPower, ForceMode.Impulse);
    }
    
}
