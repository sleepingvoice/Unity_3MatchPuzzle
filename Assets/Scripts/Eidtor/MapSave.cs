using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapSave : EditorWindow
{
    private int i_row;
    private int i_column;


    [MenuItem("MapEditor/SaveMapData")]
    static void LoadMap()
    {
        MapSave window = (MapSave)EditorWindow.GetWindow(typeof(MapSave));
        window.Show();
    }

    private void OnGUI()
    {
        SetToggleBtn();
    }

    bool _change = true;
    List<List<bool>> _newBoolList = new List<List<bool>>();

    private void SetToggleBtn()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Row");
        EditorGUILayout.LabelField(i_row.ToString());
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Colmn");
        EditorGUILayout.LabelField(i_column.ToString());
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("MakeToggleBtn"))
        {
            InitMapList();
        }

        MakeNewToggle(_newBoolList);

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate Row"))
        {
            if (i_row < 20) i_row++;
            _change = false;
        }
        if (GUILayout.Button("Generate Column"))
        {
            if (i_column < 20) i_column++;
            _change = false;
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(2);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Delete Row"))
        {
            if (i_row > 0) i_row--;
        }
        if (GUILayout.Button("Delete Column"))
        {
            if (i_column > 0) i_column--;
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        if (GUILayout.Button("InitValue"))
        {
            i_row = 0;
            i_column = 0;
            InitMapList();
        }
        GUILayout.Space(30);

        if (_newBoolList.Count != 0)
        {
            if (GUILayout.Button("MakeMapJson"))
            {

            }
        }
    }

    private void MakeNewToggle(List<List<bool>> ListValue)
    {
        for (int i = 0; i < ListValue.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            // 토글 배열 생성

            for (int j = 0; j < ListValue[0].Count; j++)
            {
                Rect toggleRect = GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.toggle);

                bool oldValue = ListValue[i][j];
                bool newValue = EditorGUI.Toggle(toggleRect, GUIContent.none, ListValue[i][j]);

                if (oldValue != newValue)
                {
                    ListValue[i][j] = newValue;
                }
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
    }

    public void InitMapList()
    {
        _newBoolList = new List<List<bool>>();
        for (int i = 0; i < i_row; i++)
        {
            _newBoolList.Add(new List<bool>());
            for (int j = 0; j < i_column; j++)
            {
                _newBoolList[i].Add(true);
            }
        }
    }

    List<List<T>> DeepCopy<T>(List<List<T>> original)
    {
        List<List<T>> copy = new List<List<T>>();

        foreach (var row in original)
        {
            List<T> rowCopy = new List<T>(row);
            copy.Add(rowCopy);
        }

        return copy;
    }
}
