using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameSystem
{
    public class EventManager : MonoBehaviour
    {
        private Dictionary<string, UnityEvent> m_eventDict;

        private static EventManager m_eventManager;

        public static EventManager instance
        {
            get
            {
                if (!m_eventManager)
                {
                    m_eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                    if (!m_eventManager)
                    {
                        Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
                    }
                    else
                    {
                        m_eventManager.Init();

                        DontDestroyOnLoad(m_eventManager);
                    }
                }
                return m_eventManager;
            }
        }

        void Init()
        {
            if (m_eventDict == null)
            {
                m_eventDict = new Dictionary<string, UnityEvent>();
            }
        }

        public static void CreateEvent(string eventName)
        {
            UnityEvent newEvent = new();
            instance.m_eventDict.Add(eventName, newEvent);
        }

        public static void StartListening(string eventName, UnityAction listener)
        {
            UnityEvent thisEvent;

            if (instance.m_eventDict.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                Debug.LogError($"{eventName} event not found");
            }
        }

        public static void StopListening(string eventName, UnityAction listener)
        {
            if (m_eventManager == null) return;
            UnityEvent thisEvent = null;
            if (instance.m_eventDict.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        public static void TriggerEvent(string eventName)
        {
            UnityEvent thisEvent = null;
            if (instance.m_eventDict.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke();
            }
            else
            {
                Debug.LogError($"{eventName} event not found");
            }
        }
    }
}
