using UnityEngine;
using Spine.Unity;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SpineShadowFollow : MonoBehaviour
{
    public SkeletonAnimation sourceSkeleton; // 拖入本體
    private MeshFilter shadowMeshFilter;
    private MeshRenderer shadowMeshRenderer;

    void Start()
    {
        shadowMeshFilter = GetComponent<MeshFilter>();
        shadowMeshRenderer = GetComponent<MeshRenderer>();
        
        // 確保影子使用你的 Skew Shader 材質
        // 建議在 Inspector 直接指派好 Material
    }

    // 使用 LateUpdate 確保在本體骨骼計算完後才抓取
    void LateUpdate()
    {
        if (sourceSkeleton == null) return;

        // 直接共用本體的 Mesh
        // 注意：如果本體有多個 Submesh (多張貼圖)，這裡需要更複雜的處理
        Mesh sourceMesh = sourceSkeleton.GetComponent<MeshFilter>().sharedMesh;
        if (sourceMesh != null)
        {
            shadowMeshFilter.sharedMesh = sourceMesh;
        }

        // 保持位置同步，但你可以根據需求加上位移
        transform.position = sourceSkeleton.transform.position;
        transform.rotation = sourceSkeleton.transform.rotation;
        transform.localScale = sourceSkeleton.transform.localScale;
    }
}