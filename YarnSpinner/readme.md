---
**IMPORTANT WARNING**

Yarn Spinner released a major update in December 2021, going from version 1.2 to 2.0.
This tutorial is outdated and is not applicable to any 2.0+ versions of Yarn Spinner.

---

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

Pay extra attention to `On Line Update`, as this is the exact interface for your gameplay code to read in dialogue lines.

## Writing Dialogue in Yarn

I recommend using their official online editor provided by Yarn Spinner. You can find it here: https://yarnspinnertool.github.io/YarnEditor/.
It provides syntax highlighting and graphical representation of node structure, and it's what we'll use in this tutorial.
For other workflow options like raw text editing with VSCode extension, please refer to this guide: https://yarnspinner.dev/docs/writing/text-editor/

(There's a small bug with the editor, if for some reason the editor opens without anything but the background, doing a force refresh with ctrl+shift+r to make it reload should resolve the problem)

If you go to File -> Settings, you should see the following:

![image](https://user-images.githubusercontent.com/39484269/139951069-c4e542bf-c2dd-481d-8c7d-0f30d9a9aa96.png)

It means your script here is in English. A script can only have one language setting.

For now, let's just write a couple of plain text nodes connected by options.
First double click on the empty node automatically generated on opening, and change its title to NPC1.Start (in Yarn runtime, node names are game-scope global instead of local inside a script, i.e. if 2 scripts have the same "Start" node and they both got added to DialogueRunner, bad things happen, thus make sure to give some extra information in each node name to avoid potential crash).

Then type these:

```
NPC1: Hello
[[Greet | Greeted]]
[[Say nothing | Been_rude]]
```

The first line `Hello` is a "plain text" line, where DialogueUI feed to functions in `On Line Update` event.
The other 2 are "Options", ones that will be displayed using the buttons we just hooked in DialgueUI, in the same order as you put in the script.
Inside the options there are 2 parts: text before `|` is the display text, where it's going to show up on the button; text on the right is node name, where DialogueRunner (and the editor) searches for the name.
Note that node name needs to be one word, i.e. don't put space in it otherwise Yarn will get angry and refuse to load the file as Yarn Program.

Now when you hit ESC to go back, and you should see 2 additional nodes have been created.
This might look familiar if you have experience working with Twine, and it's indeed the editor generating new nodes based on the options in the previous node.

Then go to `Greeted`, and put these:

```
Wassup?
NPC1: See ya.
See ya.
```

And for the other,

```
...
NPC1: ...?
NPC1: See ya ... I guess.
```

Finally, save the script. Simply do ctrl+s should work, or you can go to File menu and click "Save as Yarn":

![image](https://user-images.githubusercontent.com/39484269/139950752-0055ca04-98d6-46a7-af68-3d7958d665a0.png)

Drag that into your Unity editor and let Yarn transform it into a `Yarn Program`. We'll use this special type of text asset later.

## Dialogue Controller

We'll be writing code starting here, and all the code you need to write are already in the files and commented out, please use that as reference when you lose track of this guide.

Having hooked the UI components into Yarn's components and a very simple script done,, we now need to write a dialogue controller for the custom logic.
Remember that Yarn merely "informs" your Mono runtime what's happening within the script, and everything else needs custom logic.

Note that the steps here are my approach in designing the system, and you should feel free to experiment with whatever you see fit when putting Yarn Spinner into your own projects.

First, create a new class (it's called `TextField` in my sample code cuz it sounds cool but feel free to name it whatever, just make everything consistent), and make it a singleton.

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

With that knowledge, we can add these fields and methods:

```cs
[SerializeField]
TextMeshProUGUI text;

[SerializeField]
DialogueRunner runner;

public bool inDialog { get; set; } = false;

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
    runner.StartDialogue(node);
}

private void Update()
{
    if (inDialog && Input.GetKeyDown(KeyCode.Space))
    {
        runner.Dialogue.Continue();
    }
}
```

I'll explain these one by one.

`ShowLine` sets the incoming text content to `text` which is a TextMeshPro object. You can also swap it with anything that can show text.

`RegisterSpeaker` is called to register a NPC and its corresponding script.

`StartDialogue` is a proxy to `runner.StartDialogue`, for now it doesn't have additional functionality but we'll add stuff later.

What's in the Update function is a call to DialogueRunner to continue to next line.
DialogueRunner does give this decision to gameplay code, and you can also set the trigger to something else, like a player event.

This is all we need for the controller for now.

## Hooking Controller into the Game

Next, let's hook the controller by adding the controller to correct slots in DialogueUI's delegations.
Before the later steps, attach the controller script from previous section to the object with yarn components, and assign the TMP box in canvas to it.
Drag DialogueRunner component to the field on controller component.

Go to DialogueUI component we created earlier, find the delegation box saying `On Line Update`, add a new delegation and drag the controller in.
Then in function selection, find a section saying "dynamic string" and find the `ShowLine` function.
This delegation will keep the text box updated as the dialogue lines are rolling.
Then add setter of `inDialog` boolean to dialogue start and end delegations, set them accordingly so it's true when dialogue is active and false when dialogue is inactive.

Then we'll need to change the NPC script.
Go to the NPC obejct in scene, and open the NonPlayer component attached to it.
Add the following fields:

```cs
[SerializeField]
YarnProgram script;

[SerializeField]
string startingNode = "";
```

Add the following code to `Start`:

```cs
TextField.instance.RegisterSpeaker(script, npcName, pfp);
```

This line of code finds the singleton instance of the controller class, and registers its script to it. The controller will then put the script in DialogueRunner.

Then add these in `TalkTo` method:

```cs
TextField.instance.StartDialog(startingNode);
```

`TalkTo` is called when player gets close to NPC and presses Space (hard coded in `PlayerController`, it's bad habbit hard coding things don't do it in actual projects).
Here we are adding the actual logic of "talking" to our NPC.

Now go back to the editor and hit play, the dialogue should correctly show up as you use WSAD to approach an NPC and press Space to talk.
Next up we'll go over more advanced functionality of Yarn, Commands, by implement a profile picture swapping mechanism using Yarn commands.

## Commands

Commands are ways your Yarn script/program talk to Monobehaviours.
To add a command in Yarn script, put something like `<<CommandName arg1 arg2>>`.
When Yarn sees something enclosed in double angle-brackets, it knows it's a command and will feed it to the command workflow instead of that we discussed for line.

Now let's get some code to work with that.
First, add this function in your controller class:

```cs
[SerializeField]
Image pfpBox;

Dictionary<string, Sprite> nameToPfp = new Dictionary<string, Sprite>();

private void HandleSetPic(string[] args)
{
    if (!args || args.Length != 1)
        throw new UnityException("Error: Command 'SetPic' should have 1 argument, came with " + args.Length.ToString() + " args.");

    string name = args[0];
    if (!nameToPfp.ContainsKey(name))
        throw new UnityException("Error: Command 'SetPic' has unknown name '" + name + "'.");

    pfpBox.sprite = nameToPfp[name];
}
```

Also change the register function:

```cs
public void RegisterSpeaker(YarnProgram script, string name = "", Sprite pfp = null)
{
    runner.Add(script);

    if (pfp && name != "")
    {
        nameToPfp.Add(name, pfp);
    }
}
```

Adding the default values allows your code to compile before you fix every instance of usage.

An Image element is added to the class, and the HandleSetPic function sets the Image to a sprite, from its dictionary of pictures.
Take a look into the handling logic: `args` is an array containing all the arguments in the original command line.
For example, using the previously written command, `<<CommandName arg1 arg2>>`, `args` would be `{"arg1", "arg2"}`.
Take special note though, if there are no arguments, the args input from Yarn will be `null` instead of empty,
and bad things happen if you try to read the length of null.

Here you can see the error messages mentioning a command "SetPic", which is exactly what we'll do next. Add this line in `Start` of your controller class (remember `runner` is a DialogueRunner instance assigned from editor):

```cs
runner.AddCommandHandler("SetPic", HandleSetPic);
```

This line will tell the DialogueRunner instance "let `HandleSetPic` function handle all commands named "SetPic".
Note here I'm enforcing a fixed number of arguments for the command here, but if you want you can also make it dynamic, just handle all the possible cases in your handler function.

Before the next step, let's make a little changes to NPC script so the pfp dictionary actually gets content.
Go to `NonPlayer` and add these:

```cs
[SerializeField]
Sprite pfp;
[SerializeField]
string npcName;

private void Start()
{
    TextField.instance.RegisterSpeaker(script, npcName, pfp);
}
```

This will allow NPCs to set name and pfp on Start, thus when a command to show their face shows up, the controller knows what to put.

Now we can put the commands in the Yarn script.
Remove all the "NPC1: " and instead put `<<SetPic NPC1>>` before these lines said by NPC.
The starting node will look like this:

```
<<SetPic NPC1>>
Hello
[[Greet | Greeted]]
[[Say nothing | Been_rude]]
```

Same thing happens for the other nodes.

Before starting the game, remember to set NPC1's name and picture.
The name needs to match the argument after "SetPic" exactly.
After doing that, booting up the game you should be able to see the picture set to NPC1's picture correctly.

## Variable

I'll only cover the high-level of variable storage here.
Basically there are a set of special commands in Yarn that lets you set and read variables.
They are usually interpreted as "saved progress".
For example, you might want the story to remember a trivial branch (ones using `->` instead of branching).
You can also hook it to a serialization module (not provided by Yarn Spinner), and use that to save the game, but that alone deserves another full tutorial.

Details can be found here: https://yarnspinner.dev/docs/unity/components/variable-storage/

## End

With that concludes this tutorial on Yarn Spinner.
Be sure to experiment with different designs using Yarn Spinner, as my way of using it might not be the best practice.
There are also more features of Yarn Spinner that are kind of specific (like variable storage), and be sure to go on their website if you don't know how to do something.
There might be something on there.
