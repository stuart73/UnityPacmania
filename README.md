# UnityPacmania
A near perfect clone of the arcade game Pacmania done in the Unity game engine.  This version includes all levels, cut-screens and menus.

Because this a Unity game, it can work on Windows, Mac, Linux and Mobile.  I’ve only tested this on Windows and Android so far.

![App Screenshot](https://www.photovidshow.com/pacmania/pacmania.jpg)

## How-to-setup
This project requires:-
 
 - Unity (current version uses 2021.2).
 
 - C# code editor (I used Visual Studio community 2019).
 
 Download the project files and extract onto your PC.
 
 Start Unity and in the Unity hub window select 'Add'.  Then add the project by choosing the 'UnityPacmania' folder.
 
Once the project is loaded into Unity, either 'Build and Run' the game or to play the game in the Unity editor, select the 'Scenes' and then 'Menus' folder and then load the 'Logo' scene.  This is the game’s initial scene.  Alternatively you can play any level by loaded the desired scene which starts with the name ‘level’.

## Playing the game
To control on keyboard use the arrow keys and the spacebar to jump.  Joystick support is also done.  For mobile use swipe and tap actions.

To start a new game press the spacebar, joystick trigger button or tap the screen for mobile.

Other keys (when in game):-
  f1 = Invincible on.
  f2 = Invincible off.
  f3 = Infinite lives on.
  f4 = Infinite lives off.
  f5 = Finish level now.

## Technologies
This game was created with Unity and C#.

## Motivation
This project exists simply for tutorial and educational purposes to help people learn how to create a classic 2D arcade game in Unity and C#.  This project focuses directly on the programming side and also using the Unity editor.  The programming is done in a modern C# style using unity's component model.  All the game media files are based from the original and added to the project.

To take advantage of this tutorial, it is presumed that a basic understanding of Unity and C# programming is required.

I also created this project to challenge myself to work out how I could use Unity to create and old arcade game.

Although this game is a near perfect clone of the arcade version, there are small differences:-

- 2 player mode is not included.

- No need for the 'Insert coin' type logic.

- The arcade version gets brutally hard very quickly, so the difficulty increases much slower in this version.

- ‘Continue game?’ option is not included (it was simply there to encourage the player to add more coins).

- Understanding the original arcade ghost AI was difficult, so I did a best approximation which worked well.  A lot of the AI is based on how the original pacman worked.  This is well documented online.


