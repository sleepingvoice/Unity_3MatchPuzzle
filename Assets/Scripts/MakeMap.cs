using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeMap : MonoBehaviour
{
    #region �� ����Ʈ �����

    public List<List<bool>> CheckMakeMap = new List<List<bool>>();

    [SerializeField]
    private int i_row = 0;

    [SerializeField]
    private int i_column = 0;

    public void InitValue()
    {
        i_row = 0;
        i_column = 0;
    }

    public void AddMapRow()
    {
        if(i_row<20) i_row++;
    }

    public void AddMapColumn()
    {
        if(i_column < 20) i_column++;
    }

    public void DeleteMapRow()
    {
        if(i_row > 0) i_row--;
    }

    public void DeleteMapColumn()
    {
        if (i_column > 0) i_column--;
    }

    #endregion

    /// <summary>
    /// �̹����� �ȼ� ��ġ�� �´� ��ġ�� ��ȯ
    /// </summary>
    /// <param name="TargetImage"> Ÿ�� �̹��� </param>
    /// <param name="Coordinate"> �̹��� �ȼ� ��ġ </param>
    /// <returns></returns>
    Vector3 ConvertCoordinate(Image TargetImage, Vector2 Coordinate)
    {
        Vector3 Pos = TargetImage.transform.position;
        Vector2 CovnvertVec = -(Coordinate - Vector2.one / 2);
        float halfWidth = TargetImage.rectTransform.sizeDelta.x * CovnvertVec.x;
        float halfHeight = TargetImage.rectTransform.sizeDelta.y * CovnvertVec.y;
        return Pos - new Vector3(halfWidth, halfHeight, 0f);
    }
}
