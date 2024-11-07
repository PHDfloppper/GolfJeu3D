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
    /// fonction qui g�n�re un nouveau parcour quand le joueur passe au prochain niveau
    /// prend la longeur de map pour savoir combien de prefab le parcour doit utiliser
    /// les mets al�atoirement dans un tableau pour cr�er un parcour, en ayant le spawn au d�but et le trou � la fin du tableau
    /// Ensuite, instantie chaque pi�ce du parcour � la bonne position. chaque pi�ce de parcour a un rectangle qui fait
    /// tout la longueur de la pi�ce pour facilit� l'obtention de la longueur total d'une pi�ce, pour qu'on puisse mettre
    /// les pi�ces bout � bout
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

        //for qui met al�atoirement des sols dans un tableau pour cr�er une map
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

        //for qui instantie chaque sols du tableau de la map � la bonne position
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
        //si l'ancienne longueur de map enregistr� est inf�rieur � la nouvelle, �a vaut dire que le joueur est au prochain niveau
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
