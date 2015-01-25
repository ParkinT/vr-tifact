using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An audio category represents a set of AudioItems. Categories allow to change the volume of all containing audio items.
/// </summary>
[System.Serializable]
public class AudioCategory
{
    /// <summary>
    /// The name of category ( = <c>categoryID</c> )
    /// </summary>
    public string Name;

    /// <summary>
    /// The volume factor applied to all audio items in the category.
    /// You change the volume by script and the change will be apply to all 
    /// playing audios immediately.
    /// </summary>
    public float Volume
    {
        get { return _volume; }
        set { _volume = value; _ApplyVolumeChange(); }
    }

    /// <summary>
    /// Allows to define a specific audio object prefab for this category. If none is defined, 
    /// the default prefab as set by <see cref="AudioController.AudioObjectPrefab"/> is taken.
    /// </summary>
    /// <remarks> This way you can e.g. use special effects such as the reverb filter for 
    /// a specific category. Just add the respective filter component to the specified prefab.</remarks>
    public GameObject AudioObjectPrefab;

    /// <summary>
    /// Define your AudioItems using Unity inspector.
    /// </summary>  
    public AudioItem[] AudioItems;

    [SerializeField]
    private float _volume = 1.0f;

    internal void _AnalyseAudioItems( Dictionary<string, AudioItem> audioItemsDict )
    {
        if ( AudioItems == null ) return;

        foreach ( AudioItem ai in AudioItems )
        {
            if ( ai != null )
            {
                ai._Initialize( this );
#if AUDIO_TOOLKIT_DEMO
                int? demoMaxNumAudioItemsConst = 0x12345B;

                int? demoMaxNumAudioItems = (demoMaxNumAudioItemsConst & 0xf);
                demoMaxNumAudioItems++;

                if ( audioItemsDict.Count > demoMaxNumAudioItems )
                {
                    Debug.LogError( "Audio Toolkit: The demo version does not allow more than " + demoMaxNumAudioItems + " audio items." );
                    Debug.LogWarning( "Please buy the full version of Audio Toolkit!" );
                    return;
                }
#endif

                //Debug.Log( string.Format( "SubItem {0}: {1} {2} {3}", fi.Name, ai.FixedOrder, ai.RandomOrderStart, ai._lastChosen ) );

                if ( audioItemsDict != null )
                {
                    try
                    {
                        audioItemsDict.Add( ai.Name, ai );
                    }
                    catch ( ArgumentException )
                    {
                        Debug.LogWarning( "Multiple audio items with name '" + ai.Name + "'");
                    }
                }
            }

        }
    }

    private void _ApplyVolumeChange()
    {
        // TODO: change Volume into a property and call ApplyVolumeChange automatically (requires editor inspector adaption!) 

        AudioObject[] objs = RegisteredComponentController.GetAllOfType<AudioObject>();

        foreach ( AudioObject o in objs )
        {
            if ( o.category == this )
            {
                //if ( o.IsPlaying() )
                {
                    o._ApplyVolume();
                }
            }
        }
    }
}

/// <summary>
/// Used by <see cref="AudioItem"/> to determine which <see cref="AudioSubItem"/> is chosen. 
/// </summary>
public enum AudioPickSubItemMode
{
    /// <summary>disables playback</summary>  
    Disabled,

    /// <summary>chooses a random subitem with a probability in proportion to <see cref="AudioSubItem.Probability"/> </summary>  
    Random,

    /// <summary>chooses a random subitem with a probability in proportion to <see cref="AudioSubItem.Probability"/> and makes sure it is not played twice in a row (if possible)</summary>
    RandomNotSameTwice,

    /// <summary> chooses the subitems in a sequence one after the other starting with the first</summary>
    Sequence,

    /// <summary> chooses the subitems in a sequence one after the other starting with a random subitem</summary>
    SequenceWithRandomStart,

    /// <summary> chooses all subitems at the same time</summary>
    AllSimultaneously,

    /// <summary> chooses two different subitems at the same time (if possible)</summary>
    TwoSimultaneously,
}

/// <summary>
/// The AudioItem class represents a uniquely named audio entity that can be played by scripts.
/// </summary>
/// <remarks>
/// AudioItem objects are defined in an AudioCategory using the Unity inspector.
/// </remarks>
[System.Serializable]
public class AudioItem
{
    /// <summary>
    /// The unique name of the audio item ( = audioID )
    /// </summary>
    public string Name;

    /// <summary>
    /// If enabled the audio item will get looped when played.
    /// </summary>
    public bool Loop = false;

    /// <summary>
    /// If disabled, the audio will keep on playing if a new scene is loaded.
    /// </summary>
    public bool DestroyOnLoad = true;

    /// <summary>
    /// The volume applied to all audio sub-items of this audio item. 
    /// </summary>
    public float Volume = 1;

    /// <summary>
    /// Determines which <see cref="AudioSubItem"/> is chosen when playing an <see cref="AudioItem"/>
    /// </summary>
    public AudioPickSubItemMode SubItemPickMode = AudioPickSubItemMode.RandomNotSameTwice;

    /// <summary>
    /// Assures that the same audio item will not be played multiple times within this time frame. This is useful if several events triggered at almost the same time want to play the same audio item which can cause unwanted noise artifacts.
    /// </summary>
    public float MinTimeBetweenPlayCalls = 0.1f;

    /// <summary>
    /// Assures that the same audio item will not be played more than <c>MaxInstanceCount</c> times simultaneously.
    /// </summary>
    /// <remarks>Set to 0 to disable.</remarks>
    public int MaxInstanceCount = 0;

    /// <summary>
    /// Defers the playback of the audio item for <c>Delay</c> seconds.
    /// </summary>
    public float Delay = 0;


