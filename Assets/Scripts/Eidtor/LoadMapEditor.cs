using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class LoadMapEditor : EditorWindow
{
    public Sprite S_BGImg;
    public Sprite S_ButtonImg;

    //맵 불러올 변수
    private List<string> dataList = new List<string>();
    private Dictionary<int, string> mapFileDirList = new Dictionary<int, string>();
    private int dataNum = 0;

    //불러온 맵
    private List<List<bool>> checkMakeMap = new List<List<bool>>();

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

    private void AddMap()
    {
        GUILayout.Space(10);

        if (checkMakeMap.Count <= 0) // 불러온 맵이 없을때
        {
            if (GUILayout.Button("MapCheck")) // 맵불러오기
            {
                LoadData();
            }

            if (dataList.Count > 0) // 맵을 불러왔다면
            {
                GUILayout.Space(10);

                dataNum = EditorGUILayout.Popup(dataNum, dataList.ToArray()); // 원하는 맵의 번호를 선택

                GUILayout.Space(10);
                if (GUILayout.Button("LoadMapNumber")) // 선택한 맵을 불러온다
                {
                    int mapNum = int.Parse(dataList[dataNum]);
                    var JsonValue = JsonUtility.FromJson<MapClass>(File.ReadAllText(mapFileDirList[mapNum]));
                    checkMakeMap = ReadFile(JsonValue);
                }
            }
            return;
        }

        //불러온 맵 보여주기
        for (int i = 0; i < checkMakeMap.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            for (int j = 0; j < checkMakeMap[0].Count; j++)
            {
                EditorGUILayout.LabelField(checkMakeMap[i][j] ? "■" : "□", GUILayout.Width(10));
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.Space(20);

        GUILayout.BeginHorizontal();
        GUILayout.Label("배경 이미지", GUILayout.Width(EditorGUIUtility.labelWidth - 50));
        S_BGImg = (Sprite)EditorGUILayout.ObjectField(S_BGImg, typeof(Sprite), true);
        GUILayout.EndHorizontal();

        GUILayout.Space(20);

        GUILayout.BeginHorizontal();
        GUILayout.Label("버튼 이미지", GUILayout.Width(EditorGUIUtility.labelWidth - 50));
        S_ButtonImg = (Sprite)EditorGUILayout.ObjectField(S_ButtonImg, typeof(Sprite), true);
        GUILayout.EndHorizontal();

        GUILayout.Space(20);

        if (GUILayout.Button("MakeMapImg"))
        {
            if (S_BGImg != null && S_ButtonImg != null)
            {

            }
        }
    }

    /// <summary>
    /// 폴더에 파일이있는지 확인하고 불러오는 기능
    /// </summary>
    private void LoadData()
    {
        string[] jsonFiles = Directory.GetFiles(Application.dataPath + "/MapData", "*.json"); // 정해진 경로의 맵파일 있는지 확인

        if (jsonFiles.Length != 0)
        {
            dataList = new List<string>();
            mapFileDirList = new Dictionary<int, string>();
            foreach (string value in jsonFiles)
            {
                var jsonClass = JsonUtility.FromJson<MapClass>(File.ReadAllText(value));

                mapFileDirList.Add(jsonClass.MapNumber, value);
                dataList.Add(jsonClass.MapNumber.ToString());
            }
        }
        else
        {
            ShowError();
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


    /// <summary>
    /// MapClass의 리스트 값을 읽기위한 맵 모양으로 변경"
    /// </summary>
    private List<List<bool>> ReadFile(MapClass ChangeMap)
    {
        var List = ChangeMap.BoolList;
        List<List<bool>> ReturnList = new List<List<bool>>();
        foreach (var ValueList in List)
        {
            List<bool> AddList = new List<bool>();
            foreach (int value in ValueList.JsonValue)
            {
                AddList.Add(value == 1 ? true : false);
            }
            ReturnList.Add(AddList);
        }
        return ReturnList;
    }
}
