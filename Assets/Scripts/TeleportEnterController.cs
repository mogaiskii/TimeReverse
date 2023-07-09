using UnityEngine;

public class TeleportEnterController : MonoBehaviour
{
    public GameObject teleportExit;

    public DudeController dude;
    public AudioSource SoundSource;

    private void OnTriggerEnter2D(Collider2D col)
    {
        SoundSource.Play();
        dude.Teleport(teleportExit.transform.position);
    }
}
