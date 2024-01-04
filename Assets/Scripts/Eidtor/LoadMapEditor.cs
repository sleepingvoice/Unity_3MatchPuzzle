using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class LoadMapEditor : EditorWindow
{
    public Sprite S_BGImg;
    public Sprite S_ButtonImg;

    //�� �ҷ��� ����
    private List<string> dataList = new List<string>();
    private Dictionary<int, string> mapFileDirList = new Dictionary<int, string>();
    private int dataNum = 0;

    //�ҷ��� ��
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

        if (checkMakeMap.Count <= 0) // �ҷ��� ���� ������
        {
            if (GUILayout.Button("MapCheck")) // �ʺҷ�����
            {
                LoadData();
            }

            if (dataList.Count > 0) // ���� �ҷ��Դٸ�
            {
                GUILayout.Space(10);

                dataNum = EditorGUILayout.Popup(dataNum, dataList.ToArray()); // ���ϴ� ���� ��ȣ�� ����

                GUILayout.Space(10);
                if (GUILayout.Button("LoadMapNumber")) // ������ ���� �ҷ��´�
                {
                    int mapNum = int.Parse(dataList[dataNum]);
                    var JsonValue = JsonUtility.FromJson<MapClass>(File.ReadAllText(mapFileDirList[mapNum]));
                    checkMakeMap = ReadFile(JsonValue);
                }
            }
            return;
        }

        //�ҷ��� �� �����ֱ�
        for (int i = 0; i < checkMakeMap.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            for (int j = 0; j < checkMakeMap[0].Count; j++)
            {
                EditorGUILayout.LabelField(checkMakeMap[i][j] ? "��" : "��", GUILayout.Width(10));
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.Space(20);

        GUILayout.BeginHorizontal();
        GUILayout.Label("��� �̹���", GUILayout.Width(EditorGUIUtility.labelWidth - 50));
        S_BGImg = (Sprite)EditorGUILayout.ObjectField(S_BGImg, typeof(Sprite), true);
        GUILayout.EndHorizontal();

        GUILayout.Space(20);

        GUILayout.BeginHorizontal();
        GUILayout.Label("��ư �̹���", GUILayout.Width(EditorGUIUtility.labelWidth - 50));
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
    /// ������ �������ִ��� Ȯ���ϰ� �ҷ����� ���
    /// </summary>
    private void LoadData()
    {
        string[] jsonFiles = Directory.GetFiles(Application.dataPath + "/MapData", "*.json"); // ������ ����� ������ �ִ��� Ȯ��

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
        GUILayout.Label("����� ���� �����ϴ�. Ȯ�ιٶ��ϴ�.", CenterAllign);
    }


    /// <summary>
    /// MapClass�� ����Ʈ ���� �б����� �� ������� ����"
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
