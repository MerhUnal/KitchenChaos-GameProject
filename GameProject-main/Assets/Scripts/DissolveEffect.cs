using DG.Tweening;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    [SerializeField] private Material dissolveMaterial;
    [SerializeField] private float dissolveDuration = 1f;

    private void Start()
    {
        if (dissolveMaterial != null)
        {
            dissolveMaterial.SetFloat("_dissolveStrength", 0);
        }
    }

    public async Task StartDissolve()
    {
        
        //StartCoroutine(DissolveCoroutine());
       await dissolveMaterial.DOFloat(1, "_dissolveStrength", dissolveDuration).AsyncWaitForCompletion();
    }


    public async Task ReverseDissolve()
    {
        await dissolveMaterial.DOFloat(0, "_dissolveStrength", dissolveDuration).AsyncWaitForCompletion();
    }
}
