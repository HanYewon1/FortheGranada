using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class submenuUI : MonoBehaviour
{
    public void OnClickCloseButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
