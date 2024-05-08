using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopulationManager : MonoBehaviour
{

    public GameObject botPrefab;
    public int populationSize = 50;
    List<GameObject> population = new List<GameObject>();
    public static float elapse = 0;
    public float trialTime = 5;
    int generation = 1;

    GUIStyle gUIStyle = new GUIStyle();

    private void OnGUI()
    {
        gUIStyle.fontSize = 15;
        gUIStyle.normal.textColor = Color.white;
        GUI.BeginGroup(new Rect(10, 10, 250, 150));
        GUI.Box(new Rect(0, 0, 140, 140), "Stats", gUIStyle);
        GUI.Label(new Rect(10, 25, 200, 30), "Gen: " + generation, gUIStyle);
        GUI.Label(new Rect(10, 50, 200, 30), string.Format("Time: {0:0.00}", elapse), gUIStyle);
        GUI.Label(new Rect(10, 75, 200, 30), "population " + population.Count, gUIStyle);
        GUI.EndGroup();
    }

    private void Start()
    {
        //instantiate a new bot and add to list
        for (int i = 0; i < populationSize; i++)
        {
            Vector3 startingpos = new Vector3(transform.position.x + Random.Range(-1, 1), transform.position.y, transform.position.z + Random.Range(-1, 1));

            GameObject b = Instantiate(botPrefab, startingpos, transform.rotation);
            b.GetComponent<Brain>().Init();
            population.Add(b);
        }
    }

    //get two parents to create an offspring
    GameObject Breed(GameObject parent1, GameObject parent2)
    {

        Vector3 startingPos = new Vector3(transform.position.x + Random.Range(-1, 1), transform.position.y, transform.position.z + Random.Range(-1, 1));

        GameObject offSpring = Instantiate(botPrefab, startingPos, transform.rotation);

        //get the brain
        Brain b = offSpring.GetComponent<Brain>();

        if (Random.Range(0, 100) == 1)// if it 1 create a  completely new brain or else use the parents dna
        {   //initialize the brain
            b.Init();
            //create a new mutation
            //get random genes at random position
            b.dna.Mutate();
        }
        else
        {
            b.Init();
            //combine the genes of parents
            b.dna.Combine(parent1.GetComponent<Brain>().dna, parent2.GetComponent<Brain>().dna);
        }
        //get the child
        return offSpring;
    }

    void BreedNewPopulation()
    {
        // sort in descending order by timeAlive variable
        List<GameObject> sortedList = population.OrderBy(x => x.GetComponent<Brain>().catchEnemy+x.GetComponent<Brain>().collWall+x.GetComponent<Brain>().pointsCatch*10+x.GetComponent<Brain>().timesJump*-2 + x.GetComponent<Brain>().totalDist*5).ToList();

        population.Clear();

        //breed upper half of the sorted list
        for (int i = (int)(sortedList.Count / 2.0f) - 1; i < sortedList.Count - 1; i++)
        {
            //breed and add the offspring to the population
            population.Add(Breed(sortedList[i], sortedList[i + 1]));
            population.Add(Breed(sortedList[i + 1], sortedList[i]));
        }

        //Destroy previous generation, the parents
        for (int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }
        generation++;
    }

    private void Update()
    {
        //update elspse time 
        elapse += Time.deltaTime;

        //max time surpassed , create new/breed population
        if (elapse >= trialTime)
        {
            BreedNewPopulation();
            elapse = 0;
        }

    }

}