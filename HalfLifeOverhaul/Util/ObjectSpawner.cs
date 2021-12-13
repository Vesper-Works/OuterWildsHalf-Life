using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HalfLifeOverhaul.Util
{
    public class ObjectSpawner : MonoBehaviour
    {
        private void Update()
        {
            if (!OWInput.IsInputMode(InputMode.Menu))
            {
                if (Keyboard.current != null && Keyboard.current[Key.P].wasReleasedThisFrame)
                {
                    // Raycast
                    Locator.GetPlayerBody().DisableCollisionDetection();
                    int layerMask = OWLayerMask.physicalMask;
                    var origin = Locator.GetActiveCamera().transform.position;
                    var direction = Locator.GetActiveCamera().transform.TransformDirection(Vector3.forward);
                    if (Physics.Raycast(origin, direction, out RaycastHit hitInfo, 100f, layerMask))
                    {
                        var newObj = GameObject.Instantiate(MeshPatcher.barnaclePrefab, hitInfo.transform);
                        newObj.transform.localScale = Vector3.one * 0.05f;
                        newObj.transform.position = hitInfo.point;
                        newObj.transform.rotation = Quaternion.LookRotation(Vector3.left, -hitInfo.normal);
                        newObj.SetActive(true);

                        Logger.Log($"Spawned {newObj.name} at {newObj.transform.localPosition} with rotation {newObj.transform.localRotation} parented to {Utility.GetPath(newObj.transform.parent)}");
                    }
                    Locator.GetPlayerBody().EnableCollisionDetection();
                }
            }
        }
    }
}
