using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AudioItemOverview : EditorWindow
{
    [MenuItem( "Window/Audio Toolkit/Item Overview" )]
    static void ShowWindow()
    {
        EditorWindow.GetWindow( typeof( AudioItemOverview ) );
    }

    static Vector2 _scrollPos;

    AudioController _audioController;

    public void Show( AudioController audioController )
    {
        _audioController = audioController;
        base.Show();
    }

    void OnGUI()
    {
        if ( !_audioController )
        {
            _audioController = _FindAudioController();
        }

        if ( !_audioController )
        {
            EditorGUILayout.LabelField( "No AudioController found!" );
            return;
        }

        // header

        int buttonSize = 130;

        GUIStyle headerStyle = new GUIStyle( EditorStyles.boldLabel );
        GUIStyle headerStyleButton = new GUIStyle( EditorStyles.miniButton );
        headerStyleButton.fixedWidth = buttonSize;
        headerStyleButton.fontStyle = FontStyle.Bold;

        GUIStyle styleButton = new GUIStyle( EditorStyles.miniButton );
        styleButton.fixedWidth = buttonSize;

        EditorGUILayout.BeginHorizontal();
        if ( GUILayout.Button( _audioController.name, headerStyleButton ) )
        {
            _SelectCurrentAudioController();

        }

        EditorGUILayout.LabelField( "Category", headerStyle );
        EditorGUILayout.LabelField( "Item", headerStyle );
        EditorGUILayout.LabelField( "Sub Item", headerStyle );


        EditorGUILayout.EndHorizontal();

        // data

        _scrollPos = EditorGUILayout.BeginScrollView( _scrollPos );

        int lastCategoryIndex = -1;
        int lastItemIndex = -1;

        string sameTypeString = ".";

        if ( _audioController.AudioCategories != null )
        {
            for ( int categoryIndex=0; categoryIndex < _audioController.AudioCategories.Length; categoryIndex++ )
            {
                var category = _audioController.AudioCategories[categoryIndex];
                if ( category.AudioItems == null ) continue;
                for ( int itemIndex = 0; itemIndex < category.AudioItems.Length; itemIndex++ )
                {
                    var item = category.AudioItems[itemIndex];
                    if ( item.subItems == null ) continue;
                    for ( int subitemIndex = 0; subitemIndex < item.subItems.Length; subitemIndex++ )
                    {
                        var subItem = item.subItems[ subitemIndex ];
                        EditorGUILayout.BeginHorizontal();
                        
                        if ( GUILayout.Button( "Select", styleButton ) )
                        {
                            _audioController._currentInspectorSelection.currentCategoryIndex = categoryIndex;
                            _audioController._currentInspectorSelection.currentItemIndex = itemIndex;
                            _audioController._currentInspectorSelection.currentSubitemIndex = subitemIndex;
                            _SelectCurrentAudioController();
                        }

                        EditorGUILayout.LabelField( ( categoryIndex != lastCategoryIndex ) ? category.Name : sameTypeString );
                        EditorGUILayout.LabelField( ( itemIndex != lastItemIndex ) ? item.Name : sameTypeString );

                        string subItemName;
                        if ( subItem.SubItemType == AudioSubItemType.Clip )
                        {
                            if ( subItem.Clip != null )
                            {
                                subItemName = "CLIP: " + subItem.Clip.name;
                            }
                            else
                            {
                                subItemName = "CLIP: *unset*";

                            }
                        }
                        else
                            subItemName = "ITEM: " + subItem.ItemModeAudioID;

                        EditorGUILayout.LabelField( subItemName );
                        EditorGUILayout.EndHorizontal();

                        lastItemIndex = itemIndex;
                        lastCategoryIndex = categoryIndex;

                    }
                }
            }
        }
        
        EditorGUILayout.EndScrollView();

    }

    private void _SelectCurrentAudioController()
    {
        var gos = new GameObject[ 1 ];
        gos[ 0 ] = _audioController.gameObject;
        Selection.objects = gos;
    }

    private AudioController _FindAudioController()
    {
        var ac = GameObject.FindObjectOfType( typeof( AudioController ) ) as AudioController;

        if ( ac ) return ac;

        var acArray = FindObjectsOfTypeIncludingAssets( typeof( AudioController ) ) as AudioController[];
        if ( acArray != null && acArray.Length > 0 )
        {
            return acArray[ 0 ];
        }
        return null;
    }
}
