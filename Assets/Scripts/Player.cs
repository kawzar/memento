
using Cysharp.Threading.Tasks;

using UnityEngine;

namespace Kawzar.Memento.Scripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private Transform rayOrigin;

        [SerializeField]
        private float rayLength;

        [SerializeField]
        private LayerMask interactableLayer;

        private void Update()
        {
                RaycastHit hit;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (Physics.Raycast(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward), out hit, rayLength, interactableLayer))
                    {
                        Debug.DrawRay(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                        GameManager.Instance.PlantFlower(hit.point);
                    }
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    UIManager.Instance.ShowGameplayMenu();
                }

        }

    }
}