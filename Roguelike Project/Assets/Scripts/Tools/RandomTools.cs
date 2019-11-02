using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomTools
{
    [System.Serializable]
    public class WeightedObject
    {
        public GameObject item;
        public int probability;
    }

    public WeightedObject[] CreateWeightedObjectsArray(WeightedObject[] weightedObjects)
    {
        WeightedObject[] weightedArray = new WeightedObject[weightedObjects.Select(x => x.probability).Sum()];
        int itemCont = 0;
        foreach (WeightedObject item in weightedObjects)
        {
            for (int i = 0; i < item.probability; i++)
            {
                weightedArray[itemCont] = item;
                itemCont++;
            }
        }
        return weightedArray;
    }

    public GameObject PickOne(WeightedObject[] weightedObjects)
    {
        return weightedObjects[Random.Range(0, weightedObjects.Length)].item;
    }
}

