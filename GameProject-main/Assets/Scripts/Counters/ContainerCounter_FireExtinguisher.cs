using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter_FireExtinguisher : BaseCounter

{

    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private GameObject fireExtinguisher;

   

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject() )
        {
            //Player is not carring anything
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
            
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);

            if (fireExtinguisher != null)
            {
                fireExtinguisher.SetActive(false);
            }
        }
        else if (player.HasKitchenObject()  && player.GetKitchenObject().GetKitchenObjectSO() == kitchenObjectSO)
        {
            
            player.GetKitchenObject().DestroySelf();
            fireExtinguisher.SetActive(true);
            
            player.ClearKitchenObject();
        }
    }

 


}
