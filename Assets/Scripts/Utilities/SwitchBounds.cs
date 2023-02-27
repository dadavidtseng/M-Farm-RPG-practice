using Cinemachine;
using UnityEngine;

public class SwitchBounds : MonoBehaviour
{
    //TODO : 切換場景後更改調用
    private void Start() 
    {
        SwitchConfinerShape();
    }

    private void SwitchConfinerShape()
    {
        PolygonCollider2D confinerShape = 
            GameObject.FindGameObjectWithTag("BoundsConfiner").GetComponent<PolygonCollider2D>();

        CinemachineConfiner confiner = GetComponent<CinemachineConfiner>();
        
        confiner.m_BoundingShape2D = confinerShape;
        
        //Call this if the bounding shape's shape change at runtime
        confiner.InvalidatePathCache();
    }
}
