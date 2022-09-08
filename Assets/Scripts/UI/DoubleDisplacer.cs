using UnityEngine;

public class DoubleDisplacer : Displacer
{
    [SerializeField]
    Vector2 extraDisplace = Vector2.zero;
    Vector2 basePos = Vector2.zero;

    /*changes 'basePos' between zero and 
     * 'extraDisplace' according to its actual value,
     * moving the gameobject in local space to match it if required*/
    public void Switcheroo()
    {
        basePos = basePos == Vector2.zero ? extraDisplace : Vector2.zero;
        LoadRect();
        if (rectTransform.anchoredPosition != displaceAmount) rectTransform.anchoredPosition = basePos;
    }

    /*moves the gameobject in local space between
     * 'basePos' and 'displaceAmount' according to its actual position
     * and rotates it's child 180º in Z*/
    public override void Displace()
    {
        LoadRect();
        rectTransform.anchoredPosition = rectTransform.anchoredPosition == displaceAmount ? basePos : displaceAmount;
        rectTransform.GetChild(0).Rotate(0, 0, 180);
    }
}
