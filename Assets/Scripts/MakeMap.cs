using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeMap : MonoBehaviour
{
    #region 맵 리스트 만들기

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
    /// 이미지의 픽셀 위치에 맞는 위치값 반환
    /// </summary>
    /// <param name="TargetImage"> 타겟 이미지 </param>
    /// <param name="Coordinate"> 이미지 픽셀 위치 </param>
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
