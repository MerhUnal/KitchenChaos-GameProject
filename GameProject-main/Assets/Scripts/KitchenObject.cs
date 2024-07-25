using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKithchenObjectParent kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO()
    {

    return kitchenObjectSO;
    }
    public void SetKitchenObjectParent(IKithchenObjectParent kitchenObjectParent)
    {
        if(this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }

        this.kitchenObjectParent = kitchenObjectParent;

        if (kitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("IKitchenObjectParent has a KitchenObject");
        }
        kitchenObjectParent.SetKitchenObject(this);

        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
        if (kitchenObjectSO.objectName == "FireExtinguisher")
        {
            transform.localRotation = Quaternion.Euler(0, 90, 90); 
        }

    }

    public IKithchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    

    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }
    

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKithchenObjectParent kithchenObjectParent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();

        kitchenObject.SetKitchenObjectParent(kithchenObjectParent);

        return kitchenObject;
    }

}

