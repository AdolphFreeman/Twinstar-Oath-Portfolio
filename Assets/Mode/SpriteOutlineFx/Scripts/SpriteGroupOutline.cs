using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/2D Sprite/Group Outline")]
public class SpriteGroupOutline : MonoBehaviour
{
    public Camera targetCamera;
    public Color outlineColor = Color.red;
    [Range(0, 10)]
    public float outlineDistance = 1.0f;
    
    private CommandBuffer cb;
    private Material maskMaterial;
    private Material combineMaterial;

    void OnEnable()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        if (targetCamera == null) return;

        cb = new CommandBuffer();
        cb.name = "Sprite Group Outline: " + this.gameObject.name;

        maskMaterial = new Material(Shader.Find("Hidden/SpriteGroupMask"));
        combineMaterial = new Material(Shader.Find("Hidden/SpriteGroupOutlineCombine"));

        targetCamera.AddCommandBuffer(CameraEvent.BeforeForwardAlpha, cb);
    }

    void OnDisable()
    {
        if (targetCamera != null && cb != null)
        {
            targetCamera.RemoveCommandBuffer(CameraEvent.BeforeForwardAlpha, cb);
        }
    }

    void LateUpdate()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
            if (targetCamera != null && cb != null)
                targetCamera.AddCommandBuffer(CameraEvent.BeforeForwardAlpha, cb);
        }

        if (cb == null || targetCamera == null || maskMaterial == null || combineMaterial == null) return;

        cb.Clear();

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return;

        int maskId = Shader.PropertyToID("_SpriteGroupMaskTex");
        cb.GetTemporaryRT(maskId, -1, -1, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);

        cb.SetRenderTarget(maskId);
        cb.ClearRenderTarget(false, true, Color.clear);

        int mainTexId = Shader.PropertyToID("_MainTex");

        foreach (var r in renderers)
        {
            if (r is SpriteRenderer)
            {
                cb.DrawRenderer(r, maskMaterial);
            }
            else if (r is MeshRenderer || r is SkinnedMeshRenderer)
            {
                Material[] sharedMats = r.sharedMaterials;
                for (int i = 0; i < sharedMats.Length; i++)
                {
                    if (sharedMats[i] != null && sharedMats[i].HasProperty(mainTexId))
                    {
                        var tex = sharedMats[i].GetTexture(mainTexId);
                        if (tex != null)
                        {
                            cb.SetGlobalTexture(mainTexId, tex);
                            cb.DrawRenderer(r, maskMaterial, i);
                        }
                        else
                        {
                            cb.DrawRenderer(r, maskMaterial, i);
                        }
                    }
                    else
                    {
                        cb.DrawRenderer(r, maskMaterial, i);
                    }
                }
            }
        }

        combineMaterial.SetColor("_Color", outlineColor);
        combineMaterial.SetFloat("_Distance", outlineDistance);

        cb.Blit(maskId, BuiltinRenderTextureType.CameraTarget, combineMaterial);

        cb.ReleaseTemporaryRT(maskId);
    }
}
