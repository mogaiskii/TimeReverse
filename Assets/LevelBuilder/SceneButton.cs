using UnityEngine;

public class SceneButton : MonoBehaviour
{

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("ButtonClicked");
            ComponentsPallete.instance.SaveToClip();
        }
    }
}
