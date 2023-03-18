using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AppendManager : MonoBehaviour
{
    public static AppendManager appendManagerInstance;

    public int maxLegLength = 3;
    public int currentLegLength;
    public float legBlockSpacing = 1.0f;

    public int maxSideLength = 3;
    public int currentSideLength;
    public float sideBlockSpacing = 1.0f;

    public Transform previousLegBlockTransform;
    public Transform previousSideBlockTransform;

    public List<GameObject> legBlocks = new List<GameObject>();
    public List<GameObject> sideBlocks = new List<GameObject>();

    private void Awake()
    {
        appendManagerInstance = this;
    }

    public void GenerateLegs(List<Transform> cornerPosList)
    {
        //Remove all blank entries
        //cornerPosList.RemoveAll(item => item == null);

        currentLegLength = UnityEngine.Random.Range(1, maxLegLength);

        for(int j=0; j< cornerPosList.Count; j++)
        {
            previousLegBlockTransform = cornerPosList[j];

            for (int i = 0; i < currentLegLength; i++)
            {
                var currentLegBlock = Instantiate(legBlocks[Random.Range(0, legBlocks.Count)], new Vector3(previousLegBlockTransform.position.x, previousLegBlockTransform.position.y - legBlockSpacing, previousLegBlockTransform.position.z), Quaternion.identity); // Create a specific cell on position (x,y)
                previousLegBlockTransform = currentLegBlock.transform;

                currentLegBlock.transform.parent = ChairManager.chairManagerInstance.parent.transform;
            }
        }
    }

    public void GenerateSides(List<Transform> edgePosList)
    {
        //Remove all blank entries
        //edgePosList.RemoveAll(item => item == null);

        currentSideLength = UnityEngine.Random.Range(0, maxSideLength);

        for (int j = 0; j < edgePosList.Count/2; j++)
        {
            previousSideBlockTransform = edgePosList[j];

            for (int i = 0; i < currentSideLength; i++)
            {
                var currentSideBlock = Instantiate(sideBlocks[Random.Range(0, sideBlocks.Count)], new Vector3(previousSideBlockTransform.position.x, previousSideBlockTransform.position.y + sideBlockSpacing, previousSideBlockTransform.position.z), Quaternion.identity); // Create a specific cell on position (x,y)
                previousSideBlockTransform = currentSideBlock.transform;

                currentSideBlock.transform.parent = ChairManager.chairManagerInstance.parent.transform;
            }
        }
    }

    public void SetMaxLegLengthSlider(System.Single _maxLegLength)
    {
        maxLegLength = (int)_maxLegLength;
    }

    public void SetMaxSideLengthSlider(System.Single _maxSideLength)
    {
        maxSideLength = (int)_maxSideLength;
    }
}
