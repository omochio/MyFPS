using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Level
{
    public class LevelGenerationManager : MonoBehaviour
    {
        [SerializeField] AssetReference m_obstacleRef;
        [SerializeField] AssetReference m_tunnelRef;
        [SerializeField] Transform m_tunnelParentTransform;
        [SerializeField] Transform m_obstacleParentTransform;
        [SerializeField] Vector3 m_tunnelBeginPos;
        [SerializeField] uint m_tunnelCount;
        [SerializeField] Vector3 m_minObstacleScale;
        [SerializeField] Vector3 m_maxObstacleScale;
        [SerializeField] uint m_obstacleCountPerUnit;

        void Start()
        {
            var obstacleHandle = m_obstacleRef.LoadAssetAsync<GameObject>();
            var tunnelHandle = m_tunnelRef.LoadAssetAsync<GameObject>();
            GameObject obstaclePrefab = obstacleHandle.WaitForCompletion();
            GameObject tunnelPrefab = tunnelHandle.WaitForCompletion();

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
            Bounds obstacleBounds = obstaclePrefab.transform.GetComponent<MeshFilter>().sharedMesh.bounds;
            
            float tunnelPosZOffset = tunnelBounds.size.z * tunnelScale.z * tunnelPrefab.transform.localScale.z;
            for (var i = 0; i < m_tunnelCount; ++i)
            {
                var tunnelObj = Instantiate(tunnelPrefab, m_tunnelBeginPos + (tunnelPosZOffset / 2 + i * tunnelPosZOffset) * Vector3.forward, Quaternion.identity, m_tunnelParentTransform);
                Vector3 tunnelLeftButtomEdgePos = new(
                    tunnelObj.transform.position.x - tunnelBounds.extents.x * tunnelScale.x * tunnelObj.transform.localScale.x,
                    tunnelObj.transform.position.y - tunnelBounds.extents.y * tunnelScale.y * tunnelObj.transform.localScale.y,
                    tunnelObj.transform.position.z - tunnelBounds.extents.z * tunnelScale.z * tunnelObj.transform.localScale.z);
                Vector3 tunnelRightTopEdgePos = new(
                    tunnelLeftButtomEdgePos.x + tunnelBounds.size.x * tunnelScale.x * tunnelObj.transform.localScale.x,
                    tunnelLeftButtomEdgePos.y + tunnelBounds.size.y * tunnelScale.y * tunnelObj.transform.localScale.y,
                    tunnelLeftButtomEdgePos.z + tunnelBounds.size.z * tunnelScale.z * tunnelObj.transform.localScale.z);

                //GameObject prevObstacleObj = null;
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

                    //if (prevObstacleObj)
                    //{
                    //    if (obstacleObj.GetComponent<Collider>().CompareTag("Obstacle"))
                    //    {
                    //        Vector3 obstacleLeftButtomEdgePos = new(
                    //            obstacleObj.transform.position.x - obstacleBounds.extents.x * obstacleObj.transform.localScale.x,
                    //            obstacleObj.transform.position.y - obstacleBounds.extents.y * obstacleObj.transform.localScale.y,
                    //            obstacleObj.transform.position.z - obstacleBounds.extents.z * obstacleObj.transform.localScale.z);
                    //        Vector3 obstacleRightButtomEdgePos = new(
                    //            obstacleLeftButtomEdgePos.x + obstacleBounds.size.x * obstacleObj.transform.localScale.x,
                    //            obstacleLeftButtomEdgePos.y,
                    //            obstacleLeftButtomEdgePos.z + obstacleBounds.size.z * obstacleObj.transform.localScale.z);
                    //        Vector3 obstacleLeftTopEdgePos = new(
                    //            obstacleObj.transform.position.x - obstacleBounds.extents.x * obstacleObj.transform.localScale.x,
                    //            obstacleLeftButtomEdgePos.y + obstacleBounds.size.y * obstacleObj.transform.localScale.y,
                    //            obstacleObj.transform.position.z - obstacleBounds.extents.z * obstacleObj.transform.localScale.z);
                    //        Vector3 obstacleRightTopEdgePos = new(
                    //            obstacleLeftTopEdgePos.x + obstacleBounds.size.x * obstacleObj.transform.localScale.x,
                    //            obstacleLeftTopEdgePos.y,
                    //            obstacleLeftTopEdgePos.z + obstacleBounds.size.z * obstacleObj.transform.localScale.z);
                    //        Vector3 obstacleLeftCenterEdgePos = new(
                    //            obstacleObj.transform.position.x - obstacleBounds.extents.x * obstacleObj.transform.localScale.x,
                    //            obstacleObj.transform.position.y,
                    //            obstacleObj.transform.position.z - obstacleBounds.extents.z * obstacleObj.transform.localScale.z);
                    //        Vector3 obstacleRightCenterEdgePos = new(
                    //            obstacleLeftCenterEdgePos.x + obstacleBounds.size.x * obstacleObj.transform.localScale.x,
                    //            obstacleObj.transform.position.y,
                    //            obstacleLeftCenterEdgePos.z + obstacleBounds.size.z * obstacleObj.transform.localScale.z);
                    //        Vector3 prevObstacleLeftButtomEdgePos = new(
                    //            prevObstacleObj.transform.position.x - obstacleBounds.extents.x * prevObstacleObj.transform.localScale.x ,
                    //            prevObstacleObj.transform.position.y - obstacleBounds.extents.y * prevObstacleObj.transform.localScale.y,
                    //            prevObstacleObj.transform.position.z - obstacleBounds.extents.z * prevObstacleObj.transform.localScale.z);
                    //        Vector3 prevObstacleRightButtomEdgePos = new(
                    //            prevObstacleLeftButtomEdgePos.x + obstacleBounds.size.x * prevObstacleObj.transform.localScale.x,
                    //            prevObstacleLeftButtomEdgePos.y,
                    //            prevObstacleLeftButtomEdgePos.z + obstacleBounds.size.z * prevObstacleObj.transform.localScale.z);
                    //        Vector3 prevObstacleLeftTopEdgePos = new(
                    //            prevObstacleObj.transform.position.x - obstacleBounds.extents.x * prevObstacleObj.transform.localScale.x,
                    //            prevObstacleLeftButtomEdgePos.y + obstacleBounds.size.y * prevObstacleObj.transform.localScale.y,
                    //            prevObstacleObj.transform.position.z - obstacleBounds.extents.z * prevObstacleObj.transform.localScale.z);
                    //        Vector3 prevObstacleRightTopEdgePos = new(
                    //            prevObstacleLeftTopEdgePos.x + obstacleBounds.size.x * prevObstacleObj.transform.localScale.x,
                    //            prevObstacleLeftTopEdgePos.y,
                    //            prevObstacleLeftTopEdgePos.z + obstacleBounds.size.z * prevObstacleObj.transform.localScale.z);
                    //        Vector3 prevObstacleLeftCenterEdgePos = new(
                    //            prevObstacleObj.transform.position.x - obstacleBounds.size.x * prevObstacleObj.transform.localScale.x,
                    //            prevObstacleObj.transform.position.y,
                    //            prevObstacleObj.transform.position.z - obstacleBounds.size.z * prevObstacleObj.transform.localScale.z);
                    //        Vector3 prevObstacleRightCenterEdgePos = new(
                    //            prevObstacleLeftCenterEdgePos.x + obstacleBounds.size.x * prevObstacleObj.transform.localScale.x,
                    //            prevObstacleObj.transform.position.y,
                    //            prevObstacleLeftCenterEdgePos.z + obstacleBounds.size.z * prevObstacleObj.transform.localScale.z);

                    //        Vector3 displacement = prevObstacleObj.transform.position - obstacleObj.transform.position;

                    //        if (displacement.x < 0 && displacement.y < 0)
                    //        {
                    //            obstacleObj.transform.position = prevObstacleLeftButtomEdgePos;
                    //        }
                    //        else if (displacement.x < 0 && displacement.y >= 0)
                    //        {
                    //            obstacleObj.transform.position = prevObstacleLeftTopEdgePos;
                    //        }
                    //        else if (displacement.x >= 0 && displacement.y < 0)
                    //        {
                    //            obstacleObj.transform.position = prevObstacleRightButtomEdgePos;
                    //        }
                    //        else
                    //        {
                    //            obstacleObj.transform.position = prevObstacleRightTopEdgePos
                    //        }
                    //    }
                    //}

                    //prevObstacleObj = obstacleObj;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}

