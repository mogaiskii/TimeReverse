using UnityEngine;


public enum Turning
{
    ok,
    ok90,
    ok180,
    ok270
}

public class WallLooking : MonoBehaviour
{
    public bool isCorner;
    
    public Sprite Normal;
    public Sprite Corner;

    public Turning turning;

    public SpriteRenderer rendererer;
    // Start is called before the first frame update
    void Start()
    {
        if (isCorner) rendererer.sprite = Corner;
        else rendererer.sprite = Normal;
        //
        // if (!isCorner)
        // {
        //     if (transform.position.x == -7) turning = Turning.ok270;
        //     else if (transform.position.x == 4) turning = Turning.ok90;
        //     else if (transform.position.y == -4) turning = Turning.ok180;
        // }
        // switch (turning)
        // {
        //     case Turning.ok90:
        //         transform.rotation = new Quaternion(0, 0, 90, 0);
        //         break;
        //     case Turning.ok180:
        //         transform.rotation = new Quaternion(0, 0, 180, 0);
        //         break;
        //     case Turning.ok270:
        //         transform.rotation = new Quaternion(0, 0, 270, 0);
        //         break;
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
