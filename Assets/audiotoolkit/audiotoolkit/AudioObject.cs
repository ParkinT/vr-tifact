// AudioObject - copyright by ClockStone 2011
// part of the ClockStone Unity Audio Framework, see AudioController.cs

using UnityEngine;
using System.Collections;

/// <summary>
/// The object playing the audio clip associated with a <see cref="AudioSubItem"/>
/// </summary>
[RequireComponent( typeof( AudioSource ) )]
[AddComponentMenu("ClockStone/Audio/AudioObject")]
public class AudioObject : RegisteredComponent
{

	// **************************************************************************************************/
	//          public functions
	// **************************************************************************************************/

	/// <summary>
	/// Gets the audio ID.
	/// </summary>
	public string audioID
	{
		get;
		internal set;
	}

	/// <summary>
	/// Gets the category.
	/// </summary>
	public AudioCategory category
	{
		get;
		internal set;
	}

    /// <summary>
    /// Gets the corresponding AudioSubItem
    /// </summary>
    public AudioSubItem subItem
    {
        get;
        internal set;
    }

    /// <summary>
    /// The audio event delegate type.
    /// </summary>
    public delegate void AudioEventDelegate( AudioObject audioObject );

    /// <summary>
    /// Gets or sets the delegate to be called once an audio clip was completely played.
    /// </summary>
    public AudioEventDelegate completelyPlayedDelegate
    { set; get; }

	/// <summary>
	/// Gets or sets the volume.
	/// </summary>
    /// <remarks>
    /// This is the adjusted volume volume value with which the Audio clip is currently playing.
    /// It is the value resulting from multiplying the volume of the subitem, item, and the category. 
    /// It does not contain the global volume or the fading value.<para/>
    /// "Adjusted" means that the value does not equal Unity's internal audio clip volume value,
    /// because Unity's volume range is not distributed is a perceptually even manner.<para/>
    /// <c>unityVolume = Mathf.Pow( adjustedVolume, 1.6 )</c>
    /// </remarks>
	public float volume
	{
		get
		{
			return _volumeWithCategory;
		}
		set
		{
			// sets _volumeExcludingCategory so that _volumeWithCategory = volume = value 

			float volCat = _volumeFromCategory;

			if ( volCat > 0 )
			{
				_volumeExcludingCategory = value / volCat;
			}
			else
				_volumeExcludingCategory = value;

			_ApplyVolume();

		}
	}

    /// <summary>
    /// Gets the total volume.
    /// </summary>
    /// <remarks>
    /// This is the adjusted volume volume value with which the Audio clip is currently playing.
    /// It is the value resulting from multiplying the volume of the subitem, item, the category,  
    /// the global volume, and the fading value.<para/>
    /// "Adjusted" means that the value does not equal Unity's internal audio clip volume value,
    /// because Unity's volume range is not distributed is a perceptually even manner.<para/>
    /// <c>unityVolume = Mathf.Pow( adjustedVolume, 1.6 )</c>
    /// </remarks>
    public float volumeTotal
    {
        get
        {
            float vol = _volumeWithCategory * _volumeFromFade;
            
            if ( _audioController )
            {
                vol *= AudioController.GetGlobalVolume();
            }
            return vol;  
        }

    }

    /// <summary>
    /// Gets the time at which the audio started playing. 
    /// </summary>
    /// <remarks>
    /// Does not include a possible delay.
    /// </remarks>
    public float startedPlayingAtTime
    {
        get
        {
            return _playInitialTime;
        }
    }

    /// <summary>
    /// Gets the length of the clip. 
    /// </summary>
    /// <remarks>
    /// Is effected by <see cref="AudioSubItem.ClipStopTime"/> and <see cref="AudioSubItem.ClipStartTime"/>
    /// </remarks>
    public float clipLength
    {
        get
        {
            if ( _stopClipAtTime > 0 )
            {
                return _stopClipAtTime - _startClipAtTime;
            }
            else
            {
                if ( audio.clip != null )
                {
                    return audio.clip.length - _startClipAtTime;
                }
                else return 0;
            }
        }
    }

    /// <summary>
    /// Sets or gets the current audio time relative to <see cref="AudioSubItem.ClipStartTime"/> 
    /// </summary>
    public float audioTime
    {
        get
        {
            return audio.time - _startClipAtTime;
        }
        set
        {
            audio.time = value + _startClipAtTime;
        }
    }
    /// <summary>
    /// return <c>true</c> if the audio is currently fading out
    /// </summary>
    public bool isFadingOut
    {
        get
        {
            return _fadeOutTotalTime > 0;
        }
    }

