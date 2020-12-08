using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System.IO;

public class VideoRenderer : MonoBehaviour
{
    string fileName;
    public RenderTexture cubeMap;
    public RenderTexture equirect;

    int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        fileName = Application.dataPath + "/Frames/file.png";
        Debug.Log(fileName);
    }

    private void RenderFrame()
    {
        cubeMap = new RenderTexture(1024, 1024, 16, RenderTextureFormat.ARGB32);
        cubeMap.dimension = TextureDimension.Cube;
        //equirect height should be twice the height of cubemap
        equirect = new RenderTexture(1920 , 1080, 16, RenderTextureFormat.ARGB32);

         
        Debug.Log(GetComponent<Camera>().RenderToCubemap(cubeMap, 63,Camera.MonoOrStereoscopicEye.Mono));
        cubeMap.ConvertToEquirect(equirect, Camera.MonoOrStereoscopicEye.Mono);

        Texture2D tempTexture = new Texture2D(equirect.width, equirect.height);
        // Copies EquirectTexture into the tempTexture
        RenderTexture currentActiveRT = RenderTexture.active;
        RenderTexture.active = equirect;
        tempTexture.ReadPixels(new Rect(0, 0, equirect.width, equirect.height), 0, 0);

        // Exports to a PNG
        var bytes = tempTexture.EncodeToPNG();
        fileName = Application.dataPath + "/Frames/file" + i.ToString() + ".png";
        i++;
        File.WriteAllBytes(fileName, bytes);

        // Restores the active render texture
        RenderTexture.active = currentActiveRT;
    }

    // Update is called once per frame
    void Update()
    {
       RenderFrame();
    }
}
