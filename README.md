# UNITY SFX MANAGER
### by Varollo
------------------------------------------------------------
## What is it?
Unity SFX Manager is a Sound Manager for 2D sounds, like Background Music and Sound Effects, that uses a Scriptable Object based manager, meaning you don't need any instances of it on your scene.

## How to install it?
You can clone this repository or add it to your project with Unity's Package Manager if you have git installed on your machine.

## How to use it?
Somewhere in your project, right click and go to:
> Create/Varollo/SFX Manager/New SFX Mananger

This will create a instance of the ```ScriptableSFXManager``` class.
Inside it you can create multiple ```SFXTrack``` objects, and for each, multiple ```Sound```  objects.

**To play a sound** from a track you can call ```Play(trackName, soundName)```, or you can call with one single parameter ```Play(track_sound)```.
```track_sound``` being a string with the track name and sound name separated by a ```_``` character.
This means a **tracks cannot be named with ```_``` character**.
Sounds don't have that problem.

Once you call ```Play``` on that instance of the Manager, if not already, a new ```AudioSource``` will be instantiated in your scene for the playing track, and the sound parameters will be loaded into that ```AudioSource```.

The created object is persistent between scenes, and will only be recreated if it ever get's destroyed.

## Disclaimer
I am not supporting this that much, it's just a thing for me to use in multiple projects, but none the less, you can use it if you don't mind the lack of support from my side.

I also plan on making a custom editor for it in the future, but don't get your hopes to high.