    /// <summary>
    /// return <c>true</c> if the audio is currently fading in
    /// </summary>
    public bool isFadingIn
    {
        get
        {
            return _fadeInTotalTime > 0;
        }
    }

	/// <summary>
	/// Fades-in a playing audio.
	/// </summary>
	/// <param name="fadeInTime">The fade time in seconds.</param>
	public void FadeIn( float fadeInTime )
	{
		_fadeInTotalTime = fadeInTime;
		_fadeInStartTime = Time.time;
	}

	/// <summary>
	/// Plays the audio clip with the specified delay.
	/// </summary>
	/// <param name="delay">The delay.</param>
	public void Play( float delay )
	{
		_PlayInitial( delay );
	}

	/// <summary>
	/// Plays the audio clip.
	/// </summary>
	public void Play()
	{
		if ( _destroyIfNotPlaying )
		{
			audio.Play();
			_paused = false;
		}
		else
		{
			_PlayInitial( 0 );
		}
	}

	/// <summary>
	/// Stops playing this instance.
	/// </summary>
	public void Stop()
	{
		Stop( 0 );
	}

	/// <summary>
	/// Stops a playing audio with fade-out.
	/// </summary>
    /// <param name="fadeOutTime">The fade time in seconds.</param>
    public void Stop( float fadeOutTime )
	{
        if ( fadeOutTime > 0 )
		{
            _fadeOutTotalTime = fadeOutTime;
			_fadeOutStartTime = Time.time;
		}
		else
		{
			_fadeOutTotalTime = -1;
			_fadeOutStartTime = -1;
			_Stop();
		}
	}

	/// <summary>
	/// Pauses the audio clip.
	/// </summary>
	public void Pause()
	{
		audio.Pause();
		_paused = true;
	}

	/// <summary>
	/// Determines whether the audio clip is paused.
	/// </summary>
	/// <returns>
	///   <c>true</c> if paused; otherwise, <c>false</c>.
	/// </returns>
	public bool IsPaused()
	{
		return _paused;
	}

	/// <summary>
	/// Determines whether the audio clip is playing.
	/// </summary>
	/// <returns>
	///   <c>true</c> if the audio clip is playing; otherwise, <c>false</c>.
	/// </returns>
	public bool IsPlaying()
	{
		return audio.isPlaying;
	}


	// **************************************************************************************************/
	//          private / protected functions and properties
	// **************************************************************************************************/

	internal float _volumeFromCategory
	{
		get
		{
			if ( category != null )
			{
				return category.Volume;
			}
			return 1.0f;
		}
	}

	internal float _volumeWithCategory
	{
		get
		{
			return _volumeFromCategory * _volumeExcludingCategory;
		}
	}

	internal float _volumeExcludingCategory = 1;  // untransformed volume
    private float _volumeFromFade = 1;

	bool _paused = false;
	bool _applicationPaused = false;

	float _fadeOutTotalTime = -1;
	float _fadeOutStartTime = -1;

	float _fadeInTotalTime = -1;
	float _fadeInStartTime = -1;

#pragma warning disable 414
	float _playInitialTime = -1;
#pragma warning restore 414

	bool _destroyIfNotPlaying = false;

    AudioController _audioController;
    internal float _stopClipAtTime = 0;
    internal float _startClipAtTime = 0;

    internal bool _isCurrentPlaylistTrack = false;

	/// <summary>
	/// Initializes this instance.
	/// </summary>
	protected override void Awake()
	{
		base.Awake();

        _audioController = AudioController.Instance;
		//Debug.Log( "AudioObject.Awake" );

		audio.playOnAwake = false;
		audio.clip = null;
		category = null;
        completelyPlayedDelegate = null;

        _fadeOutTotalTime = -1;
		_fadeOutStartTime = -1;
		_fadeInTotalTime = -1;
		_fadeInStartTime = -1;
		_playInitialTime = -1;
        _volumeFromFade = 1;
		_destroyIfNotPlaying = false;
		_volumeExcludingCategory = 1;
		_paused = false;
		_applicationPaused = false;
        _isCurrentPlaylistTrack = false;
        _stopClipAtTime = 0;
        _startClipAtTime = 0;
	}

	private void _PlayInitial( float delay )
	{
		//Debug.Log( "_PlayInitial:" + audioID + " sub:" + _subItemID );

		ulong d = (ulong) ( ( 44100.0f / audio.clip.frequency ) * delay * audio.clip.frequency ); // http://unity3d.com/support/documentation/ScriptReference/AudioSource.Play.html

		//Debug.Log( "Play Clip:" + audio.clip.name + " S/N:"+ GetComponent<PoolableObject>().GetSerialNumber());

		audio.Play( d );

		_playInitialTime = Time.fixedTime;

		_destroyIfNotPlaying = true;
		_paused = false;
	}

