using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CalculatorSolution : EditorWindow
{
    enum Operation
    {
        Add,
        Sub
    }

    TextField curOutputField;
    TextField lastOutputField;

    decimal? curValue = null;
    decimal? lastValue = null;

    // NOTE: m suffix is for decimals (higher "precision" than double)
    decimal place = 1.0m;
    int decimalDigits = 0;
    Operation? operation = null;

    [MenuItem("Game/Calculator Solution")]
    static void ShowWindow(MenuCommand menuCommand)
    {
        GetWindow<CalculatorSolution>();
    }

    void CreateGUI()
    {        
        List<VisualElement> rows = new List<VisualElement>();
        int buttonNumber = 9;
        for (int rowNum = 0; rowNum < 4; ++rowNum)
        {
            var row = new VisualElement();
            row.style.flexGrow = 1;
            row.style.maxHeight = 50;
            rows.Add(row);

            if (buttonNumber > 0)
            {
                // reverse order, since it goes 7 8 9, not 9 8 7
                for (int num = buttonNumber - 2; num <= buttonNumber; ++num)
                {
                    // make copy so lambda doesn't capture num by reference
                    int numCopy = num;
                    var button = new Button(() => OnNumericButtonPress(numCopy));
                    button.text = num.ToString();
                    // make all buttons have equal width and grow to width of screen
                    button.style.flexGrow = 1;
                    button.style.flexBasis = 0;
                    row.Add(button);
                }
            }
            else
            {
                var button = new Button(() => OnNumericButtonPress(0));
                button.text = "0";
                button.style.flexBasis = 0;
                button.style.flexGrow = 2;

                row.Add(button);
            }
            // next row
            buttonNumber -= 3;

            // add other buttons
            if(rowNum == 0)
            {
                var button = new Button(OnClearButtonPress);
                button.text = "C";
                button.style.flexBasis = 0;
                button.style.flexGrow = 1;
                row.Add(button);
            }
            else if(rowNum == 1)
            {
                var button = new Button(OnPlusButtonPress);
                button.text = "+";
                button.style.flexBasis = 0;
                button.style.flexGrow = 1;
                row.Add(button);
            }
            else if (rowNum == 2)
            {
                var button = new Button(OnMinusButtonPress);
                button.text = "-";
                button.style.flexBasis = 0;
                button.style.flexGrow = 1;
                row.Add(button);
            }
            else if (rowNum == 3)
            {
                var decimalButton = new Button(OnDecimalButtonPress);
                decimalButton.text = ".";
                decimalButton.style.flexBasis = 0;
                decimalButton.style.flexGrow = 1;
                row.Add(decimalButton);

                var enterButton = new Button(OnEnterButtonPress);
                enterButton.text = "=";
                enterButton.style.flexBasis = 0;
                enterButton.style.flexGrow = 1;
                row.Add(enterButton);
            }

            row.style.flexDirection = FlexDirection.Row;
            rootVisualElement.Add(row);
        }

        curOutputField = new TextField();
        curOutputField.SetEnabled(false);

        lastOutputField = new TextField();
        lastOutputField.SetEnabled(false);

        UpdateOutputText();

        rootVisualElement.Add(lastOutputField);
        rootVisualElement.Add(curOutputField);
    }

    void OnClearButtonPress()
    {
        if(curValue != null)
        {
            ClearCurVal();
        }
        else if(lastValue != null)
        {
            lastValue = null;
        }
        UpdateOutputText();
    }

    void OnPlusButtonPress()
    {
        if (curValue != null)
        {
            OnEnterButtonPress();
            operation = Operation.Add;
        }
        else if (lastValue != null)
        {
            operation = Operation.Add;
        }

        UpdateOutputText();
    }

    void OnMinusButtonPress()
    {
        if (curValue != null)
        {
            OnEnterButtonPress();
            operation = Operation.Sub;
        }
        else if (lastValue != null)
        {
            operation = Operation.Sub;
        }
        else
        {
            curValue = 0;
            OnEnterButtonPress();
            operation = Operation.Sub;
        }
        UpdateOutputText();
    }

    void OnEnterButtonPress()
    {
        if(operation != null && curValue != null)
        {
            if(operation == Operation.Add)
            {
                lastValue += curValue;
            }
            else
            {
                lastValue -= curValue;
            }
            ClearCurVal();
            operation = null;
        }
        else if(curValue != null)
        {
            lastValue = curValue;
            ClearCurVal();
        }
        UpdateOutputText();
    }

    void OnNumericButtonPress(int value)
    {
        if(curValue == null)
        {
            curValue = value;
        }
        else
        {
            if(place < 1m)
            {
                curValue += value * place;
                place /= 10;
                decimalDigits++;
            }
            else
            {
                curValue *= 10;
                curValue += value;
            }
        }
        UpdateOutputText();
    }

    void OnDecimalButtonPress()
    {
        if(curValue == null)
        {
            curValue = 0;
        }
        // 1f is an exact power of 2, so comparison will work
        if(place == 1.0m)
        {
            place = 0.1m;
        }
    }

    void ClearCurVal()
    {
        curValue = null;
        place = 1.0m;
        decimalDigits = 0;
    }

    void UpdateOutputText()
    {
        curOutputField.value = "";
        lastOutputField.value = "";
        if (curValue is decimal curValueNN)
        {
            curOutputField.value = curValueNN.ToString(string.Format("F{0}", decimalDigits));
        }
        if(lastValue != null)
        {
            lastOutputField.value = lastValue.ToString();
            if(operation != null)
            {
                lastOutputField.value += operation == Operation.Add ? " +" : " -";
            }
        }
    }
}
