# UnityPacmania
A near perfect clone of the arcade game Pacmania done in the Unity game engine.  This version includes all original levels, cut-screens and menus.

Because this a Unity game, it can work on Windows, Mac, Linux and will soon be playable on Android and iOS.  I’ve only tested this on Windows and Android so far.

![App Screenshot](https://www.photovidshow.com/pacmania/pacmania.jpg)

## How-to-setup
This project requires:-
 
 - Unity (I used version 2019.3 but other versions will be fine).
 
 - C# code editor (I used Visual Studio community 2019).
 
 Download the project files and extract onto your PC.
 
 Start Unity and in the Unity hub window select 'Add'.  Then add the project by choosing the 'UnityPacmania' folder.
 
## Playing the game
Currently this game only works on keyboard with the arrow keys used to move pacman and the spacebar to jump.

To start a new game press the spacebar.

Other keys (when in game):-
  f1 = Invincible on.
  f2 = Invincible off.
  f3 = Infinite lives on.
  f4 = Infinite lives off.
  f5 = Finish level now.

## Technologies
This game was created with Unity and C#.

## Motivation
This project exists simply for tutorial and educational purposes to help people learn how to create a classic 2D arcade game in Unity and C#.  This project focuses directly on the programming side and also using the Unity editor.  All the game media files are simply a copy from the original and added to the project.

To take advantage of this tutorial, it is presumed that a basic understanding of Unity and C# programming is required.

I also created this project to challenge myself to work out how I could use Unity to create and old arcade game.

Although this game is a near perfect clone of the arcade version, there are small differences:-

- 2 player mode is not included.

- No need for the 'Insert coin' type logic.

- The arcade version gets brutally hard very quickly, so the difficulty increases much slower in this version.

- ‘Continue game?’ option is not included (it was simply there to encourage the player to add more coins).

- Reverse engineering the AI for the ghosts was difficult, so I did a best approximation which worked well.  A lot of the AI is based on how the original pacman worked.  This is well documented online.

- The Hi score table is currently not persistent.


