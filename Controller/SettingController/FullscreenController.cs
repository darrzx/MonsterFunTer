using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullscreenController : MonoBehaviour
{
    public void SetFullscreen(bool isFullscreen)
    {
        if (isFullscreen)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }
}
