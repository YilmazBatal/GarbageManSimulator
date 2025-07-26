using UnityEngine;

public class PickableItem : MonoBehaviour, IInteractable
{
    private PlayerInteraction player;
    private GameObject playerHand;
    private Rigidbody rb;
    private bool isPickedUp => player.isHolding;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>();
        if (player != null)
        {
            playerHand = player.transform.GetChild(0).Find("Hand").gameObject; // Assuming the player's hand is a child of the main camera
        }
        rb = GetComponent<Rigidbody>();

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
        }
    }
}