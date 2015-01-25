using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



[CustomEditor( typeof(AudioObject) )]
public class AudioObject_Editor : EditorEx
{
    protected AudioObject AO;

    public override void OnInspectorGUI()
    {
        DrawInspector();
    }

    string FormatVolume( float volume )
    {
        float dB = 20 * Mathf.Log10( AudioObject.TransformVolume( volume ) );
        return string.Format( "{0:0.000} ({1:0.0} dB)", volume, dB );
    }

    private void DrawInspector()
    {
        AO = (AudioObject) target;

        BeginInspectorGUI();

        //DrawDefaultInspector();
        //VerticalSpace();

        ShowString( AO.audioID, "Audio ID:" );
        ShowString( AO.category != null ? AO.category.Name : "---" , "Audio Category:" );
        ShowString( FormatVolume( AO.volume ), "Item Volume:" );
        ShowString( FormatVolume( AO.volumeTotal ), "Total Volume:" );
        ShowString( string.Format( "{0:0.00} half-tones", AudioObject.InverseTransformPitch( AO.audio.pitch ) ), "Pitch:" );
        if ( AO.audio && AO.audio.clip )
        {
            ShowString( string.Format( "{0} / {1}", AO.audio.time, AO.clipLength ), "Time:" );
        }
        ShowFloat( AO.startedPlayingAtTime, "Time Started:" );

        if ( GUILayout.Button( "Update" ) )
        {

        }


        EndInspectorGUI();
    }

    
    private void VerticalSpace()
    {
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
    }
   
}
