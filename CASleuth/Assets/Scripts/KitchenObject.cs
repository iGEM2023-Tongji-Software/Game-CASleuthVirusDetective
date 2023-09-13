using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        if (this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }
        this.kitchenObjectParent = kitchenObjectParent;

        if (kitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("IKitchenObjectParent already has a KitchenObject!");
        }
        kitchenObjectParent.SetKitchenObject(this);

        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject();

        Destroy(gameObject);
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
        return kitchenObject;
    }

    //判断是否是能提交的种类之一
    public bool TrytoDeliver(out KitchenObject kitchenObject)
    {
        if(this.GetKitchenObjectSO().objectName == "dv2-CAS9-detRNP" || this.GetKitchenObjectSO().objectName == "rv2-CAS12-detRNP" ||
            this.GetKitchenObjectSO().objectName == "rv2-CAS13-detRNP" || this.GetKitchenObjectSO().objectName == "dv1-CAS9-detRNP" ||
            this.GetKitchenObjectSO().objectName == "rv1-CAS12-detRNP" || this.GetKitchenObjectSO().objectName == "rv1-CAS13-detRNP")
        {
            kitchenObject = this as KitchenObject;
            return true;
        }
        else
        {
            kitchenObject = null;
            return false;
        }
    }

}
