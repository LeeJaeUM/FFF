using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCampManager : MonoBehaviour
{
    public static BaseCampManager instance;
    public static BaseCampManager Instance
    {
        get
        {
            if(instance == null)
                instance = FindAnyObjectByType<BaseCampManager>();
            return instance;
        }
    }

    [SerializeField] WBUI wbui;
    public WBUI WBUI
    {
        get
        {
            if (wbui == null)
                wbui = FindAnyObjectByType<WBUI>();
            return wbui;
        }
    }

    [SerializeField]BlockSpwaner spwaner;
    public BlockSpwaner BlockSpwaner
    {
        get
        {
            if(spwaner == null)
                spwaner = FindAnyObjectByType<BlockSpwaner>();
            return spwaner;
        }
    }
       
}
