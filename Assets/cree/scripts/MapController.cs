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

    [SerializeField] //détermine la longeur de la map. doit absolument contenir le start et end donc minimum 2
    private int longueurMap = 6;

    private GameObject[] map;
    //rivate float nombreSols = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        map = new GameObject[longueurMap];


        //for qui met aléatoirement des sols dans un tableau pour créer une map
        for (int i = 0; i < longueurMap;i++)
        {
            if (i == 0)
            {
                map[i] = start;
            }
            else if(i == 5) 
            {
                map[i] = end; 
            }
            else
            {
                map[i] = solsPrefabs[Random.Range(0, solsPrefabs.Length)];
            }
        }

        //for qui instantie chaque sols du tableau de la map à la bonne position
        for(int i = 0;i < map.Length;i++) 
        {
            GameObject sol = Instantiate(map[i]);
            if(i==0)
            {
                sol.transform.position = new Vector3(0, 0, 0);
                map[i] = sol;
            }
            else if (i > 0)
            {
                sol.transform.position = map[i - 1].transform.position + new Vector3(0, 0, map[i - 1].transform.Find("longueur").localScale.z*-1);
                map[i] = sol;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
