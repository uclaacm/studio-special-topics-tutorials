# Studio Beginner Tutorials - Roll a Ball Part 1

**Date**: October 12, 2021<br>
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
* [C# and Unity Features](#csharp-and-unity-features)
  * Coroutines
  * ```static``` Keyword
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
## CSharp and Unity Features
For this section, you can follow along and code in the Programming Essentials Unity project this README is located within. We will be demonstrating the use of various C# and Unity engine features by coding scripts for a typewriter effect, settings to store the text delay between displaying characters for the typewriter effect, and a slider to change the text delay.

---
## Essential Links
- [Studio Discord](https://discord.com/invite/bBk2Mcw)
- [Linktree](https://linktr.ee/acmstudio)
- [ACM Membership Portal](https://members.uclaacm.com/)

## Additional Resources
- [Unity Documentation](https://docs.unity3d.com/Manual/index.html)
- [ACM Website](https://www.uclaacm.com/)
- [ACM Discord](https://discord.com/invite/eWmzKsY)
