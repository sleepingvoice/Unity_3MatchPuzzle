using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LoadMapEditor : EditorWindow
{
    public Sprite s_BGImg;
    public Sprite s_tempImg;
    private List<List<bool>> CheckMakeMap = new List<List<bool>>();

    [MenuItem("MapEditor/LoadMapCreate")]
    static void LoadMap()
    {
        LoadMapEditor window = (LoadMapEditor)EditorWindow.GetWindow(typeof(LoadMapEditor));
        window.Show();
    }

    private void OnGUI()
    {
        AddMap();
    }


    private int TextrueValue = 0;

    private void AddMap()
    {

        if (CheckMakeMap.Count <= 0)
        {
            ShowError();
            return;
        }

        //만들어진 맵을 보여주기
        for (int i = 0; i < CheckMakeMap.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            for (int j = 0; j < CheckMakeMap[0].Count; j++)
            {
                EditorGUILayout.LabelField(CheckMakeMap[i][j] ? "■" : "□", GUILayout.Width(10));
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.Space(20);

        GUILayout.BeginHorizontal();
        GUILayout.Label("배경 이미지", GUILayout.Width(EditorGUIUtility.labelWidth - 50));
        s_BGImg = (Sprite)EditorGUILayout.ObjectField(s_BGImg, typeof(Sprite), true);
        GUILayout.EndHorizontal();

        GUILayout.Space(20);

        GUILayout.BeginHorizontal();
        GUILayout.Label("버튼 이미지", GUILayout.Width(EditorGUIUtility.labelWidth - 50));
        s_tempImg = (Sprite)EditorGUILayout.ObjectField(s_tempImg, typeof(Sprite), true);
        GUILayout.EndHorizontal();

        GUILayout.Space(20);

        if (GUILayout.Button("MakeMapImg"))
        {
            if (s_BGImg != null && s_tempImg != null)
            {

            }
        }
    }

    private void ShowError()
    {
        GUILayout.Space(10);
        GUIStyle CenterAllign = new GUIStyle();
        CenterAllign.alignment = TextAnchor.MiddleCenter;
        CenterAllign.normal.textColor = Color.red;
        CenterAllign.fontSize = 20;
        GUILayout.Label("저장된 맵이 없습니다. 확인바랍니다.", CenterAllign);
    }

}
