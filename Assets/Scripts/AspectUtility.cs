using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AspectUtility : MonoBehaviour
{
    Camera targetCamera;
    public float m_x_aspect = 6.0f;
    public float m_y_aspect = 16.0f;
    void Awake()
    {
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            // カメラを検索します。
            targetCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
            // 指定された比率からサイズを出します。
            Rect rect = calcAspect(m_x_aspect, m_y_aspect);
            // カメラの比率を変更します。
            targetCamera.rect = rect;
        };
    }
    // アスペクト比計算
    public Rect calcAspect(float width, float height)
    {
        float target_aspect = width / height;
        float window_aspect = (float)Screen.width / (float)Screen.height;
        float scale_height = window_aspect / target_aspect;
        Rect rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
        if (1.0f > scale_height)
        {
            rect.x = 0f;
            rect.y = (1f - scale_height) / 2.0f;
            rect.width = 1f;
            rect.height = scale_height;
        }
        else
        {
            float scale_width = 1f / scale_height;
            rect.x = (1f - scale_width) / 2f;
            rect.y = 0f;
            rect.width = scale_width;
            rect.height = 1f;
        }
        return rect;
    }
}