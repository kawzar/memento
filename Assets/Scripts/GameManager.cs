using System.Collections;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

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

        public Player Player => player;

        private string getUrl, postUrl;

        private IList<Flower> flowers = new List<Flower>();
        private FlowerResult flowersFromJson;

        private async void Awake()
        {
            Instance = this;

            serverUrl = serverUrl.Trim();
            getUrl = $"{serverUrl}flowers";
            postUrl = $"{serverUrl}flower";

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
                        player.gameObject.SetActive(true);
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

        public void LoadGame()
        {
            SceneManager.LoadScene(1);
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}