    /// <summary>
    /// Define your audio sub-items using the Unity inspector.
    /// </summary>
    public AudioSubItem[] subItems;

    internal int _lastChosen = -1;
    internal float _lastPlayedTime = -1;

    /// <summary>
    /// the <c>AudioCategroy</c> the audio item belongs to.
    /// </summary>
    public AudioCategory category
    {
        private set;
        get;
    }

    void Awake()
    {
        _lastChosen = -1;
    }

    /// <summary>
    /// Initializes the audio item for a certain category. (Internal use only, not required to call).
    /// </summary>
    internal void _Initialize( AudioCategory categ )
    {
        category = categ;

        _NormalizeSubItems();
    }

    private void _NormalizeSubItems()
    {
        float sum = 0.0f;

        int subItemID = 0;

        foreach ( AudioSubItem i in subItems )
        {
            i.item = this;
            if ( _IsValidSubItem( i ) )
            {
                sum += i.Probability;
            }
            i._subItemID = subItemID;
            subItemID++;
        }

        if ( sum <= 0 )
        {
            return;
        }

        // Compute normalized probabilities

        float summedProb = 0;

        foreach ( AudioSubItem i in subItems )
        {
            if ( _IsValidSubItem( i ) )
            {
                summedProb += i.Probability / sum;

                i._SummedProbability = summedProb;
            }
        }
    }

    private static bool _IsValidSubItem( AudioSubItem item )
    {
        switch ( item.SubItemType )
        {
        case AudioSubItemType.Clip:
            return item.Clip != null;
        case AudioSubItemType.Item:
            return item.ItemModeAudioID != null && item.ItemModeAudioID.Length > 0;
        }
        return false;
    }
}

/// <summary>
/// The type of an <see cref="AudioSubItem"/>  
/// </summary>
public enum AudioSubItemType
{
    /// <summary>The <see cref="AudioSubItem"/> plays an <see cref="UnityEngine.AudioClip"/></summary>
    Clip,
    /// <summary>The <see cref="AudioSubItem"/> plays an <see cref="AudioItem"/></summary>
    Item,
}

/// <summary>
/// An AudioSubItem represents a specific Unity audio clip.
/// </summary>
/// <remarks>
/// Add your AudioSubItem to an AudioItem using the Unity inspector.
/// </remarks>
[System.Serializable]
public class AudioSubItem
{
    /// <summary>
    /// Specifies the type of this <see cref="AudioSubItem"/>  
    /// </summary>
    public AudioSubItemType SubItemType = AudioSubItemType.Clip;

    /// <summary>
    /// If multiple sub-items are defined within an audio item, the specific audio clip is chosen with a probability in proportion to the <c>Probability</c> value.
    /// </summary>
    public float Probability = 1.0f;

    /// <summary>
    /// Specifies the <c>audioID</c> to be played in case of the <see cref="AudioSubItemType.Item"/> mode
    /// </summary>
    public string ItemModeAudioID;

    /// <summary>
    /// Specifies the <see cref="UnityEngine.AudioClip"/> to be played in case of the <see cref="AudioSubItemType.Item"/> mode.
    /// </summary>
    public AudioClip Clip;

    /// <summary>
    /// The volume applied to the audio sub-item.
    /// </summary>
    public float Volume = 1.0f;

    /// <summary>
    /// Alters the pitch in units of semitones ( thus 12 = twice the speed)
    /// </summary>
    public float PitchShift = 0f;

    /// <summary>
    /// Alters the pan: -1..left,  +1..right
    /// </summary>
    public float Pan2D = 0;

    /// <summary>
    /// Defers the playback of the audio sub-item for <c>Delay</c> seconds.
    /// </summary>
    public float Delay = 0;

    /// <summary>
    /// Randomly shifts the pitch in units of semitones ( thus 12 = twice the speed)
    /// </summary>
    public float RandomPitch = 0;

    /// <summary>
    /// Randomly shifts the volume +/- this value
    /// </summary>
    public float RandomVolume = 0;

    /// <summary>
    /// Randomly adds a delay between 0 and RandomDelay
    /// </summary>
    public float RandomDelay = 0;

    /// <summary>
    /// Ends playing the audio at this time (in seconds).
    /// </summary>
    /// <remarks>
    /// Can be used as a workaround for an unknown clip length (e.g. for tracker files)
    /// </remarks>
    public float ClipStopTime = 0;

    /// <summary>
    /// Offsets the the audio clip start time (in seconds).
    /// </summary>
    /// <remarks>
    /// Does not work with looping.
    /// </remarks>
    public float ClipStartTime = 0;

    /// <summary>
    /// Automatic fade-in in seconds
    /// </summary>
    public float FadeIn = 0;

    /// <summary>
    /// Automatic fade-out in seconds
    /// </summary>
    public float FadeOut = 0;

    /// <summary>
    /// Starts playing at a random position.
    /// </summary>
    /// <remarks>
    /// Useful for audio loops.
    /// </remarks>
    public bool RandomStartPosition = false;

    private float _summedProbability = -1.0f; // -1 means not initialized or invalid
    internal int _subItemID = 0;

    internal float _SummedProbability
    {
        get { return _summedProbability; }
        set { _summedProbability = value; }
    }

    /// <summary>
    /// the <c>AudioItem</c> the sub-item belongs to.
    /// </summary>
    public AudioItem item
    {
        internal set;
        get;
    }

    /// <summary>
    /// Returns the name of the audio clip for debugging.
    /// </summary>
    /// <returns>
    /// The debug output string.
    /// </returns>
    public override string ToString()
    {
        if ( SubItemType == AudioSubItemType.Clip )
        {
            return "CLIP: " + Clip.name;
        }
        else
            return "ITEM: " + ItemModeAudioID;
    }

}