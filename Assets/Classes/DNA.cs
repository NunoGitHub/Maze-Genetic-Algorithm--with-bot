using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class is responsible for storing an individual's genes. It includes methods for randomly initializing genes, manually setting genes, combining the genes of two parents to create a descendant, performing mutations on genes, and retrieving the value of a gene at a specific position.
//Essentially, the DNA class serves as the genetic code of an individual, determining their potential characteristics and behaviors.
public class DNA
{
    private List<int> genes = new List<int>();

    private int dnaLenght { get; set; }
    private int maxValues { get; set; }

    public DNA(int l, int v)
    {
        //size , how many things the charcter can do
        dnaLenght = l;
        maxValues = v;
        SetRandom();
    }
    public void SetRandom()
    {
        genes.Clear();

        for (int i = 0; i < dnaLenght; i++)
        {
            genes.Add(Random.Range(0, maxValues));
        }
    }

    //whith you want to hard code some genes to make the ai do something in specific
    public void SetInt(int pos, int value)
    {
        genes[pos] = value;
    }

    public void Combine(DNA d1, DNA d2)
    {
        for (int i = 0; i < dnaLenght; i++)
        {
            //first half from parent one second half from parent 2
            //combine the 2 parents radomly into the offspring
            if (i < dnaLenght / 2.0f)
            {
                int c = d1.genes[i];
                genes[i] = c;
            }
            else
            {
                int c = d2.genes[i];
                genes[i] = c;
            }
        }
    }

    //set a a random value to a sequence
    public void Mutate()
    {
        genes[Random.Range(0, dnaLenght)] = Random.Range(0, maxValues);
    }

    //get gene in a position
    public int GetGene(int pos)
    {
        return genes[pos];
    }
}
