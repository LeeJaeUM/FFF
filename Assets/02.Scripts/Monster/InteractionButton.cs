using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionButton : MonoBehaviour
{
    [SerializeField]
    private GameObject trap;
    [SerializeField]
    private GameObject pictureFrame;

    public void PictureFrameButton1()
    {
        Debug.Log("버튼 클릭");
        //trap.SetActive(true);
        //pictureFrame.SetActive(false);
    }
}
