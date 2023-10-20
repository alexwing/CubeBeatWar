# Cube Beat War

Cube Beat War allows players to cast the magical Kame Hame Ha with their hands in Mixed Reality. To do this, players must perform specific hand gestures, such as bringing their palms together and then opening them to generate magic, and pushing forward to cast it. The game includes a menu and a scoring system, and consists of eliminating colored cubes that appear randomly in the real environment. The game uses a gesture detection library that allows players to interact with the menu using the finger pointing gesture, as if it were a laser pointer. The game ends when all the cubes are eliminated. The application uses the Oculus XR plugin and requires Unity 2022.3.7f1 and an Oculus Quest 3 to work.

[![IMAGE ALT TEXT HERE](https://img.youtube.com/vi/mweYGYzR3dk/0.jpg)]([https://youtu.be/XJ0JKzzmJ5M](https://www.youtube.com/watch?v=mweYGYzR3dk))

## How to play

- Show/hide menu: Perform horns gesture with right hand.
- Cursor: Make a pointing gesture with the index finger.
- Pressing palms together and then opening them generates the magic.
- Pushing forward launches the magic.
  
## New implementations in this development

- Universal render pipeline migration.
- Backed and URP materials from SpaceShip model.
- Modified reused shaders to work in VR and URP.
- The gesture recognition script now has the ability to set a time interval for the gesture to be recognised. This allows to control false positives.
- Game menu via dynamic script.
- Level creation Script
- Scores and game pause
- Mixed Reality integration using Oculus Passthrough API
- Cube spawning based on real-world surfaces detection
- Collision detection between magic and cubes


## Reused from previous projects

- Gesture recognition script
- Kame Hame Ha script to perform magic through gestures.
- Shaders modified and adapted by me.
- Texture and materials of the hands from photos of my hand.
- Own library of utilities

## Plugins used

- Plugin Oculus XR
- Spaceship
- 52 Special Effects Pack

## Requirements

* Unity 2022.3.7f1
* Oculus Quest 3
