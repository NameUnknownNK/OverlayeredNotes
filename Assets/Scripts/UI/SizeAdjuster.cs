using UnityEngine;

public class SizeAdjuster : MonoBehaviour
{
    //sets localscale of gameobject to (size,size,0)
    public void SetSize(float size)
    {
        transform.localScale = new Vector3(size,size);
    }
}
