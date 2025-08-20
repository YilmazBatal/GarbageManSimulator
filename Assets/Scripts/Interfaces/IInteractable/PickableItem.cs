using System;
using UnityEngine;

public class PickableItem : MonoBehaviour, IInteractable
{
    [SerializeField] public TrashTypes trashData;  // name, desc, rarity, weight, value, isToxic, isRecyclable, icon, prefab
    private PlayerInteraction player;
    private GameObject playerHand;
    private Rigidbody rb;
    private bool isPickedUp => player.isHolding;
    private ParticleSystem rarityParticles;

    private void Start()
    {   
        // if not a reward delete the particle system
        // rarityParticles = GetComponentInChildren<ParticleSystem>();
        // if (rarityParticles != null)
        // {
        //     rarityParticles.Stop();
        // }

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>();
        if (player != null)
        {
            playerHand = player.transform.GetChild(0).Find("Hand").gameObject; // Assuming the player's hand is a child of the main camera
        }
        rb = GetComponent<Rigidbody>();

        rb.mass = trashData.weight;

    }
    public void Interact()
    {
        if (!isPickedUp)
        {
            Debug.Log("Item picked up: " + gameObject.name);
            gameObject.transform.SetParent(playerHand.transform); // Detach from parent if needed
            gameObject.transform.localPosition = Vector3.zero; // Reset position to player's hand
            gameObject.transform.localRotation = Quaternion.identity;
            rb.isKinematic = true;
            player.isHolding = true;
            // if its a reward and has been picked up stop and destroy particle system
            // if (rarityParticles != null)
            // {
            //     print("Particle object isnt null its just has been stopped in start");
            //     rarityParticles.Stop();
            //     // Destroy(rarityParticles.gameObject);

            // }
        }
    }
}