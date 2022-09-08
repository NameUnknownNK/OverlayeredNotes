using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ColliderSetter : MonoBehaviour
{
    [SerializeField]
    Vector2 relOffset = Vector2.zero;

    /* Resizes the collider to match the current size of the gameobject
     * adds an offset to it relative to its size according to 'relOffset'
     * and deletes this component*/
    void Start()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        Vector2 size = (transform as RectTransform).rect.size;
        col.size = size;
        col.offset = relOffset * size;
        Destroy(this);
    }
}
