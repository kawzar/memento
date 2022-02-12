namespace Kawzar.Memento.Scripts
{
    [System.Serializable]
    public class Flower
    {
        public int id;
        public float positionX;
        public float positionY;
        public float positionZ;
    }

    [System.Serializable]

    public class FlowerResult
    {
        public Flower[] flowers;
    }
}