using UnityEngine;
using TMPro;

public class TextUpdater : NotMonoBehaviour
{
    TMP_Text text = null;
    [SerializeField]
    int wordIndexToCheck = 0;
    [SerializeField]
    string[] wordToChange = new string[2];

    #region Funtionalities

    /*changes the text of the TMPro component
     * switches the 'wordIndexToCheck' position word of the current text
     * between the 2 words stored in 'wordToChange'*/
    public void Change()
    {
        if (text == null) text = GetComponentInChildren<TMP_Text>(true);
        string[] phrase = text.text.Split(' ');
        if (phrase[wordIndexToCheck] == wordToChange[0]) phrase[wordIndexToCheck] = wordToChange[1];
        else
        {
            if (phrase[wordIndexToCheck] == wordToChange[1]) phrase[wordIndexToCheck] = wordToChange[0];
            else Debug.Log("fail on match: " + phrase[wordIndexToCheck] + " with " + wordToChange);
        }
        text.text = string.Join(" ", phrase);
    }
    #endregion
}
