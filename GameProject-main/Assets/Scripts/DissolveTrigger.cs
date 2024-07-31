using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

public class DissolveTrigger : MonoBehaviour
{
    public Transform destination;
    public GameObject playerg;
    private float dissolveDuration = 1f;
    private bool isPlayerTeleported = false;
    Player player;

    private void Start()
    {
        player=playerg.GetComponent<Player>();
    }

    private async void OnTriggerEnter(Collider other)
    {
        
        // Sadece playerg tetikleyiciye girdiğinde çalışacak
        if (other.gameObject == playerg)
        {
            
            if (player.isTeleported) return;
            player.isTeleported = true;
            player.enteredCollider = GetComponent<Collider>();
            DissolveEffect dissolveEffect = other.GetComponent<DissolveEffect>();
            if (dissolveEffect != null)
            {
               await DissolveAndTeleport(dissolveEffect);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerg)
        {
            player.CheckTeleport(GetComponent<Collider>());
        }
    }

    private async Task DissolveAndTeleport(DissolveEffect dissolveEffect)
    {
        print("StartDissolve");
        CharacterController characterController = playerg.GetComponent<CharacterController>();

        if (characterController != null)
        {
            characterController.enabled = false; // Oyuncunun hareket etmesini engelle
        }

        var myTween= dissolveEffect.StartDissolve();
        await myTween;

        // Çözülme efekti tamamlanana kadar bekleyin

        

        // Oyuncu hala tetikleyici içindeyse yeni konuma taşıyoruz
        playerg.transform.position = destination.position;

        var myTween2 = dissolveEffect.ReverseDissolve();
        await myTween2;

        if (characterController != null)
        {
            characterController.enabled = true; // Oyuncunun hareket etmesini tekrar etkinleştir
        }

    }
}
