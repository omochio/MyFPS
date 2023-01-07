using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] AssetReference reference;
    [SerializeField] Transform parentTransform;
    [SerializeField] float spawnTime;
    [SerializeField] int enemyCountLimit;

    int enemyCount;
    Vector3 spawnPosition;

    float timeCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        AsyncOperationHandle handle = reference.LoadAssetAsync<GameObject>();
        handle.Completed += Handle_Completed;
    }

    void Handle_Completed(AsyncOperationHandle obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($"AssetReference {reference.RuntimeKey} Succeeded to load.");
        }
        else
        {
            Debug.LogError($"AssetReference {reference.RuntimeKey} failed to load.");
        }
    }

    void Update()
    {
        if (enemyCount > enemyCountLimit)
        {
            return;
        }

        timeCount += Time.deltaTime;

        if (timeCount >= spawnTime && reference.IsDone)
        {
            spawnPosition.Set(
            Random.Range(-50.0f, 50.0f),
            Random.Range(-50.0f, 50.0f),
            Random.Range(-50.0f, 50.0f));
            Instantiate(reference.Asset, spawnPosition, Quaternion.identity, parentTransform);
            Debug.Log($"Enemy popped! Count: {enemyCount}");
            timeCount = 0;
            enemyCount += 1;
        }
    }

    void OnDestroy()
    {
        reference.ReleaseAsset();
    }
}
