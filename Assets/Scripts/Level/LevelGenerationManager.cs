using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Level
{

    public class LevelGenerationManager : MonoBehaviour
    {
        // Prefab references
        [SerializeField] AssetReference m_obstacleRef;
        [SerializeField] AssetReference m_tunnelRef;
        [SerializeField] AssetReference m_targetRef;

        // Parent transform of instantiated objects
        [SerializeField] Transform m_tunnelParentTransform;
        [SerializeField] Transform m_obstacleParentTransform;
        [SerializeField] Transform m_targetParentTransform;

        // Tunnel properties
        [SerializeField] Vector3 m_tunnelBeginPos;
        [SerializeField] uint m_tunnelCount;

        // Obstacle properties
        [SerializeField] Vector3 m_minObstacleScale;
        [SerializeField] Vector3 m_maxObstacleScale;
        [SerializeField] uint m_obstacleCountPerUnit;

        // Target properties
        [SerializeField] Vector3 m_minTargetScale;
        [SerializeField] Vector3 m_maxTargetScale;
        [SerializeField] uint m_targetCountPerUnit;

        // Tunnel end position
        float m_tunnelEndZPos;
        public float tunnelEndZPos { get { return m_tunnelEndZPos; } }

        void Start()
        {
            // Load addressable assets
            var tunnelHandle = m_tunnelRef.LoadAssetAsync<GameObject>();
            var obstacleHandle = m_obstacleRef.LoadAssetAsync<GameObject>();
            var targetHandle = m_targetRef.LoadAssetAsync<GameObject>();
            GameObject tunnelPrefab = tunnelHandle.WaitForCompletion();
            GameObject obstaclePrefab = obstacleHandle.WaitForCompletion();
            GameObject targetPrefab = targetHandle.WaitForCompletion();

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
                PlaceObjectsRomdomInTunnel(obstaclePrefab, m_obstacleParentTransform, m_minObstacleScale, m_maxObstacleScale, m_obstacleCountPerUnit, tunnelLeftButtomEdgePos, tunnelRightTopEdgePos);

                // Place targets
                PlaceObjectsRomdomInTunnel(targetPrefab, m_targetParentTransform, m_minTargetScale, m_maxTargetScale, m_targetCountPerUnit, tunnelLeftButtomEdgePos, tunnelRightTopEdgePos);
            }

            m_tunnelEndZPos = m_tunnelBeginPos.z + tunnelBounds.size.z * tunnelScale.z * tunnelPrefab.transform.localScale.z * m_tunnelCount;
        }

        void PlaceObjectsRomdomInTunnel(GameObject prefab, Transform parentTransform, Vector3 minScale, Vector3 maxScale, uint count, Vector3 tunnelLeftButtomEdgePos, Vector3 tunnelRightTopEdgePos)
        {
            Bounds bounds = prefab.GetComponent<MeshFilter>().sharedMesh.bounds;
            for (var i = 0; i < count; ++i)
            {
                Vector3 scale = new(
                    Random.Range(minScale.x, maxScale.x),
                    Random.Range(minScale.y, maxScale.y),
                    Random.Range(minScale.z, maxScale.z));

                Vector3 pos = new(
                    Random.Range(tunnelLeftButtomEdgePos.x + bounds.extents.x * scale.x, tunnelRightTopEdgePos.x - bounds.extents.x * scale.x),
                    Random.Range(tunnelLeftButtomEdgePos.y + bounds.extents.y * scale.y, tunnelRightTopEdgePos.y - bounds.extents.y * scale.y),
                    Random.Range(tunnelLeftButtomEdgePos.z + bounds.extents.z * scale.z, tunnelRightTopEdgePos.z - bounds.extents.z * scale.z));

                var obj = Instantiate(
                    prefab,
                    pos,
                    Quaternion.identity,
                    parentTransform);
                obj.transform.localScale = scale;
            }
        }
    }
}

