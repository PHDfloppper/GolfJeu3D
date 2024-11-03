using System;
using UnityEngine;

public class MapInitializer : MonoBehaviour
{
    private int mapLastLongueur;
    [SerializeField]
    private GameObject map;

    [SerializeField]
    private GameObject balle;

    private void Start()
    {
        mapLastLongueur = BalleController.longueurMap;
    }

    // Update is called once per frame
    void Update()
    {
        if (mapLastLongueur < BalleController.longueurMap)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            mapLastLongueur = BalleController.longueurMap;
            GameObject _map = Instantiate(map);
            _map.transform.SetParent(gameObject.transform);
            _map.SetActive(true);
        }
    }
}
