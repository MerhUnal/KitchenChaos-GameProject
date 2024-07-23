using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManager : MonoBehaviour
{
    public GameObject firePrefab;
    public Transform[] firePoints;

    private void Start()
    {
        KitchenGameManager.Instance.startFireAction += StartRandomFire;
    }

    private void OnDestroy()
    {
        KitchenGameManager.Instance.startFireAction -= StartRandomFire;
    }

    private void StartRandomFire()
    {
        if (firePoints.Length == 0) return;

        int randomIndex = UnityEngine.Random.Range(0, firePoints.Length);
        Transform firePoint = firePoints[randomIndex];
        Instantiate(firePrefab, firePoint.position, firePoint.rotation);
    }
}
