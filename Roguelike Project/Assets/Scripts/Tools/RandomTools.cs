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
    public class SizeWeightedObject : WeightedObject
    {
        public int minRoomFloorsWidth;
        public int minRoomFloorsHeight;
        public int maxRoomFloorsWidth;
        public int maxRoomFloorsHeight;
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

    public List<SizeWeightedObject> CreateSizeWeightedObjectsList(List<SizeWeightedObject> weightedSizedObjects)
    {
        List<SizeWeightedObject> weightedList = new List<SizeWeightedObject>();
        foreach (SizeWeightedObject item in weightedSizedObjects)
        {
            for (int i = 0; i < item.probability; i++)
            {
                weightedList.Add(item);
            }
        }
        return weightedList;
    }

    public GameObject PickOne(WeightedObject[] weightedObjects)
    {
        return weightedObjects[Random.Range(0, weightedObjects.Length)].item;
    }

    public SizeWeightedObject PickOneSized(List<SizeWeightedObject> weightedSizedObjects)
    {
        return weightedSizedObjects[Random.Range(0, weightedSizedObjects.Count)];
    }
}

