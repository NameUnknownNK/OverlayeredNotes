using UnityEngine;

//make the gameobject move from side to side of the maincamera view nonstop
public class DVD : MonoBehaviour
{
    Camera mcamera;

    Vector3 move = Vector3.zero;
    int height;
    int width;

    void Start()
    {
        mcamera = Camera.main;
        height = mcamera.scaledPixelHeight;
        width = mcamera.scaledPixelWidth;
        move.x = 10;
        move.y = 10;
    }

    void FixedUpdate()
    {
        transform.position += move*Time.fixedDeltaTime;
        Vector2 screenpos = mcamera.WorldToScreenPoint(transform.position);
        if(screenpos.x <0 | screenpos.x > width)
        {
            move.x *= -1;
        }
        if(screenpos.y <0 | screenpos.y > height)
        {
            move.y *= -1;
        }
    }
}
