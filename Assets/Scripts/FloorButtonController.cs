using UnityEngine;

public class FloorButtonController : MonoBehaviour
{
    public delegate void StateChangeHandler(bool state);
    public event StateChangeHandler StateChanged;

    public bool IsTemproraral;

    public AudioSource soundSource;
    public AudioClip openSound;
    public AudioClip closeSound;

    private bool _state ;

    public Sprite StateOn;
    public Sprite StateOff;
    public SpriteRenderer rendererer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsTemproraral)
        {
            _state = true;
        }
        else
        {
            _state = !_state;
        }

        PlaySound();
        ChangeMyState();
    }

    private void PlaySound()
    {
        if (_state) soundSource.clip = openSound;
        else soundSource.clip = closeSound;

        soundSource.Play();
    }

    void ChangeMyState()
    {
        StateChanged?.Invoke(_state);
        if (_state)
        {
            rendererer.sprite = StateOn;
        }
        else rendererer.sprite = StateOff;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsTemproraral)
        {
            _state = false;
            PlaySound();
            ChangeMyState();
        }
    }
}
