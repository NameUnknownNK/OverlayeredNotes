using UnityEngine;

public class Displacer : NotMonoBehaviour
{
    public Vector2 displaceAmount;
    protected RectTransform rectTransform = null;

    #region Init

    protected void LoadRect()
    {
        if (rectTransform == null) rectTransform = transform as RectTransform;
    }

    #endregion

    #region Funtionality

    /*moves the gameobject in local space between
     * zero and 'displaceAmount' according to its actual position
     * and rotates it's child 180º in Z*/
    public virtual void Displace()
    {
        LoadRect();
        rectTransform.anchoredPosition = rectTransform.anchoredPosition == displaceAmount ? Vector2.zero : displaceAmount;
        rectTransform.GetChild(0).Rotate(0, 0, 180);
    }

    #endregion
}
