using UnityEngine;
using System.Collections;

#pragma warning disable 1591 // undocumented XML code warning

public class AudioToolkitDemo : MonoBehaviour
{

    public bool musicPaused = false;

    float musicVolume = 1;

    void OnGUI()
    {
        GUI.Label( new Rect( 22, 10, 200, 20 ), "ClockStone Audio Toolkit Demo" );

        int ypos = 10;
        int yposOff = 35;

        GUI.Label( new Rect( 250, ypos, 200, 30 ), "Global Volume" );

        AudioController.SetGlobalVolume( GUI.HorizontalSlider( new Rect( 250, ypos + 30, 200, 30 ), AudioController.GetGlobalVolume(), 0, 1 ) );

        ypos += 50;

        if ( GUI.Button( new Rect( 20, ypos, 200, 30 ), "Cross-fade to music track 1" ) )
        {
            AudioController.PlayMusic( "MusicTrack1" );
        }

        GUI.Label( new Rect( 250, ypos, 200, 30 ), "Music Volume" );

        float musicVolumeNew = GUI.HorizontalSlider( new Rect( 250, ypos + 30, 200, 30 ), musicVolume, 0, 1 );

        if ( musicVolumeNew != musicVolume )
        {
            musicVolume = musicVolumeNew;
            AudioController.SetCategoryVolume( "Music", musicVolume );
        }

        ypos += yposOff;

        if ( GUI.Button( new Rect( 20, ypos, 200, 30 ), "Cross-fade to music track 2" ) )
        {
            AudioController.PlayMusic( "MusicTrack2" );
        }

        ypos += yposOff;

        if ( GUI.Button( new Rect( 20, ypos, 200, 30 ), "Stop Music" ) )
        {
            AudioController.StopMusic( 0.3f );
        }

        ypos += yposOff;

        bool musicPausedNew = GUI.Toggle( new Rect( 20, ypos, 200, 30 ), musicPaused, "Pause Music" );

        if ( musicPausedNew != musicPaused )
        {
            musicPaused = musicPausedNew;

            if ( musicPaused )
            {
                AudioController.PauseMusic();
            }
            else
                AudioController.UnpauseMusic();
        }


        ypos += yposOff;
        ypos += yposOff;

        if ( GUI.Button( new Rect( 20, ypos, 200, 30 ), "Play Sound with random pitch" ) )
        {
            AudioController.Play( "RandomPitchSound" );
        }
        ypos += yposOff;

        if ( GUI.Button( new Rect( 20, ypos, 200, 30 ), "Play Sound with alternatives" ) )
        {
            AudioObject soundObj = AudioController.Play( "AlternativeSound" );
            if( soundObj != null ) soundObj.completelyPlayedDelegate = OnAudioCompleteleyPlayed;
        }
        ypos += yposOff;

        if ( GUI.Button( new Rect( 20, ypos, 200, 30 ), "Play Both" ) )
        {
            AudioObject soundObj = AudioController.Play( "RandomAndAlternativeSound" );
            if ( soundObj != null ) soundObj.completelyPlayedDelegate = OnAudioCompleteleyPlayed;
        }
        ypos += yposOff;

        ypos += yposOff;

        if ( GUI.Button( new Rect( 20, ypos, 200, 30 ), "Play Music Playlist" ) )
        {
            AudioController.PlayMusicPlaylist();
        }

        ypos += yposOff;

        if ( AudioController.IsPlaylistPlaying() && GUI.Button( new Rect( 20, ypos, 200, 30 ), "Next Track on Playlist" ) )
        {
            AudioController.PlayNextMusicOnPlaylist();
        }

        ypos += 32;

        if ( AudioController.IsPlaylistPlaying() && GUI.Button( new Rect( 20, ypos, 200, 30 ), "Previous Track on Playlist" ) )
        {
            AudioController.PlayPreviousMusicOnPlaylist();
        }

        ypos += yposOff;
        AudioController.Instance.loopPlaylist = GUI.Toggle( new Rect( 20, ypos, 200, 30 ), AudioController.Instance.loopPlaylist, "Loop Playlist" );
        ypos += yposOff;
        AudioController.Instance.shufflePlaylist = GUI.Toggle( new Rect( 20, ypos, 200, 30 ), AudioController.Instance.shufflePlaylist, "Shuffle Playlist " );

        if ( GUI.Button( new Rect( Screen.width / 2 - 150, Screen.height - 40, 300, 30 ), "Video tutorial & more info..." ) )
        {
            Application.OpenURL( "http://unity.clockstone.com" );
        }
    }

    void OnAudioCompleteleyPlayed( AudioObject audioObj )
    {
        Debug.Log( "Finished playing " + audioObj.audioID + " with clip " + audioObj.audio.clip.name );
    }
}
