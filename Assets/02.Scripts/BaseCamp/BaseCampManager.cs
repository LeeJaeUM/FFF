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

       
}
