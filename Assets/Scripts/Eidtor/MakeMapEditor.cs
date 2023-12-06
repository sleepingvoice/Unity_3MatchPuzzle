using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(MakeMap))]
public class MakeMapEditor : Editor
{
    private SerializedProperty _row;
    private SerializedProperty _colmn;

    private void OnEnable()
    {
        _row = serializedObject.FindProperty("i_row");
        _colmn = serializedObject.FindProperty("i_column");
    }

    bool _makeMapList = false;
    bool _showMapList = false;

    public override void OnInspectorGUI()
    {
        _makeMapList = EditorGUILayout.Foldout(_makeMapList, "MakeMap");
        if (_makeMapList)
        {
            SetToggleBtn();
        }

        _showMapList = EditorGUILayout.Foldout(_makeMapList, "ShowMap");
        if (_makeMapList)
        {
            MakeMap map = (target as MakeMap);
            for (int i = 0; i < map.CheckMakeMap.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                for (int j = 0; j < map.CheckMakeMap[0].Count; j++)
                {
                    EditorGUILayout.LabelField(map.CheckMakeMap[i][j] ? "■" : "□", GUILayout.Width(10));
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }

        }
    }

    #region 맵 리스트 만들기

    bool _change = true;
    List<List<bool>> _newBoolList = new List<List<bool>>();

    private void SetToggleBtn()
    {
        MakeMap map = (target as MakeMap);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Row");
        EditorGUILayout.LabelField(_row.intValue.ToString());
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Colmn");
        EditorGUILayout.LabelField(_colmn.intValue.ToString());
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("MakeToggleBtn"))
        {
            InitMapList();
            _change = false;
        }

        if (!_change)
        {
            for (int i = 0; i < _newBoolList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                // 토글 배열 생성

                for (int j = 0; j < _newBoolList[0].Count; j++)
                {
                    Rect toggleRect = GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.toggle);

                    bool oldValue = _newBoolList[i][j];
                    bool newValue = EditorGUI.Toggle(toggleRect, GUIContent.none, _newBoolList[i][j]);

                    if (oldValue != newValue)
                    {
                        _newBoolList[i][j] = newValue;
                    }
                }

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
        }

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate Row"))
        {
            map.AddMapRow();
            _change = false;
        }
        if (GUILayout.Button("Generate Column"))
        {
            map.AddMapColumn();
            _change = false;
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(2);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Delete Row"))
        {
            map.DeleteMapRow();
            _change = false;
        }
        if (GUILayout.Button("Delete Column"))
        {
            map.DeleteMapColumn();
            _change = false;
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        if (GUILayout.Button("InitValue"))
        {
            map.InitValue();
            _change = false;
        }

        GUILayout.Space(30);

        if (GUILayout.Button("MakeList"))
        {
            map.CheckMakeMap = DeepCopy(_newBoolList);
        }
    }

    public void InitMapList()
    {
        _newBoolList = new List<List<bool>>();
        for (int i = 0; i < _row.intValue; i++)
        {
            _newBoolList.Add(new List<bool>());
            for (int j = 0; j < _colmn.intValue; j++)
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

    #endregion


}
