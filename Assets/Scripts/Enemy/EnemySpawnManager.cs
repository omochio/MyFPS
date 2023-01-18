using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Enemy
{
    public class EnemySpawnManager : MonoBehaviour
    {
        [SerializeField] AssetReference enemyRef;
        [SerializeField] Transform parentTransform;
        [SerializeField] float spawnTime;
        [SerializeField] int enemyCountLimit;

        int m_enemyCount;
        Vector3 m_spawnPosition;

        float m_timeCount = 0;

        void Awake()
        {
            AsyncOperationHandle handle = enemyRef.LoadAssetAsync<GameObject>();
            handle.Completed += Handle_Completed;
        }

        void Handle_Completed(AsyncOperationHandle obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log($"AssetReference {enemyRef.RuntimeKey} Succeeded to load.");
            }
            else
            {
                Debug.LogError($"AssetReference {enemyRef.RuntimeKey} failed to load.");
            }
        }

        void Update()
        {
            if (m_enemyCount > enemyCountLimit)
            {
                return;
            }

            m_timeCount += Time.deltaTime;

            if (m_timeCount >= spawnTime && enemyRef.IsDone)
            {
                m_spawnPosition.Set(
                Random.Range(-50.0f, 50.0f),
                Random.Range(-50.0f, 50.0f),
                Random.Range(-50.0f, 50.0f));
                Instantiate(enemyRef.Asset, m_spawnPosition, Quaternion.identity, parentTransform);
                //Debug.Log($"Enemy popped! Count: {m_enemyCount}");
                m_timeCount = 0;
                m_enemyCount += 1;
            }
        }

        void OnDestroy()
        {
            enemyRef.ReleaseAsset();
        }
    }
}
