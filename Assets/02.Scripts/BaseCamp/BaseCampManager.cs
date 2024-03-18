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


    [SerializeField]Workbench workbench;
    public Workbench Workbench 
    { 
        get 
        { 
            if (workbench == null)
                workbench = FindAnyObjectByType<Workbench>();
            return workbench; 
        }

    }
    /// <summary>
    /// buildmode가 foundation일때 반투명하게 미리 위치를 보여주는 오브젝트
    /// </summary>
    public GameObject fa_preview;
    public GameObject wall_preview;

    private void Start()
    {
        fa_preview = transform.GetChild(0).gameObject;
        fa_preview = transform.GetChild(1).gameObject;
    }

    public void FA_preview_Hide()
    {
        fa_preview.SetActive(false);
    }
    public void FA_preview_Show()
    {
        fa_preview.SetActive(true);
    }
       
}
