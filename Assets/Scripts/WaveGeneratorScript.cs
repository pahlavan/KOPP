using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGeneratorScript : MonoBehaviour
{
    // Start is called before the first frame update
    public static IList<WaveProperty> WaveProperties =
        new List<WaveProperty> {
            //new WaveProperty { ShipName = "TemplateShip", GracePriod = 0, IntensificationInterval = 10, UnitSpawnRatePerSecond = 0.4f, MaxSpawnRate = 2 },
            new WaveProperty { ShipName = "Caravel", GracePriod = 0, IntensificationInterval = 20, UnitSpawnRatePerSecond = 0.05f, MaxSpawnRate = 2 },
            new WaveProperty { ShipName = "Caravel", GracePriod = 0, IntensificationInterval = 100000, UnitSpawnRatePerSecond = 0.2f, MaxSpawnRate = 2 },
            new WaveProperty { ShipName = "Galleon", GracePriod = 15, IntensificationInterval = 20, UnitSpawnRatePerSecond = 0.05f, MaxSpawnRate = 1 },
            new WaveProperty { ShipName = "Frigate", GracePriod = 30, IntensificationInterval = 20, UnitSpawnRatePerSecond = 0.05f, MaxSpawnRate = 1 },
        };
    public List<GameObject> SpaceshipPrefabs;
    public GameObject InitialPlanet;

    private IDictionary<string, GameObject> spaceShips;

    void createNewShip(GameObject ship)
    {
        var newShip = Instantiate(ship, Vector3.zero, Quaternion.identity);
        var shipScript = newShip.GetComponent<SpaceshipScript>();
        shipScript.InitialPlanet = InitialPlanet;
    }

    void Start()
    {
        foreach(var wave in WaveProperties)
        {
            wave.CurrentSpawnRatePerSecond = 0;
            wave.NextSpawnTime = wave.GracePriod;
            wave.NextIntensification = wave.GracePriod;
        }

        spaceShips = new Dictionary<string, GameObject>(StringComparer.OrdinalIgnoreCase);
        foreach (var ship in SpaceshipPrefabs)
        {
            spaceShips[ship.name] = ship;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float now = Time.time;

        foreach (var wave in WaveProperties)
        {
            if(wave.NextIntensification < now)
            {
                wave.CurrentSpawnRatePerSecond = Math.Min(wave.UnitSpawnRatePerSecond + wave.CurrentSpawnRatePerSecond, wave.MaxSpawnRate);
                wave.NextIntensification += wave.IntensificationInterval;
            }

            if (wave.NextSpawnTime < now)
            {
                GameObject ship;
                if (spaceShips.TryGetValue(wave.ShipName, out ship))
                {
                    createNewShip(ship);
                }

                wave.NextSpawnTime += 1 / wave.CurrentSpawnRatePerSecond;
            }
        }
    }
}

public class WaveProperty
{
    public string ShipName;
    public float GracePriod;
    public float IntensificationInterval;
    public float UnitSpawnRatePerSecond;
    public float MaxSpawnRate;

    public float CurrentSpawnRatePerSecond;
    public float NextSpawnTime;
    public float NextIntensification;
}
