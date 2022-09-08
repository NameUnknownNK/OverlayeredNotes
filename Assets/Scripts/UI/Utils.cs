using UnityEngine;
using UnityEngine.UI;

public class Utils : NotMonoBehaviour
{
    [SerializeField]
    Color unselectedColor = Color.white;
    [SerializeField]
    Color selectedColor = Color.black;

    public void Flip(GameObject target)
    {
        target.SetActive(!target.activeSelf);
    }

    //changes color of Image to 'selectedColor'
    public void Select(Image image)
    {
        image.color = selectedColor;
    }

    //changes color of Image to 'unselectedColor'
    public void Unselect(Image image)
    {
        image.color = unselectedColor;
    }

    //calls onclick event of 'button'
    public void Press(Button button)
    {
        button.onClick.Invoke();
    }
}
