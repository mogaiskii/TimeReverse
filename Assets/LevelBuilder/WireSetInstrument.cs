using System;
using System.Collections.Generic;
using UnityEngine;

public class WireSetInstrument : MonoBehaviour
{

    public bool active = false;
    public PalleteChoice startItem;
    
    private void OnMouseOver()
    {
        if (active == false && Input.GetMouseButtonDown(0))
        {
            GameObject hoverInstance = Instantiate(gameObject, transform.position, Quaternion.identity);
            WireSetInstrument instrument = hoverInstance.GetComponent<WireSetInstrument>();
            instrument.SetActive();
            ComponentsPallete.instance.SetWireInstrument(instrument);
        }
    }

    private void SetActive()
    {
        active = true;
        transform.localScale = new Vector3(0.5f, 0.5f, transform.localScale.z);
    }

    private void FixedUpdate()
    {
        if (startItem != null)
        {
            Vector3 startPos = new Vector3(transform.position.x, transform.position.y, 0);
            Vector3 endPos = new Vector3(startItem.transform.position.x, startItem.transform.position.y, 0);
            Debug.DrawLine(startPos, endPos, Color.magenta);
        }
    }

    public void connectItem(PalleteChoice selectedItem)
    {
        if (startItem == null && selectedItem.hasConnection)
        {
            startItem = selectedItem;
        }

        if (startItem != null && selectedItem.isConnectionTarget)
        {
            startItem.currentConnection = selectedItem;
            startItem = null;
        }
    }
}
