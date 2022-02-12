
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
        private float plantFlowerCooldown = 3f;

        [SerializeField]
        private Camera playerCamera;

        [SerializeField]
        private LayerMask envLayerMask;

        [SerializeField]
        private TerrainCollider terrain;

        private float lastPlantedFlowerTime;
        private float elapsedPlantedFlowerTime;

        private void Start()
        {
            lastPlantedFlowerTime = 0;
            elapsedPlantedFlowerTime = 0;
        }
        private void Update()
        {

            Ray ray = new(playerCamera.transform.position, playerCamera.transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * 10);
            elapsedPlantedFlowerTime += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (lastPlantedFlowerTime + elapsedPlantedFlowerTime > plantFlowerCooldown && Physics.Raycast(ray, 5f, envLayerMask))
                {
                    RaycastHit hit;
                    Physics.Raycast(ray, out hit, envLayerMask);
                    Debug.Log(hit);
                    lastPlantedFlowerTime = Time.time;
                    elapsedPlantedFlowerTime = 0;
                    Vector3 position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                    GameManager.Instance.PlantFlower(position);
                }
            }
        }

    }
}