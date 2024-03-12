using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCampManager : Singleton<BaseCampManager>
{
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