	//#if UNITY_EDITOR
	//void OnGUI()
	//{
	//    if ( Time.fixedTime - _playInitialTime < 0.5 )
	//    {
	//        GUI.Label( new Rect( 0, 0, 100, 20 ), "PlayAudio" );
	//    }
	//}
	//#endif

	//public void Restart()
	//{
	//    Play();
	//}

	private void _Stop()
	{
		//Debug.Log( "Stop " );
		if (audio != null) {
			audio.Stop();
		}
	}

	private void Update()
	{
		if ( !_destroyIfNotPlaying ) return;

		if ( !_paused && !_applicationPaused )
		{
            if ( !audio.isPlaying )
            {
                bool destroy;
                if ( completelyPlayedDelegate != null )
                {
                    completelyPlayedDelegate( this );
                    destroy = !audio.isPlaying;
                }
                else
                    destroy = true;

                if ( _isCurrentPlaylistTrack )
                {
                    if( _audioController ) _audioController._NotifyPlaylistTrackCompleteleyPlayed( this );
                }

                if ( destroy )
                {
                    DestroyAudioObject();
                    return;
                }
            }
            else
            {
                if( !audio.loop )
                {
                    if ( _isCurrentPlaylistTrack &&  
                         _audioController && _audioController.crossfadePlaylist &&
                         audioTime > clipLength - _audioController.musicCrossFadeTime )
                    {
                        _audioController._NotifyPlaylistTrackCompleteleyPlayed( this );
                    }
                    else
                    {
                        if ( !isFadingOut && audioTime >= clipLength - subItem.FadeOut )
                        {
                            Stop( subItem.FadeOut ); return;
                        }
                    }
                }
            }
		}

		float fadeVolume = 1;

        if ( isFadingOut )
		{
			fadeVolume *= 1.0f - _GetFadeValue( Time.time - _fadeOutStartTime, _fadeOutTotalTime );

			if ( fadeVolume == 0 )
			{
				Stop(); return;
			}
		}

        if ( isFadingIn )
		{
			fadeVolume *= _GetFadeValue( Time.time - _fadeInStartTime, _fadeInTotalTime );
		}

		if( fadeVolume != _volumeFromFade )
		{
            _volumeFromFade = fadeVolume;
			_ApplyVolume();
		}
	}

	private float _GetFadeValue( float t, float dt )
	{
		return Mathf.Clamp( t / dt, 0.0f, 1.0f);
	}

	private void OnApplicationPause( bool b )
	{
		_applicationPaused = b;
	}
	
	/// <summary>
    /// Destroys the audio object (using <see cref="ObjectPoolController"/> if pooling is enabled)
	/// </summary>
	public void DestroyAudioObject()
	{
        if ( audio.isPlaying )
        {
            audio.Stop();
        }

		//Debug.Log( "Destroy:" + audio.clip.name );
#if AUDIO_TOOLKIT_DEMO
		GameObject.Destroy( gameObject );
#else
		ObjectPoolController.Destroy( gameObject );
#endif
		_destroyIfNotPlaying = false;
	}

    /// <summary>
    /// Transforms the volume to make it perceptually more intuitive to scale and cross-fade.
    /// </summary>
    /// <param name="volume">The volume to transform.</param>
    /// <returns>
    /// The transformed volume <c> = Pow( volume, 1.6 ) </c>
    /// </returns>
	static public float TransformVolume( float volume )
	{
		return Mathf.Pow( volume, 1.6f ); 
	}

    /// <summary>
    /// Transforms the pitch from semitones to a multiplicative factor
    /// </summary>
    /// <param name="pitchSemiTones">The pitch shift in semitones to transform.</param>
    /// <returns>
    /// The transformed pitch <c> = Pow( 2, pitch / 12 ) </c>
    /// </returns>
    static public float TransformPitch( float pitchSemiTones )
	{
        return Mathf.Pow( 2, pitchSemiTones / 12.0f );
	}

    /// <summary>
    /// Inverse pitch transformation: <see cref="TransformPitch"/>
    /// </summary>
    static public float InverseTransformPitch( float pitch )
    {
        return ( Mathf.Log( pitch ) / Mathf.Log( 2.0f ) ) * 12.0f;
    }

	internal void _ApplyVolume()
	{
        float volumeToSet = TransformVolume( volumeTotal );
		//Debug.Log( "volume set:" + volumeToSet );
		audio.volume = volumeToSet;
	}
	
	/*void OnDestroyed()
	{
		Debug.Log( "Destroy:" + audio.clip.name );
	}*/
}


