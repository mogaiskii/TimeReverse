using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PalleteItemState
{
    IN_PALLETE,
    ON_MOUSE,
    ON_BOARD
}

public class PalleteChoice : MonoBehaviour
{
    public string objectName;
    public bool hasInnerState;
    public bool currentInnerState;
    public bool hasConnection;
    public bool isConnectionTarget;
    public PalleteChoice currentConnection;
    public PalleteItemState state = PalleteItemState.IN_PALLETE;
    public Collider2D objectCollider;
    public Vector2 boardCoords;

    private void FixedUpdate()
    {
        if (hasConnection && currentConnection != null)
        {
            Vector3 startPos = new Vector3(transform.position.x, transform.position.y, 0);
            Vector3 endPos = new Vector3(
                currentConnection.transform.position.x, currentConnection.transform.position.y, 0);
            Debug.DrawLine(startPos, endPos, Color.red);
        }
    }

    public string ToLevelStr()
    {
        string outp = objectName;
        if (hasInnerState)
        {
            if (currentInnerState) outp = outp + ":true";
            else outp = outp + ":false";
        }

        if (hasConnection)
        {
            if (currentConnection == null) Debug.LogError((objectName, transform.position, "must have connection"));
            Vector2 coords = currentConnection.boardCoords;
            outp = outp + "=" + coords.x.ToString() + "," + coords.y.ToString();
        }

        return outp;
    }
    
    public void SetState(PalleteItemState newState)
    {
        state = newState;
        if (state == PalleteItemState.ON_MOUSE)
        {
            objectCollider.enabled = false;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }

    public void SetInnerState(bool newState)
    {
        if (state != PalleteItemState.ON_BOARD) return;

        currentInnerState = newState;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (newState)
        {
            spriteRenderer.color =
                new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
        }
        else
        {
            spriteRenderer.color =
                new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.25f);
        }
    }
    
    private void OnMouseOver()
    {
        if (state == PalleteItemState.IN_PALLETE && Input.GetMouseButtonDown(0))
        {
            GameObject hoverInstance = Instantiate(gameObject, transform.position, Quaternion.identity);
            PalleteChoice palleteChoice = hoverInstance.GetComponent<PalleteChoice>();
            palleteChoice.SetState(PalleteItemState.ON_MOUSE);
            ComponentsPallete.instance.SetPickedObject(palleteChoice);
        }
    }
}
