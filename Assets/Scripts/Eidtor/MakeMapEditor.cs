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
    private SerializedProperty _bGImg;
    private SerializedProperty _tempImg;

    private void OnEnable()
    {
        _row = serializedObject.FindProperty("i_row");
        _colmn = serializedObject.FindProperty("i_column");
        _bGImg = serializedObject.FindProperty("s_BGImg");
        _tempImg = serializedObject.FindProperty("s_tempImg");
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

        _showMapList = EditorGUILayout.Foldout(_showMapList, "ShowMap");
        if (_showMapList)
        {
            AddMap();
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



    #endregion

    #region 맵 만들기

    private int TextrueValue = 0;

    private void AddMap()
    {
        MakeMap map = (target as MakeMap);
        if (map.CheckMakeMap.Count <= 0)
        {
            ShowError();
            return;
        }

        //만들어진 맵을 보여주기
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

        EditorGUILayout.PropertyField(_bGImg, true); // 임시 뒷배경

        EditorGUILayout.PropertyField(_tempImg, true); // 임시 버튼 위치

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("MakeMapImg"))
        {
            if (_bGImg != null && _tempImg != null)
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

    #endregion

    #region Utill
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

    Texture2D LoadTexture(string path)
    {
        byte[] fileData = System.IO.File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData); // 자동으로 이미지 크기 등을 설정합니다.
        return texture;
    }

    #endregion
}
