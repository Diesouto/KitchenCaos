using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent newKitchenObjectParent)
    {
        if (this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }

        this.kitchenObjectParent = newKitchenObjectParent;

        if (newKitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("IKParent already has a kitchen object!");
        }

        newKitchenObjectParent.SetKitchenObject(this);

        transform.parent = newKitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }
    
    public IKitchenObjectParent GetClearKitchenObjectParent()
    {
        return kitchenObjectParent;
    }
}
