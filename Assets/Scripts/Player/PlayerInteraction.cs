using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] float interactRange = 5f;
    [SerializeField] float throwForce = 10f;
    public bool isHolding = false;
    IInteractable currentTarget;
    GameObject trashObject;
    GameObject playerHand;
    GameObject heldItem;
    GameObject lastOutlinedObject;

    void Start()
    {
        playerHand = gameObject.transform.GetChild(0).transform.Find("Hand").gameObject; // Assuming the player's hand is a child of the player GameObject
        trashObject = GameObject.Find("Trash"); // Find the trash object in the scene
    }

    void Update()
    {
        if (!UIManager.Instance.isAnyPanelOpen)
        { // If any UI panel is open, disable interaction
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
            { // Check if the ray hits an object within the interaction range
                IInteractable target = hit.collider.GetComponent<IInteractable>();

                if (target != null) // if ray was hit by an interactable object
                {
                    if (hit.collider.gameObject != lastOutlinedObject) // if the item player looks at is different from the last object that player looked at 
                    {
                        DisableLastOutline();

                        Outline outline = hit.collider.GetComponent<Outline>();
                        if (outline != null)
                        {
                            outline.enabled = true;
                            lastOutlinedObject = hit.collider.gameObject;
                        }
                    }

                    currentTarget = target;
                    return; // Successfully opened outline, early exit
                }
            }
        }
        // If no interactable object is found, disable the last outline and reset current target
            currentTarget = null;
        DisableLastOutline();
    }

    void DisableLastOutline()
    {
        if (lastOutlinedObject != null)
        {
            Outline outline = lastOutlinedObject.GetComponent<Outline>();
            if (outline != null) outline.enabled = false;
            lastOutlinedObject = null;
        }
    }

    public void TryInteract()
    {
        currentTarget?.Interact();
    }
    public void TryThrow()
    {
        heldItem = playerHand.transform.GetChild(0).gameObject; // Get the item currently held in the player's hand
        print("Throwing item");
        if (isHolding)
        {
            print("Throwing item: " + heldItem.name);
            heldItem.GetComponent<Rigidbody>().isKinematic = false;
            heldItem.transform.SetParent(trashObject.transform);
            heldItem.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);
            isHolding = false; // Reset holding state after throwing
        }
    }
}

// do throwing item and holding in hand logic