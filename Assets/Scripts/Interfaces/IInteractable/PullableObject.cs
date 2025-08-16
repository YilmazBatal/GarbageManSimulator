using UnityEngine;

public class PullableObject : MonoBehaviour, IInteractable
{
    PlayerInteraction playerInteraction;
    Rigidbody rb;

    void Start()
    {
        playerInteraction = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>();
        rb = GetComponent<Rigidbody>();    
    }

    public void Interact()
    {
        Vector3 pullDir = (transform.position - playerInteraction.transform.position).normalized;

        rb.AddForce(-pullDir * playerInteraction.pullPower, ForceMode.Impulse);
    }
}
