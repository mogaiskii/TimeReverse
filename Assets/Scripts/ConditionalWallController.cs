using UnityEngine;

public class ConditionalWallController : MonoBehaviour
{
    public FloorButtonController subscribeTo;
    public bool state = true;

    public Collider2D collider2d;
    public SpriteRenderer spriteRenderer;

    public static int bgLayer = 5;
    public static int wallLayer = 1;

    private bool _prohibitedToLock;
    public Sprite StateOn;
    public Sprite StateOff;
    public SpriteRenderer rendererer;
    
    // Update is called once per frame
    void Start()
    {
        OnSubscription();
    }

    public void OnSubscription()
    {
        
        if (subscribeTo != null)
            subscribeTo.StateChanged += OnStateChanged;
        SwitchWall();
    }

    void SwitchWall()
    {
        if (state && !_prohibitedToLock)
        {
            collider2d.isTrigger = false;
            transform.position = new Vector3(transform.position.x, transform.position.y, wallLayer);
            spriteRenderer.color =
                new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
        }
        else
        {
            collider2d.isTrigger = true;
            transform.position = new Vector3(transform.position.x, transform.position.y, bgLayer);
            spriteRenderer.color =
                new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.25f);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        _prohibitedToLock = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _prohibitedToLock = false;
        SwitchWall();
    }

    void OnStateChanged(bool newState)
    {
        state = !state;
        SwitchWall();
        // TODO: mb vfx
        if (state)
        {
            rendererer.sprite = StateOn;
        }
        else spriteRenderer.sprite = StateOff;
    }
}
