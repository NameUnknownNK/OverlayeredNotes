using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
//changes the color of the component Image to match the one of 'toCopy'
public class ColorCange : MonoBehaviour
{
    [SerializeField]
    Image toCopy = null;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().color= toCopy.color;
    }

}
