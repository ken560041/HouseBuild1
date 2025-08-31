using UnityEngine;
using System.IO;

public class ModelToSpriteSaver : MonoBehaviour
{
    public Camera renderCamera;
    public Transform modelToCapture;
    public int textureSize = 256;
    public float paddingMultiplier = 1.2f; // thêm khoảng trống

    [ContextMenu("Capture Transparent Sprite")]
    public void Capture()
    {
        modelToCapture=gameObject.GetComponent<Transform>();
        if (renderCamera == null || modelToCapture == null)
        {
            Debug.LogError("Camera hoặc Model chưa gán!");
            return;
        }

        SetupCamera();

        // Setup RenderTexture
        RenderTexture rt = new RenderTexture(textureSize, textureSize, 24, RenderTextureFormat.ARGB32);
        string name =gameObject.name;
        rt.antiAliasing = 8;
        renderCamera.targetTexture = rt;

        // Clear background với alpha 0
        renderCamera.backgroundColor = new Color(0, 0, 0, 0);
        renderCamera.clearFlags = CameraClearFlags.SolidColor;

        // Render
        renderCamera.Render();

        // Copy từ RenderTexture ra Texture2D
        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, textureSize, textureSize), 0, 0);
        tex.Apply();

        // Cleanup
        renderCamera.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(rt);

        // Encode PNG giữ alpha
        byte[] bytes = tex.EncodeToPNG();
        string path = Application.dataPath +"/"+name +".png";
        File.WriteAllBytes(path, bytes);
        Debug.Log("Saved Transparent Sprite at: " + path);

        // Nếu muốn tạo Sprite runtime:
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        // Bạn có thể dùng sprite này luôn trong game!
    }

    private void SetupCamera()
    {
        Bounds bounds = CalculateBounds(modelToCapture);

        // Lấy size lớn nhất
        float maxSize = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z) * paddingMultiplier;

        // Camera field of view
        float fov = renderCamera.fieldOfView;

        // Tính khoảng cách cần thiết
        float distance = (maxSize) / (2f * Mathf.Tan(Mathf.Deg2Rad * fov * 0.5f));

        // Set vị trí và nhìn vào model
        renderCamera.transform.position = bounds.center - renderCamera.transform.forward * distance;
        renderCamera.transform.LookAt(bounds.center);

        // Gần xa clip plane
        renderCamera.nearClipPlane = 0.01f;
    }

    private Bounds CalculateBounds(Transform root)
    {
        Renderer[] renderers = root.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
            return new Bounds(root.position, Vector3.one * 0.1f); // fallback

        Bounds bounds = renderers[0].bounds;
        foreach (Renderer r in renderers)
        {
            bounds.Encapsulate(r.bounds);
        }
        return bounds;
    }
}
