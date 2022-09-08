using UnityEngine;

//maskes the gameobject spin nonstop
public class Spin : MonoBehaviour
{

    void FixedUpdate()
    {
        transform.Rotate(1f, 1f, 1f);
    }
}
