using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;

public class LoadMapEditor : EditorWindow
{
    public Image O_bgImg;
    public Object O_PrefabImg;
    public Sprite S_buttonImg;

    public int I_bgWidth;
    public int I_bgHeight;
    public int I_buttonSize;
    public float f_buttonSpace;

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
                    
                    //�ʱ�ȭ �۾�
                    O_bgImg = null;
                    O_PrefabImg = null;
                    S_buttonImg = null;
                    I_bgWidth = 0;
                    I_bgHeight = 0;
                    I_buttonSize = 0;
                    f_buttonSpace = 0;

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
        O_bgImg = (Image)EditorGUILayout.ObjectField(O_bgImg, typeof(Image), true);
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("��ư �̹���", GUILayout.Width(EditorGUIUtility.labelWidth - 50));
        S_buttonImg = (Sprite)EditorGUILayout.ObjectField(S_buttonImg, typeof(Sprite), true);
        GUILayout.EndHorizontal();

        GUILayout.Space(10);



        GUILayout.Space(20);

        Vector2 ImageSize = Vector2.zero;

        if (O_bgImg != null && S_buttonImg != null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("��ư ũ��", GUILayout.Width(EditorGUIUtility.labelWidth - 50));
            I_buttonSize = EditorGUILayout.IntField(I_buttonSize);
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.Label("��ư ���� �Ÿ�", GUILayout.Width(EditorGUIUtility.labelWidth - 50));
            f_buttonSpace = EditorGUILayout.FloatField(f_buttonSpace);
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.Label("��ư ������", GUILayout.Width(EditorGUIUtility.labelWidth - 50));
            O_PrefabImg = EditorGUILayout.ObjectField(O_PrefabImg, typeof(GameObject), true);
            GUILayout.EndHorizontal();


            if (O_PrefabImg == null || I_buttonSize == 0 || f_buttonSpace == 0)
                return;

            if (GUILayout.Button("MakeMapImg"))
            {
                Vector2 BGSize = O_bgImg.rectTransform.sizeDelta;
                I_bgHeight = (int)BGSize.y;
                I_bgWidth = (int)BGSize.x;

                // �̹����� ����� �����´�

                ImageSize = O_bgImg.rectTransform.sizeDelta;

                int ButtonTrans = I_buttonSize + (int)f_buttonSpace;

                GameObject tempObj = Instantiate(new GameObject(), O_bgImg.transform);
                tempObj.AddComponent<RectTransform>();
                tempObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // ��� ������ ����
                Vector2 ButtonValue = new Vector2(checkMakeMap[0].Count - 1, checkMakeMap.Count - 1);
                tempObj.GetComponent<RectTransform>().sizeDelta = new Vector2(ButtonTrans * ButtonValue.x + I_buttonSize, ButtonTrans * ButtonValue.y + I_buttonSize);

                for (int i = 0; i < checkMakeMap.Count; i++)
                {
                    for (int j = 0; j < checkMakeMap[0].Count; j++)
                    {
                        if (checkMakeMap[i][j])
                        {
                            GameObject ButtonObj = (GameObject)Instantiate(O_PrefabImg, tempObj.transform);
                            ButtonObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(ButtonTrans * j, -ButtonTrans * i);
                            ButtonObj.GetComponent<RectTransform>().sizeDelta = new Vector2(I_buttonSize, I_buttonSize);
                            ButtonObj.GetComponent<Image>().sprite = S_buttonImg;
                        }
                        //checkmap�� �ִ� ������ ��ư�� ����
                    }
                }
                // ��ư ũ�⿡ ���� ������� ä���(�̶� ����� �Ÿ��� �����)
                // ��� ������ �������� ���
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
