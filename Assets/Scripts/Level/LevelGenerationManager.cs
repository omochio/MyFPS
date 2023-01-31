using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Level
{
    public class LevelGenerationManager : MonoBehaviour
    {
        // Prefab references
        [SerializeField] AssetReference m_obstacleRef;
        [SerializeField] AssetReference m_tunnelRef;

        // Parent transform of instantiated objects
        [SerializeField] Transform m_tunnelParentTransform;
        [SerializeField] Transform m_obstacleParentTransform;

        // Tunnel and Obstacles properties
        [SerializeField] Vector3 m_tunnelBeginPos;
        [SerializeField] uint m_tunnelCount;
        [SerializeField] Vector3 m_minObstacleScale;
        [SerializeField] Vector3 m_maxObstacleScale;
        [SerializeField] uint m_obstacleCountPerUnit;

        void Start()
        {
            // Addressable assets
            var obstacleHandle = m_obstacleRef.LoadAssetAsync<GameObject>();
            var tunnelHandle = m_tunnelRef.LoadAssetAsync<GameObject>();
            GameObject obstaclePrefab = obstacleHandle.WaitForCompletion();
            GameObject tunnelPrefab = tunnelHandle.WaitForCompletion();

            // Calc single tunnel range
            Bounds tunnelBounds = new(Vector3.zero, Vector3.zero);
            Vector3 tunnelScale = Vector3.zero;
            foreach (Transform child in tunnelPrefab.transform)
            {
                Vector3 temp = child.localScale;
                if (temp.x > tunnelScale.x)
                {
                    tunnelScale.x = temp.x;
                }
                if (temp.y > tunnelScale.y)
                {
                    tunnelScale.y = temp.y;
                }
                if (temp.z > tunnelScale.z)
                {
                    tunnelScale.z = temp.z;
                }
                tunnelBounds.Encapsulate(child.gameObject.GetComponent<MeshFilter>().sharedMesh.bounds);
            }
            // Fetch obstacle's bounds 
            Bounds obstacleBounds = obstaclePrefab.transform.GetComponent<MeshFilter>().sharedMesh.bounds;
            
            float tunnelPosZOffset = tunnelBounds.size.z * tunnelScale.z * tunnelPrefab.transform.localScale.z;
            for (var i = 0; i < m_tunnelCount; ++i)
            {
                // Place tunnels
                var tunnelObj = Instantiate(tunnelPrefab, m_tunnelBeginPos + (tunnelPosZOffset / 2 + i * tunnelPosZOffset) * Vector3.forward, Quaternion.identity, m_tunnelParentTransform);

                // Calc tunnel's significant positions used at placing obstacles
                Vector3 tunnelLeftButtomEdgePos = new(
                    tunnelObj.transform.position.x - tunnelBounds.extents.x * tunnelScale.x * tunnelObj.transform.localScale.x,
                    tunnelObj.transform.position.y - tunnelBounds.extents.y * tunnelScale.y * tunnelObj.transform.localScale.y,
                    tunnelObj.transform.position.z - tunnelBounds.extents.z * tunnelScale.z * tunnelObj.transform.localScale.z);
                Vector3 tunnelRightTopEdgePos = new(
                    tunnelLeftButtomEdgePos.x + tunnelBounds.size.x * tunnelScale.x * tunnelObj.transform.localScale.x,
                    tunnelLeftButtomEdgePos.y + tunnelBounds.size.y * tunnelScale.y * tunnelObj.transform.localScale.y,
                    tunnelLeftButtomEdgePos.z + tunnelBounds.size.z * tunnelScale.z * tunnelObj.transform.localScale.z);

                // Place obstacles
                for (var j = 0; j < m_obstacleCountPerUnit; ++j)
                {
                    Vector3 obstacleScale = new(
                        Random.Range(m_minObstacleScale.x, m_maxObstacleScale.x),
                        Random.Range(m_minObstacleScale.y, m_maxObstacleScale.y),
                        Random.Range(m_minObstacleScale.z, m_maxObstacleScale.z));
                    Vector3 obstaclePos = new(
                        Random.Range(tunnelLeftButtomEdgePos.x + obstacleBounds.extents.x * obstacleScale.x, tunnelRightTopEdgePos.x - obstacleBounds.extents.x * obstacleScale.x),
                        Random.Range(tunnelLeftButtomEdgePos.y + obstacleBounds.extents.y * obstacleScale.y, tunnelRightTopEdgePos.y - obstacleBounds.extents.y * obstacleScale.y),
                        Random.Range(tunnelLeftButtomEdgePos.z + obstacleBounds.extents.z * obstacleScale.z, tunnelRightTopEdgePos.z - obstacleBounds.extents.z * obstacleScale.z));

                    var obstacleObj = Instantiate(
                        obstaclePrefab,
                        obstaclePos,
                        Quaternion.identity,
                        m_obstacleParentTransform);
                    obstacleObj.transform.localScale = obstacleScale;
                }
            }
        }
    }
}

