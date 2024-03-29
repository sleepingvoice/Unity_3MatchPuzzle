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
                    
                    //초기화 작업
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
        O_bgImg = (Image)EditorGUILayout.ObjectField(O_bgImg, typeof(Image), true);
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Label("버튼 이미지", GUILayout.Width(EditorGUIUtility.labelWidth - 50));
        S_buttonImg = (Sprite)EditorGUILayout.ObjectField(S_buttonImg, typeof(Sprite), true);
        GUILayout.EndHorizontal();

        GUILayout.Space(10);



        GUILayout.Space(20);

        Vector2 ImageSize = Vector2.zero;

        if (O_bgImg != null && S_buttonImg != null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("버튼 크기", GUILayout.Width(EditorGUIUtility.labelWidth - 50));
            I_buttonSize = EditorGUILayout.IntField(I_buttonSize);
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.Label("버튼 사이 거리", GUILayout.Width(EditorGUIUtility.labelWidth - 50));
            f_buttonSpace = EditorGUILayout.FloatField(f_buttonSpace);
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.Label("버튼 프리펩", GUILayout.Width(EditorGUIUtility.labelWidth - 50));
            O_PrefabImg = EditorGUILayout.ObjectField(O_PrefabImg, typeof(GameObject), true);
            GUILayout.EndHorizontal();


            if (O_PrefabImg == null || I_buttonSize == 0 || f_buttonSpace == 0)
                return;

            if (GUILayout.Button("MakeMapImg"))
            {
                Vector2 BGSize = O_bgImg.rectTransform.sizeDelta;
                I_bgHeight = (int)BGSize.y;
                I_bgWidth = (int)BGSize.x;

                // 이미지의 사이즈를 가져온다

                ImageSize = O_bgImg.rectTransform.sizeDelta;

                int ButtonTrans = I_buttonSize + (int)f_buttonSpace;

                GameObject tempObj = Instantiate(new GameObject(), O_bgImg.transform);
                tempObj.AddComponent<RectTransform>();
                tempObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // 가운데 지점에 생성
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
                        //checkmap에 있는 값으로 버튼을 만듬
                    }
                }
                // 버튼 크기에 따라 가운데부터 채운다(이때 빈공간 거리를 재야함)
                // 가운데 지점을 기준으로 계산
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
