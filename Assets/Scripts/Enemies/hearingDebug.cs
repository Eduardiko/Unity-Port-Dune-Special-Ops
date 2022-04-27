using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hearingDebug : MonoBehaviour
{
    private bool once = true;
    public LineRenderer lDrawer;
    public EnemyDetection data;
    public GameObject info;
 
    // Start is called before the first frame update
    public void DebugDraw()
    {
        if (once)
        {
            gameObject.AddComponent<LineRenderer>();
            lDrawer = gameObject.GetComponent<LineRenderer>();
            once = false;
        }
        gameObject.DrawCircleScaled(data.hearingRadius, 0.05f,info.transform.localScale);
    }

    public void DebugDelete()
    {
        Destroy(gameObject.GetComponent<LineRenderer>());
        once = true;
    }
}