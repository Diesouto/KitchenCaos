using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] KitchenObjectSO kitchenObject;
    [SerializeField] Transform countertopPoint;

    public void Interact()
    {
        Debug.Log("Interacting with ClearCounter");
        Transform kitchenObjectTransform = Instantiate(kitchenObject.prefab, countertopPoint);
        kitchenObjectTransform.localPosition = Vector3.zero;
    }
}
