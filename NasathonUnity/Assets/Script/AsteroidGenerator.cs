using UnityEngine;

public class AsteroidGenerator : MonoBehaviour
{

    [SerializeField] private Asteroid asteroid;
    // public stAsteratic AsteroidGenerator instance { get; private set; }

    // private void Awake()
    // {
    //     instance = this;
    // }


    void Start()
    {

        AsteroidCreationUI.instance.OnCreateAsteroid += OnCreateAsteroid;
    }

    private void OnCreateAsteroid(object sender, AsteroidCreationUI.AsteroidCreationData data)
    {
        Debug.Log("Hello!!!");
        Asteroid asteroid = Instantiate(this.asteroid, transform);
        asteroid.InitializeMesh(data);
    }

}
