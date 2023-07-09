using UnityEngine;

public class FlagController : MonoBehaviour
{
    public AudioSource SoundSource;

    private void OnTriggerEnter2D(Collider2D col)
    {
        SoundSource.Play();
        ProgramManager.instance.LevelIsWon();
    }
}
