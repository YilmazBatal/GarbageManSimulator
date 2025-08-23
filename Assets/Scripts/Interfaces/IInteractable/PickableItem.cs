using UnityEngine;

public class PickableItem : MonoBehaviour, IInteractable
{
    [SerializeField] public TrashTypes trashData;  // name, desc, rarity, weight, value, isToxic, isRecyclable, icon, prefab
    private PlayerInteraction player;
    private Rigidbody rb;
    private bool isPickedUp => player.isHolding;
    public bool isReward = false;
    private ParticleSystem rarityParticles;
    AudioSource audioSource;

    private void Start()
    {
        ParticleManagement();
        GetComponents();

        rb.mass = trashData.weight;
    }

    private void GetComponents()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (!isPickedUp)
        {
            gameObject.transform.SetParent(player.playerHand.transform); // Detach from parent if needed
            gameObject.transform.localPosition = Vector3.zero; // Reset position to player's hand
            gameObject.transform.localRotation = Quaternion.identity;
            rb.isKinematic = true;
            player.isHolding = true;
            if (rarityParticles != null)
            {
                rarityParticles.Stop();
                Destroy(rarityParticles.gameObject);
            }
            LeanTween.cancel(gameObject);

        }
    }
    void ParticleManagement()
    {
        rarityParticles = GetComponentInChildren<ParticleSystem>();
        if (rarityParticles != null)
        {
            if (isReward)
            {
                print("Particle system playing: " + gameObject.name);
                rarityParticles.Play();
            }
            else // not a reward
            {
                rarityParticles.Stop();
                Destroy(rarityParticles.gameObject);
                rarityParticles = null;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 1f) // eşik değer{
        {
            // Enum ismini string’e çevir
            TrashMadeOf key = trashData.madeOf;


            // AudioManager’daki listeden ara
            SoundItem sound = System.Array.Find(AudioManager.Instance.madeOf, s => s.audioName == key);

            if (sound != null && sound.clip != null)
                audioSource.PlayOneShot(sound.clip[Random.Range(0, sound.clip.Length)]);
        }
    }
}