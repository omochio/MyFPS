using UnityEngine;
using GameSystem;
using Level;

public class ProgressManager : MonoBehaviour
{
    [SerializeField] LevelGenerationManager levelGenerationManager;
    [SerializeField] GameObject goalUI;
    [SerializeField] GameObject playerObj;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.CreateEvent("Goal");
        EventManager.StartListening("Goal", OnGoal);
    }

    void Update()
    {
        if (playerObj.transform.position.z >= levelGenerationManager.tunnelEndZPos)
        {
            EventManager.TriggerEvent("Goal");
        }
    }

    void OnDestroy()
    {
        EventManager.StopListening("Goal", OnGoal);
    }

    void OnGoal()
    {
        playerObj.SetActive(false);
        goalUI.SetActive(true);
        EventManager.StopListening("Goal", OnGoal);
    }
}
