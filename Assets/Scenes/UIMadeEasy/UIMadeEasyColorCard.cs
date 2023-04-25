using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMadeEasyColorCard : MonoBehaviour
{
    [SerializeField] RawImage rawImage;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Button updateButton;

    // delayed so onvalidate only gets called on it after you press enter
    [Delayed]
    [SerializeField] string defaultHex = "";

    void OnValidate()
    {
        ///TODO: Validate defaultHex
    }

    void Reset()
    {
        ///TODO: Assign appropriate values to the serialized fields
    }

    void Start()
    {
        if (updateButton)
        {
            updateButton.onClick.AddListener(OnUpdateButtonClick);
        }

        if (inputField)
        {
            inputField.text = defaultHex;

            ///BONUS: Create a function here that validates the input at runtime!
            ///This function takes in a character to be added at a certain position
            ///and returns what character should be added instead, with '\0' meaning no character
            //inputField.onValidateInput += (text, charIndex, charAdded) =>
            //{
            //    return '\0';
            //};
            ///Note: you can also use onValueChanged (which might be better b/c onValidateInput doesn't
            ///get called when you delete a character)
        }
    }

    void OnUpdateButtonClick()
    {
        if (inputField && rawImage)
        {
            // converts hex to unity color
            if (ColorUtility.TryParseHtmlString(inputField.text, out var color))
            {
                rawImage.color = color;
            }
        }
    }
}
