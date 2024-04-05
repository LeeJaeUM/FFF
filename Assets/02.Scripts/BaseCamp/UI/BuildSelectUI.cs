using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildSelectUI : MonoBehaviour, IPointerClickHandler
{
    public enum BuildMode   //건축 모드
    {
        None = 0,
        Foundation,
        Wall_Horizontal,
        Wall_Vertical,
        Enviroment
    };
    [SerializeField] BuildMode buildMode = BuildMode.None;

    public Action<int> onClickBlock;
    /// <summary>
    /// 클릭 시 현재 이 ui에 설정된 BuildMode의 index가 BCUI로 넘어감 이걸로 BlockSpawner의 buildMOde를 변경
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        onClickBlock?.Invoke((int)buildMode);
    }

}
