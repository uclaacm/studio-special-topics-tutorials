# Studio Advanced Tutorials - Game Programming Essentials

**Date**: October 13, 2021<br>
**Location**: ACM Clubhouse (Boelter 2763)<br>
**Instructor**: Richard Cheng

## Resources
[Slides](https://docs.google.com/presentation/d/1L0TkCA3rF4-21-083rHygDGLCpq74LlxKdzWgMmwTaU/edit?usp=sharing)<br>
[Video]()<br>

## Topics Covered
* [Best Practices](#best-practices)
  * [Encapsulation / Information Hiding](#encapsulation-and-information-hiding)
  * [Code readability](#code-readability)
  * [Building and playtesting](#building-and-playtesting)
* [C# and Unity Features](#c-and-unity-features)
  * [Coroutines](#coroutines)
  * [```static``` Keyword](#static-keyword)
  * PlayerPrefs
  * Properties
  * Events
  * Attributes
  * Console & Debug
---
## Best Practices
### Encapsulation and Information Hiding
Information hiding is the principle of segregating design decisions that are likely to change, thus protecting other parts of your game from changes in implementation. In other words, you want to design your code such that you don't have to change other parts of your code when you make a change to one part of your code. There's nothing worse than making a small change to your PlayerController and finding out that you now have to rewrite jor sections of your game manager, camera, and UI scripts!

To achieve information hiding, we encapsulate data and methods within classes and functions. This includes marking class members as private or protected, as well as designing APIs for your classes that hides the internal implementation of that class. Here are some tips which may help you encapsulate your code:
* Before You Code:
  * **Think about what parts of your code are likely to change**, and how to segregate those sections so that other code can remain unaffected when it does change. Thinking and designing before you code can save you a lot of time and frustration later.
  * **Design and use interfaces**. In many cases, writing an interface is like writing down the encapsulated design you thought of, helping you to refine that design and to stick to that design while coding.
  * Think about how to **decompose larger functions into smaller ones**. Encapsulation isn't just limited to classes - if you have a large function, changes to one part of a function may necessitate changes to other parts of the function as well. Breaking unwieldy methods into smaller ones not only helps avoid this, but also improves readability as well.
* While You Code:
  * Explicitly **mark all class members with their access level** (public/private/protected). Not only does this help clarify which things are accessible and which are not, it also gives you another opportunity to consider whether you actually need something to be public.
  * **Write wrapper functions**. Wrapper functions can help simplify external function calls, and provide another layer of separation between implementation and useage, especially when the vast majority of your calls to a function will be using the same/similar parameters.
  * **Decompose larger functions into smaller ones**. If you find that a function you're writing is becoming too long, consider breaking it into smaller functions.

### Code Readability
Making your code easy to read and understand is important, especially when you are working with other programmers, as is often the case in game development. Code that is difficult to understand makes it harder to use that code or to make changes to it. Here are some tips to help improve readability:
* **Adopt a common code style.** Sharing the same code style as the people you are working with makes it much easier to read each others' code. In particular, having consistent indentation and a consistent naming scheme can save time and prevent confusion.
* **Use informative names.** Being able to intuit the meaning of a variable or function name saves time from having to closely read the code to understand what is going on.
* **Write useful comments.** Comments futher improve readability by providing explanations to what the code is doing. That being said, more comments is not always better - don't drown useful information in a deluge of trivial comments. And don't forget to update your comments when you make changes to the code!

### Building and Playtesting
Quality assurance is one part of game development that is often neglected until the end, even though it should be integrated as part of your continuous development cycle. Leaving your playtesting until the end results in issues like having to rewrite major sections of code to fix bugs, or discovering that your render pipeline doesn't support WebGL and you don't have time to fix it. Here are some tips for building and playtesting:
* **Playtest continuously**. Playtesting your game throughout development helps catch nasty bugs before they require significant work to fix. 
* **Make it easy to playtest**.
  * Include "debug" keys that allow you to skip sections of gameplay or test certain mechanics. You don't want to have to platform the same section over and over again in order to test the section after it, nor do you want to move your character to that later section and then forget to move it back afterwards. You can use ```#if``` directives to make sure that these debug keys only work in the editor and development builds, and not in your final build.
  * Have each scene be playable by itself (most applicable if you have ```DoNotDestroyOnLoad()``` objects). Being able to start playing from any scene allows you to skip to the section you actually want to test.
  * Have each scene by playable in sequence. Some bugs (```DoNotDestroyOnLoad()```) only manifest if you play scenes consecutively.
* **Build early and often**.
  * Although the majority of your playtesting can be done in the Unity editor, you also want to actually build your game since some code can behave differently in the editor and on different platforms.
  * Building your game and running it on different computers is also a good chance to see how your UI scales and appears on different screen resolutions.
  * Making regular builds provides a tangible progress check, and ensures you will definitely have _something_ to present.
  * If you've been building throughout development, chances are you won't run into any last-minute issues with building your game when it's done.
---
## C# and Unity Features
For this section, you can follow along and code in the Programming Essentials Unity project this README is located within. We will be demonstrating the use of various C# and Unity engine features by coding scripts for a typewriter effect, settings to store the text delay between displaying characters for the typewriter effect, and a slider to change the text delay.

When you open the project, you should see that the ```SampleScene``` has a ```Canvas``` with ```Text(TMP)``` as a child. ```Text(TMP)``` should have a ```TextMeshPro - Text``` component and a ```Typewriter``` script component attached. If you play the scene, you should see that the text prints out a message one character at a time. Okay, we're done here, this tutorial is finished!

Just kidding! Let's open up the ```Typewriter``` script (located in ```Assets/Scripts```). Take a moment to read through it and understand how it implements the typewriter effect. What issues does this implementation have?

<details>
 <summary>Implementation Issues</summary>
 <ul>
  <li>The typewriter effect always starts at the start of the scene, and it's not simple to add a delay before it starts either.</li>
  <li>The typewriter effect only runs once. If we wanted to reset the typewriter, we would need to change quite a few variables.</li>
  <li>The typewriter only displays the same line, although we could add code to change the exampleLine.</li>
  <li>The typewriter continues to make an if check every Update(), even after it's done printing out, slowing down the game.</li>
  <li>The code is poorly encapsulated - fixing any of these issues requires understanding the implementation and/or making a large number of changes.</li>
 </ul>
</details>

Clearly there's a lot of room for improvement! To make these improvements, we are going to turn to Coroutines.

### Coroutines
Coroutines are feature provided by the Unity engine that allow you to pause execution of a function, thus returning control to Unity, and then resume execution of that function at a later point. For our typewriter effect, for example, we can write a coroutine that adds one character to the text, pauses itself for the duration of the delay while other things happen in-game, and repeats until the entire line has been displayed.

To write a coroutine, write a function that returns an ```IEnumerator```. You don't need to know what an ```IEnumerator``` is to use coroutines, but you can read about them in the [C# documentation](https://docs.microsoft.com/en-us/dotnet/api/system.collections.ienumerator). Inside this function, when you want to pause, you can use ```yield return x``` where ```x``` is one of Unity's yield instructions. Yield instructions include ```yield return null```, which waits until the next frame, ```yield return new WaitForSeconds(float time)```, which waits for approximately some number of seconds, or even another coroutine! You can find more yield instructions in the [Unity documentation](https://docs.unity3d.com/ScriptReference/YieldInstruction.html), or code your own [CustomYieldInstruction](https://docs.unity3d.com/ScriptReference/CustomYieldInstruction.html). If you want to end a coroutine before it reaches the end of the function, you can also use ```yield break```. The code below shows an example of a coroutine, which logs "Hello World!" every second for ```times``` seconds.

```c#
public IEnumerator ExampleCoroutine(int times)
{
  for (int i = 0; i < times; i++)
  {
    Debug.Log("Hello World!");
    yield return new WaitForSeconds(1);
  }
}
```

Once you have a coroutine function, you can start the coroutine by using ```StartCoroutine()```. You can also stop a coroutine early from outside the coroutine by using ```StopCoroutine()```. The example below shows starting the ```ExampleCoroutine()``` and then immediately stopping it.

```c#
public void StartStopExample()
{
  Coroutine example = StartCoroutine(ExampleCoroutine(3));
  StopCoroutine(example);
}
```

Rewrite ```Typewriter.cs``` to use a coroutine instead, and add a wrapper function so that we can start the coroutine again whenever we want to. Finally, create a button in the scene and add your wrapper function to the ```onClick``` so that the button will restart your typewriter effect whenever you press it! An example solution can be found in ```Assets/Scripts/Solutions/CoroutineTypewriter.cs``` if you are stuck or confused. Look how much simpler, easier to understand, and better encapsulated your new typewriter script is! There's just one problem (unless you already accounted for it, in which case good job!) - do you notice it?

<details>
 <summary>One Small Issue</summary>
 Play the scene and press the button repeatedly. Notice how the text is messed up? That's because we started more coroutines that are editing the text, without stopping the ones that came before! The coroutines are competing with each other to edit the text. To fix this, add a StopCoroutine() call to your wrapper function. (I hope you see why wrapper functions are great for encapsulation!)
</details>

### ```static``` Keyword
Our typewriter script is looking good so far, but what if we want to change the delay between characters appearing at runtime? In fact, what if we want to change the delay for **all** of our typewriter scripts at once, and have it persist between scenes or event game sessions? In other words, we want to have Text Delay setting that the player can change.

We can use ```static``` to solve most of these problems. (Note: For those of you who have programmed in ```C``` and ```C++```, note that in ```C#``` ```static``` only has a subset of the meanings it has in ```C``` and ```C++```.) In ```C#```, static can be written before a member of a class (variable or method) to indicate that the member belongs to the type (class), rather than an instance of that type/class. To access these ```static``` members, you use the type name rather than the name of an instance of that class. An entire class can also be marked as ```static```. A ```static``` class consists of only ```static``` members, and cannot be instantiated. Instead, it is globablly acessible.

Create a static Settings class to hold the value of the text delay. An example is shown below, and note that it does not inherit from MonoBehaviour.
```c#
public static class Settings
{
  public static float TEXT_DELAY = 0.05f;
}
```

With this Settings class, we can now access the global value of text delay from any script by using ```Settings.TEXT_DELAY```. Edit your Typewriter script to use this global value. Also create a UI slider in the scene, and write and attach a script that will set the global value of TEXT_DELAY on the slider's ```onValueChanged```. This slider script should also set the starting value of the slider to ```Settings.TEXT_DELAY.```

---
## Essential Links
- [Studio Discord](https://discord.com/invite/bBk2Mcw)
- [Linktree](https://linktr.ee/acmstudio)
- [ACM Membership Portal](https://members.uclaacm.com/)

## Additional Resources
- [Unity Documentation](https://docs.unity3d.com/Manual/index.html)
- [ACM Website](https://www.uclaacm.com/)
- [ACM Discord](https://discord.com/invite/eWmzKsY)
