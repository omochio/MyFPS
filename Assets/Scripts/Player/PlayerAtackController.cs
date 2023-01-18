using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Color = UnityEngine.Color;
using System;
using Unity.VisualScripting;
using System.Data;

namespace Player
{
    public class PlayerAtackController : MonoBehaviour
    {
        [SerializeField] AssetReference rayRef;

        Ray m_ray;

        void Awake()
        {
            AsyncOperationHandle handle = rayRef.LoadAssetAsync<GameObject>();
            handle.Completed += Handle_Completed;
        }

        void Handle_Completed(AsyncOperationHandle obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                if (obj.Status == AsyncOperationStatus.Succeeded)
                {
                    Debug.Log($"AssetReference {rayRef.RuntimeKey} Succeeded to load.");
                }
                else
                {
                    Debug.LogError($"AssetReference {rayRef.RuntimeKey} failed to load.");
                }
            }
        }

        void FixedUpdate()
        {
            if (Mouse.current.leftButton.isPressed)
            {
                m_ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                if (Physics.Raycast(m_ray, out RaycastHit hit))
                {
                    if (hit.collider.gameObject.CompareTag("Destroyable"))
                    {
                        DrawRay(hit);
                        Debug.Log("hit");
                        Destroy(hit.collider.gameObject);
                    }
                    else
                    {
                        DrawRay();
                    }
                }
                m_ray = new();
            }
        }


        void DrawRay()
        {
            var ray = Instantiate(rayRef.Asset, m_ray.origin, Quaternion.identity);
            var lR = ray.GetComponent<LineRenderer>();
            lR.positionCount = 2;
            lR.SetPosition(0, m_ray.origin);
            lR.SetPosition(1, m_ray.direction * 1000);
        }

        void DrawRay(RaycastHit hit)
        {
            var ray = Instantiate(rayRef.Asset, m_ray.origin, Quaternion.identity);
            var lR = ray.GetComponent<LineRenderer>();
            lR.positionCount = 2;
            lR.SetPosition(0, m_ray.origin);
            lR.SetPosition(1, m_ray.direction * hit.distance);
        }
    }
}

