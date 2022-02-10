using System.Collections;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.Networking;

namespace Kawzar.Memento.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField]
        private string serverUrl;

        [SerializeField]
        private GameObject[] flowerPrefab;

        private string getUrl, postUrl, homeUrl;

        private IList<Flower> flowers = new List<Flower>();
        private FlowerResult flowersFromJson;

        private async void Awake()
        {
            Instance = this;

            serverUrl = serverUrl.Trim();
            getUrl = $"{serverUrl}flowers";
            postUrl = $"{serverUrl}flower";
            homeUrl = $"{serverUrl}load";

            flowers.Clear();
            flowersFromJson = null;
            StartCoroutine(GetFlowers());
        }

        public void PlantFlower(Vector3 position)
        {
            Flower toPlant = new Flower { positionX = position.x, positionY = position.y, positionZ = position.z };
            flowers.Add(toPlant);
            var flowerInstance = Instantiate(GetRandomFlowerAsset());
            flowerInstance.transform.position = position;
            flowerInstance.SetActive(true);

            StartCoroutine(PostFlower(toPlant));
        }

        private IEnumerator GetFlowers()
        {
            Debug.Log(getUrl);
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
                        Debug.Log(flowersFromJson);
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
            using (UnityWebRequest www = UnityWebRequest.Post(postUrl, JsonUtility.ToJson(flower)))
            {
                www.SetRequestHeader("Content-Type", "application/json");
                www.SetRequestHeader("Accept", "application/json");
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("Post Request Complete!");
                }
            }
        }

        private void InitializeFlowers()
        {
            for (int i = 0; i < flowersFromJson.flowers.Length; i++)
            {
                var flowerInstance = Instantiate(GetRandomFlowerAsset());
                Flower flower = flowersFromJson.flowers[i];
                flowerInstance.transform.position = new Vector3(flower.positionX, flower.positionY, flower.positionZ);
                flowerInstance.SetActive(true);
                flowers.Add(flower);
            }
        }
    }
}