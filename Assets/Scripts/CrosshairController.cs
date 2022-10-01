using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    public static bool isExpanded = false;
    public static bool isShrinked = true;

    public static void expandCrosshair()
    {
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Crosshair"))
        {
            if (isShrinked)
                obj.GetComponent<RectTransform>().localPosition *= 2;
        }
        isExpanded = true;
        isShrinked = false;
    }

    public static void shrinkCrosshair()
    {
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Crosshair"))
        {
            if (isExpanded)
            obj.GetComponent<RectTransform>().localPosition /= 2;
        }
        isExpanded = false;
        isShrinked = true;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
