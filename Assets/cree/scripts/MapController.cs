using System.Runtime.CompilerServices;
using UnityEngine;



public class MapController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] solsPrefabs;
    [SerializeField]
    private GameObject start;
    [SerializeField]
    private GameObject end;


    private GameObject[] map;
    private int mapLastLongueur;

    private Transform parent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parent = gameObject.transform;
        NouveauParcour(BalleController.longueurMap);
    }

    public void NouveauParcour(int longueurMap)
    {
        map = new GameObject[longueurMap];

        //vide la map
        for(int i=0;i<map.Length; i++)
        {
            Destroy(map[i]);
        }

        //for qui met aléatoirement des sols dans un tableau pour créer une map
        for (int i = 0; i < longueurMap; i++)
        {
            if (i == 0)
            {
                map[i] = start;
            }
            else if (i == longueurMap-1)
            {
                map[i] = end;
            }
            else
            {
                map[i] = solsPrefabs[Random.Range(0, solsPrefabs.Length)];
            }
        }

        //for qui instantie chaque sols du tableau de la map à la bonne position
        for (int i = 0; i < map.Length; i++)
        {
            GameObject sol = Instantiate(map[i]);
            if (i == 0)
            {
                sol.transform.SetParent(parent);
                sol.transform.position = new Vector3(0, 0, 0);
                map[i] = sol;
            }
            else if (i > 0)
            {
                sol.transform.SetParent(parent);
                sol.transform.position = map[i - 1].transform.position + new Vector3(0, 0, map[i - 1].transform.Find("longueur").localScale.z * -1);
                map[i] = sol;
            }
        }
    }
}
