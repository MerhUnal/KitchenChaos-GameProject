using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem fireEffect;

    public System.Action startFireAction; // Yang?n ba?latma action'?
    private void Start()
    {
       startFireAction += StartRandomFire;
    }

    private void OnDestroy()
    {
       // KitchenGameManager.Instance.startFireAction -= StartRandomFire;
    }

    public void StartRandomFire()
    {
        // Yang?n efektini aktif hale getirme
        if (fireEffect != null)
        {
            fireEffect.gameObject.SetActive(true);
           
        }
    }

    public void ExtinguishFire()
    {
        // Yang?n efektini devre d??? b?rakma
        if (fireEffect != null)
        {
            fireEffect.gameObject.SetActive(false);
        }
    }
}
