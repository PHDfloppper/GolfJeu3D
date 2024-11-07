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

    /// <summary>
    /// fonction qui génère un nouveau parcour quand le joueur passe au prochain niveau
    /// prend la longeur de map pour savoir combien de prefab le parcour doit utiliser
    /// les mets aléatoirement dans un tableau pour créer un parcour, en ayant le spawn au début et le trou à la fin du tableau
    /// Ensuite, instantie chaque pièce du parcour à la bonne position. chaque pièce de parcour a un rectangle qui fait
    /// tout la longueur de la pièce pour facilité l'obtention de la longueur total d'une pièce, pour qu'on puisse mettre
    /// les pièces bout à bout
    /// </summary>
    /// <param name="longueurMap"></param>
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

    private void Update()
    {
        //si l'ancienne longueur de map enregistré est inférieur à la nouvelle, ça vaut dire que le joueur est au prochain niveau
        //donc on supprime l'ancien niveau et en instantie un autre avec la nouvelle longueur
        if (mapLastLongueur < BalleController.longueurMap)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            mapLastLongueur = BalleController.longueurMap;
            NouveauParcour(mapLastLongueur);
        }
    }
}
