using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomTools
{
    private RandomTools() {}
    public static RandomTools Instance { get; } = new RandomTools();

    [System.Serializable]
    public class WeightedObject
    {
        public GameObject item;
        public int probability;
    }

    [System.Serializable]
    public class WeightedSizedObject : WeightedObject
    {
        public int tilesAvailableAbove;
        public int tilesAvailableBelow;
        public int tilesAvailableBeside;
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

    public WeightedSizedObject[] CreateWeightedSizedObjectsArray(WeightedSizedObject[] weightedSizedObjects)
    {
        WeightedSizedObject[] weightedArray = new WeightedSizedObject[weightedSizedObjects.Select(x => x.probability).Sum()];
        int itemCont = 0;
        foreach (WeightedSizedObject item in weightedSizedObjects)
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

    public WeightedSizedObject PickOneSized(WeightedSizedObject[] weightedSizedObjects)
    {
        return weightedSizedObjects[Random.Range(0, weightedSizedObjects.Length)];
    }
}

