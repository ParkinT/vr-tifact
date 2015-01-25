==============================================================
  Audio Toolkit v3.3 - (c) 2012 by ClockStone Software GmbH
==============================================================

Summary:
--------

Audio Toolkit provides an easy-to-use and performance optimized framework 
to play and manage music and sound effects in Unity.

Features include:

 - ease of use: play audio files with a simple static function call, creation 
    of required AudioSource objects is handled automatically 
 - conveniently define audio assets in categories
 - play audios from within the inspector
 - set properties such as the volume for the entire category
 - change the volume of all playing audio objects within a category at any time
 - define alternative audio clips that get played with a specified 
   probability or order
 - advanced audio pick modes such as "RandomNotSameTwice", "TwoSimultaneously", etc.
 - uses audio object pools for optimized performance particuliarly on mobile devices
 - set audio playing parameters conveniently, such as: 
       + random pitch & volume
       + minimum time difference between play calls
       + delay
       + looping
 - fade out / in 
 - special functions for music including cross-fading 
 - music track playlist management with shuffle, loop, etc.
 - delegate event call if audio was completely played
 - audio event log
 - audio overview window


Package Folders:
----------------

- AudioToolkit: The C# script files of the Audio Toolkit

- Shared Auxiliary Code: additional general purpose script files 
	required by the Audio Toolkit. These files might also be used 
	by other toolkits made by ClockStone available in the Unity Asset 
	Store.

- Demo: A scene demonstrating the use of the toolkit



Quick Guide:
------------

We recommend to watch the video tutorial on http://unity.clockstone.com

Usage:
 - create a unique GameObject named "AudioController" with the 
   AudioController script component added.
 - Create an prefab named "AudioObject" containing the following components: Unity's AudioSource, 
   the AudioObject script, and the PoolableObject script (if pooling is wanted). 
   Then set your custom AudioSource parameters in this prefab. Next, specify this prefab 
   as the "Audio Object Prefab" in the AudioController.
 - create your audio categories in the AudioController using the Inspector, e.g. "Music", "SFX", etc.
 - for each audio to be played by a script create an 'audio item' with a unique name. 
 - specify any number of audio sub-items (= the AudioClip plus parameters in CLIP mode) 
   within an audio item. 
 - to play an audio item call the static function 
   AudioController.Play( "MyUniqueAudioItemName" )
 - Use AudioController.PlayMusic( "MusicAudioItemName" ) to play music. This function 
   assures that only one music file is played at a time and handles cross fading automatically 
   according to the configuration in the AudioController instance


 For an up-to-date, detailed documentation please visit: http://unity.clockstone.com