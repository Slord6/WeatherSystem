//heavily based on code from:
//http://blog.theknightsofunity.com/make-it-snow-fast-screen-space-snow-shader/

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ScreenSpaceSnow : MonoBehaviour
{
    [SerializeField]
    private Texture2D snowTexture;
    [SerializeField]
    private Color snowColor = Color.white;
    [SerializeField]
    private float snowTextureScale = 0.1f;

    [SerializeField]
    [Range(0, 1)]
    private float bottomThreshold = 0f;
    [SerializeField]
    [Range(0, 1)]
    private float topThreshold = 1f;

    private Material _material;
    private new Camera camera;
    [SerializeField]
    private bool isEnabled;

    public float TopThreshold
    {
        get
        {
            return topThreshold;
        }
        set
        {
            topThreshold = value;
            if (topThreshold > 1)
            {
                topThreshold = 1;
            }
            else if (topThreshold < 0)
            {
                topThreshold = 0;
            }
        }
    }
    public float BottomThreshold
    {
        get
        {
            return bottomThreshold;
        }
        set
        {
            bottomThreshold = value;
            if (bottomThreshold > 1)
            {
                bottomThreshold = 1;
            }
            else if (bottomThreshold < 0)
            {
                bottomThreshold = 0;
            }
        }
    }

    public bool IsEnabled
    {
        get
        {
            return isEnabled;
        }
    }

    private void OnEnable()
    {
        // dynamically create a material that will use our shader
        camera = GetComponent<Camera>();
        _material = new Material(Shader.Find("TKoU/ScreenSpaceSnow"));

        // tell the camera to render depth and normals
        camera.depthTextureMode |= DepthTextureMode.DepthNormals;

        //initially disable
        isEnabled = false;
    }

    public void Enable()
    {
        isEnabled = true;
    }

    public void Disable()
    {
        isEnabled = false;
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (isEnabled)
        {
            // set shader properties
            _material.SetMatrix("_CamToWorld", camera.cameraToWorldMatrix);
            _material.SetColor("_SnowColor", snowColor);
            _material.SetFloat("_BottomThreshold", bottomThreshold);
            _material.SetFloat("_TopThreshold", TopThreshold);
            _material.SetTexture("_SnowTex", snowTexture);
            _material.SetFloat("_SnowTexScale", snowTextureScale);

            // execute the shader on input texture (src) and write to output (dest)
            Graphics.Blit(src, dest, _material);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}