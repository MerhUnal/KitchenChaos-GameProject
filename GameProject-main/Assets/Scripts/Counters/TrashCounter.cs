using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter, IHasProgress
{
    public static event EventHandler OnAnyObjectTrashed;
    public static event EventHandler OnTrashFull;
    public static event EventHandler OnTrashEmptied;

    public event EventHandler<IHasProgress.OnprogressChangedEventArgs> OnProgressChanged;

    private int trashCount;
    private const int maxTrashCount = 4;

    new public static void ResetStaticData()
    {
        OnAnyObjectTrashed = null;
        OnTrashFull = null;
        OnTrashEmptied = null;
    }

    public override void Interact(Player player)
    {
        if (trashCount < maxTrashCount)
        {
            if (player.HasKitchenObject())
            {
                KitchenObject kitchenObject = player.GetKitchenObject();
                if (kitchenObject.GetKitchenObjectSO().name == "FireExtinguisher")
                {
                    
                    return;
                }


                player.GetKitchenObject().DestroySelf();
                trashCount++;
                OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);

                OnProgressChanged?.Invoke(this, new IHasProgress.OnprogressChangedEventArgs
                {
                    progressNormalized = (float)trashCount / maxTrashCount
                });

                if (trashCount >= maxTrashCount)
                {
                    OnTrashFull?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        else
        {
            // Trash is full, cannot add more until emptied
            Debug.Log("Trash is full. Press F to empty.");
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (trashCount >= maxTrashCount)
        {
            trashCount = 0;
            OnTrashEmptied?.Invoke(this, EventArgs.Empty);
            OnProgressChanged?.Invoke(this, new IHasProgress.OnprogressChangedEventArgs
          {
                progressNormalized = (float)trashCount / maxTrashCount
            });
            Debug.Log("Trash emptied");
        }
    }
}
