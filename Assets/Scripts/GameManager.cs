using System.Collections;
using System.Collections.Generic;

using DG.Tweening;

using UnityEngine;
using UnityEngine.Device;
using UnityEngine.Networking;

using UnityStandardAssets.Characters.FirstPerson;

namespace Kawzar.Memento.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField]
        private string serverUrl;

        [SerializeField]
        private GameObject[] flowerPrefab;

        [SerializeField]
        private Player player;

        [SerializeField]
        private FirstPersonController playerController;

        [SerializeField]
        private CanvasGroup mainCanvas;

        [SerializeField]
        private Transform flowerContainer;

        private string getUrl, postUrl;

        private IList<Flower> flowers = new List<Flower>();
        private FlowerResult flowersFromJson;

        private void Awake()
        {
            if(Instance == null) Instance = this;

            DontDestroyOnLoad(gameObject);
            serverUrl = serverUrl.Trim();
            getUrl = $"{serverUrl}flowers";
            postUrl = $"{serverUrl}flower";

            flowers.Clear();
            flowersFromJson = null;
            
            StartCoroutine(GetFlowers());
            mainCanvas.DOFade(0, 10f).OnComplete(() =>
            {
                mainCanvas.gameObject.SetActive(false);
                player.enabled = true;
                playerController.enabled = true;
            });
        }

        public void PlantFlower(Vector3 position)
        {
            Flower toPlant = new() { positionX = position.x, positionY = position.y, positionZ = position.z };
            flowers.Add(toPlant);
            var flowerInstance = Instantiate(GetRandomFlowerAsset());
            flowerInstance.transform.position = position;
            flowerInstance.transform.SetParent(flowerContainer, true);
            flowerInstance.SetActive(true);
            flowerInstance.transform.DOPunchScale(Vector3.up, 0.75f, 5, 0.5f);
            StartCoroutine(PostFlower(toPlant));
        }

        private IEnumerator GetFlowers()
        {
            using (UnityWebRequest www = UnityWebRequest.Get(getUrl))
            {
                www.SetRequestHeader("Content-Type", "application/json");

                yield return www.SendWebRequest();
                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    if (www.isDone)
                    {
                        var result = www.downloadHandler.text;
                        flowersFromJson = JsonUtility.FromJson<FlowerResult>(result);
                        InitializeFlowers();
                    }
                }
            }
        }

        private GameObject GetRandomFlowerAsset()
        {
            return flowerPrefab[Random.Range(0, flowerPrefab.Length)];
        }

        private IEnumerator PostFlower(Flower flower)
        {
            string json = JsonUtility.ToJson(flower);

            var req = new UnityWebRequest(postUrl, "POST");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            req.uploadHandler = new UploadHandlerRaw(jsonToSend);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            yield return req.SendWebRequest();
        }

        public void Quit()
        {
            UnityEngine.Application.Quit();
        }

        private void InitializeFlowers()
        {
            for (int i = 0; i < flowersFromJson.flowers.Length; i++)
            {
                var flowerInstance = Instantiate(GetRandomFlowerAsset());
                Flower flower = flowersFromJson.flowers[i];
                var position = new Vector3(flower.positionX, flower.positionY, flower.positionZ);
                flowerInstance.transform.position = position;
                flowerInstance.transform.SetParent(flowerContainer, true);
                flowerInstance.SetActive(true);
                flowers.Add(flower);
            }            
        }
    }
}