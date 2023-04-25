//#define SOLUTION_USE_REGEX
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class UIMadeEasyColorCardSolution : MonoBehaviour
{
    [SerializeField] RawImage rawImage;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] Button updateButton;

    [Delayed]
    [SerializeField] string defaultHex = "";

    void OnValidate()
    {
        if(defaultHex.Length > 0)
        {
#if SOLUTION_USE_REGEX
            // Regex Solution
            if (!Regex.IsMatch(defaultHex, "^#([0-9a-fA-F]{3}|[0-9a-fA-F]{6}|[0-9a-fA-F]{8})$"))
            {
                defaultHex = "";
            }
#else
            bool isValid = false;
            if (defaultHex[0] == '#') {
                // only #fff #ffffff and #ffffffff are valid
                if (defaultHex.Length == 4 || defaultHex.Length == 7 || defaultHex.Length == 9)
                {
                    bool foundInvalidCharacter = false;
                    for(int i = 1; i < defaultHex.Length; ++i)
                    {
                        char ch = char.ToLower(defaultHex[i]);
                        if (!char.IsDigit(ch) && !(ch >= 'a' && ch <= 'f'))
                        {
                            foundInvalidCharacter = true;
                            break;
                        }
                    }

                    if (!foundInvalidCharacter)
                    {
                        isValid = true;
                    }
                }
            }

            if (!isValid)
            {
                defaultHex = "";
            }
#endif
        }
    }

    void Reset()
    {
        rawImage = GetComponentInChildren<RawImage>();
        inputField = GetComponentInChildren<TMP_InputField>();
        updateButton = GetComponentInChildren<Button>();
        defaultHex = "#ffffff";
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
            inputField.onValidateInput += (text, charIndex, charAdded) =>
            {
                var ch = char.ToLower(charAdded);

                if (ch == '#' && charIndex == 0 && !(text.Length > 0 && text[0] == '#'))
                {
                    return charAdded;
                }
                if (
                    (charIndex > 0 || charIndex == 0 && (text.Length == 0 || text[0] != '#'))
                    && (ch >= 'a' && ch <= 'f' || char.IsDigit(ch))
                    && (text.Length <= 7 || text.Length == 8 && text[0] == '#')
                )
                {
                    return charAdded;
                }

                return '\0';
            };
        }
    }

    void OnUpdateButtonClick()
    {
        if (inputField && rawImage)
        {
            if(ColorUtility.TryParseHtmlString(inputField.text, out var color))
            {
                rawImage.color = color;
            }
        }
    }
}
