using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Enemy
{
    public class EnemySpawnManager : MonoBehaviour
    {
        [SerializeField] AssetReference m_enemyRef;
        [SerializeField] Transform m_parentTransform;
        [SerializeField] float m_spawnTime;
        [SerializeField] int m_enemyCountLimit;

        int m_enemyCount;
        Vector3 m_spawnPosition;

        float m_timeCount = 0;

        void Awake()
        {
            AsyncOperationHandle handle = m_enemyRef.LoadAssetAsync<GameObject>();
            handle.Completed += Handle_Completed;
        }

        void Handle_Completed(AsyncOperationHandle obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log($"AssetReference {m_enemyRef.RuntimeKey} Succeeded to load.");
            }
            else
            {
                Debug.LogError($"AssetReference {m_enemyRef.RuntimeKey} failed to load.");
            }
        }

        void Update()
        {
            if (m_enemyCount > m_enemyCountLimit)
            {
                return;
            }

            m_timeCount += Time.deltaTime;

            if (m_timeCount >= m_spawnTime && m_enemyRef.IsDone)
            {
                m_spawnPosition.Set(
                Random.Range(-50.0f, 50.0f),
                Random.Range(-50.0f, 50.0f),
                Random.Range(-50.0f, 50.0f));
                Instantiate(m_enemyRef.Asset, m_spawnPosition, Quaternion.identity, m_parentTransform);
                //Debug.Log($"Enemy popped! Count: {m_enemyCount}");
                m_timeCount = 0;
                m_enemyCount += 1;
            }
        }

        void OnDestroy()
        {
            m_enemyRef.ReleaseAsset();
        }
    }
}
