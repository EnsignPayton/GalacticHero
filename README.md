# Galactic Hero

Galactic Hero is an arcade-inspired 2D action shooter based on Daniel Remar's [Hero](http://remar.se/daniel/hero.php), precursor to the fantastic [Hero Core](http://remar.se/daniel/herocore.php). All assets are either created by me or taken from Hero or Hero Core's GameMaker source code, found [here](http://remar.se/daniel/resources.php).

## Running

This project is written for the free version of Unity 2017.3.1f1. You'll need Unity installed of that version or higher. With Unity installed, just open [Main](Assets/_Scenes/Main.unity) and you should be good to go.

## Script Design

Scripting on this project was parially an excercise on inheritance. All scripts which can be added to a GameObject inherit from [Script](Assets/Scripts/Script.cs), which wraps MonoBehaviour in a nice OOP way. From here, all objects that can move and have a health total derive from [Entity](Assets/Scripts/Entity.cs).
