using UnityEngine;

public class SelectableScript : MonoBehaviour
{
    public void OnClick()
    {
        UIManager.Instance.SetLastSelectedButton(gameObject);
    }
}
