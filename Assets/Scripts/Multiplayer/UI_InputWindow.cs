using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InputWindow : MonoBehaviour
{
    private static UI_InputWindow instance;
    [SerializeField] GameObject nameInputBar;
    bool isHidden = true;

    //public void ToggleOnOff()
    //{
    //    if(isHidden) { Show(); }
    //    else { Hide(); }

    //    isHidden = !isHidden;
    //}

    //void Show()
    //{
    //    nameInputBar.SetActive(true);
    //}

    //void Hide()
    //{
    //    nameInputBar.SetActive(false);
    //}

    //private void Show(string titleString, string inputString, string validCharacters, int characterLimit, Action onCancel, Action<string> onOk)
    //{
    //    nameInputBar.SetActive(true);


    //    gameObject.SetActive(true);
    //    transform.SetAsLastSibling();

    //    titleText.text = titleString;

    //    inputField.characterLimit = characterLimit;
    //    inputField.onValidateInput = (string text, int charIndex, char addedChar) => {
    //        return ValidateChar(validCharacters, addedChar);
    //    };

    //    inputField.text = inputString;
    //    inputField.Select();

    //    okBtn.ClickFunc = () => {
    //        Hide();
    //        onOk(inputField.text);
    //    };

    //    cancelBtn.ClickFunc = () => {
    //        Hide();
    //        onCancel();
    //    };
    //}

    //private void Hide()
    //{
    //    nameInputBar.SetActive(false);
    //    gameObject.SetActive(false);
    //}

    //private char ValidateChar(string validDictionary, char testChar)
    //{
    //    if (validDictionary.IndexOf(testChar) == -1)
    //    {
    //        // Invalid
    //        return '\0';
    //    }
    //    else
    //    {
    //        // Valid
    //        return testChar;
    //    }
    //}

    //public static void Show_Static(string titleString, string inputString, string validCharacters, int characterLimit, Action onCancel, Action<string> onOk)
    //{
    //    instance.Show(titleString, inputString, validCharacters, characterLimit, onCancel, onOk);
    //}

    //public static void Show_Static(string titleString, int defaultInt, Action onCancel, Action<int> onOk)
    //{
    //    instance.Show(titleString, defaultInt.ToString(), "0123456789-", 20, onCancel,
    //        (string inputText) => {
    //            // Try to Parse input string
    //            if (int.TryParse(inputText, out int _i))
    //            {
    //                onOk(_i);
    //            }
    //            else
    //            {
    //                onOk(defaultInt);
    //            }
    //        }
    //    );
    //}
}
