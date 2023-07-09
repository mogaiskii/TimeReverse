using System.Collections.Generic;
using UnityEngine;

public class OnOffInstrument : MonoBehaviour
{
    public bool active = false;
    
    private void OnMouseOver()
    {
        if (active == false && Input.GetMouseButtonDown(0))
        {
            GameObject hoverInstance = Instantiate(gameObject, transform.position, Quaternion.identity);
            OnOffInstrument instrument = hoverInstance.GetComponent<OnOffInstrument>();
            instrument.SetActive();
            ComponentsPallete.instance.SetOnOffInstrument(instrument);
        }
    }

    private void SetActive()
    {
        active = true;
        transform.localScale = new Vector3(0.5f, 0.5f, transform.localScale.z);
    }

    public void SwitchItem(PalleteChoice selectedItem)
    {
        if (selectedItem.hasInnerState)
        {
            selectedItem.SetInnerState(!selectedItem.currentInnerState);
        }
    }
}
