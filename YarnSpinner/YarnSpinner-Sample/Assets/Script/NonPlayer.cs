using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class NonPlayer : MonoBehaviour
{
    [SerializeField]
    YarnProgram script;

    [SerializeField]
    string startingNode = "";

    [SerializeField]
    Sprite pfp;
    [SerializeField]
    string npcName;

    private void Start()
    {
        TextField.instance.RegisterSpeaker(script, npcName, pfp);
    }
    
    /// <summary>
    /// Note: Yarnspinner doesn't stop u from playing a dialog again and again, single-time dialogs are done through code.
    /// </summary>
    public void TalkTo()
    {
        TextField.instance.StartDialog(startingNode);
    }
}
