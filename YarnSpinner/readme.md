# Yarn Spinner Tutorial

This is a brief intro-level tutorial on how to implement dialog system using Yarn Spinner.

## Introduction

Quote their website, "Yarn Spinner helps you build branching narrative and dialogue in games."

It's indeed a very versatile, minimalist tool that grants the user all the liberty in how to use the dialog content.
The framework provides an interface to iterate through each line in a tree-structure of text blocks, with automated branching and command invocation.

This tutorial will teach you how to implement a simple dialog system like ones seen in typical rpg games. However, the same knowledge should be applicable in any genre that needs branching dialog.

## Preparation

To follow along, please use the unity project contained in the subfolder Yarn_Tutorial_Starter. 

It can be opened in any LTS version of Unity under 2020, just select yes when a change version prompt pops out.
The contents include an imcomplete scene where player controller and collision have been implemented with a unhooked dialog UI dangling in a canvas. We'll start from here and integrate Yarn Spinner into the game step by step.
The existing parts will not be covered in this tutorial, but feel free to read it if you want to know how it works.

For a complete version, one that you'll get after finishing the tutorial, open Yarn-Sample.

## Downloading Yarn Spinner

(read their official guide if you want to skip this section: https://yarnspinner.dev/docs/unity/installing/)

The very first step. You can't use any part of the framework without downloading it.
If you are using a 2019.3 LTS version of Unity or newer, the best way to do it is to import it through package manager.
Go to package manager, and click the + sign on top left. In the drop down, select "Add package from Git URL". In the box, enter this following URL: https://github.com/YarnSpinnerTool/YarnSpinner-Unity.git. 

For older versions, please refer to the website above, and please consider upgrade to a newer version like 2020.x for the new features and more stable interface of Yarn Spinner.

## Pulling Components into the Scene

Now go to the BlankSlate scene in Assets/Scenes, and open it.
Take a minute to familiarize with the scene, then create an empty object and attach to it these components: DialogueRunner, DialogueUI, InMemoryVariableStorage (we'll cover this later).

Now it's time to hook them up with the existing objects.
For Dialogue Runner, drag the other 2 components to their corresponding boxes. Then set Language to "English" (spelling and case must match).
Language setting is not necessary English, we are using it because when creating Yarn scripts, the default language set by editor is English.
You can change it to something else, it's used as a string literal into a hashtable of localized text lines, i.e. you can use whatever language name as long as the script and game has the same name string.
The last thing on Runner is to toggle start automatically off.
If it's on, DialogRunner will try to start dialogue on load, and in this demo we don't want that to happen.
We'll not use node delegations here, but it's good to know that throughout these 3 points in a node's life cycle, you can do things with knowledge of which node is in question.

Next up is DialogueUI.
Find there's a Dialog UI object in the scene, drag that into the "Dialog Container" field.
This field determines which object is toggled on/off when the dialog is on/off, in our example it's just the portion of UI responsible of showing the dialog.
Text Speed determines how fast the typewritter effect poops out the characters, we'll just leave it here for now.
Then, drag the 4 Button objects under Dialog UI/Choice Buttons into the list field "Option Buttons". The slot only takes buttons.
DialogueUI component will use them to show choices.
The number also matters, so don't put more than 4 choices in your script if there're only 4 choice buttons.
The button objects don't need any special scripting/delegation.
DialogueUI will take over their clicking logic and the first Text child under each Button.
Therefore, when it's time to show choice, choice text is shown in the child Text object, and clicking event on the button will tell Yarn framework which choice is taken.

Down the line on DialogueUI you'll see a series of events.
These are the pipeline/life cycle of an entire dialogue session.
In general, events will happen in this order:

1) Dialogue Container is turned on;

2) On Dialogue Start;

3) Line / Option / Command events are mixed together depending on your script;

4) On Dialogue End;

5) Dialogue Container is turned off;

For now, just turn off PlayerController on Player when Dialogue starts and turn it back on when dialogue ends.

We'll cover variable storage later, so skip it for now.

## Writing Dialogue in Yarn

// TODO

## Dialogue Controller

Having hooked the UI components into Yarn's components, we now need to write a dialogue controller for the custom logic.
Remember that Yarn merely "informs" your Mono runtime what's happening within the script, and everything else needs custom logic.
The steps here are my approach in designing the system, and you should feel free to experiment with what you see fit when putting Yarn Spinner into your own project.

First, create a new class (it's called TextField in my sample code but feel free to name it whatever), and make it a singleton.

To make a class singleton, add the following to the class definition:

```cs

public static TextField instance = null;

private void Awake() {
    if (instance == null)
        instance = this;
    else if (instance != this)
        Destroy(gameObject);
}

```

Reason for having this singleton is that we'll later have NPC scripts use its instance to communicate with DialogRunner we just created.

Now let's go over the essential API and delegations of Yarn.

First off is `DialogueRunner.Add(YarnProgram)`.
This method adds a Yarn program (i.e. yarn script that's imported into Unity) to the DialogueRunner instance, very useful if you want NPCs to hold their dialogue script individually (like what we do in this demo).

Another one that's gonna be used is `DialogueRunner.StartDialogue(string)`, which literally starts a node whose name is specified by the string parameter.

Then give it these public functions and fields:

```cs

[SerializeField]
TextMeshProUGUI text;

[SerializeField]
DialogueRunner runner;

public void ShowLine(string line)
{
    text.text = line;
}

public void RegisterSpeaker(YarnProgram script)
{
    runner.Add(script);
}

public void StartDialog(string node)
{
    if (!inDialog)
        runner.StartDialogue(node);
}

```
