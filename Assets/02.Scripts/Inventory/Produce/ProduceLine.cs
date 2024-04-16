using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 제작에 아이템을 보여주는 클래스
public class ProduceLine : MonoBehaviour
{
    #region 변수
    private ItemData_Produce data;

    public ItemData_Produce Data => data;

    public GameObject IngredientSlot;
    #endregion

    #region UI 컴포넌트
    
    #endregion

    private void Awake()
    {
        
    }

    #region 제작아이템 리스트
    /// <summary>
    /// 시작시 동작하는 함수
    /// </summary>
    /// <param name="data">들어갈 아이템 정보</param>
    public void Initialize(ItemData_Produce data)
    {

    }
    #endregion
}
