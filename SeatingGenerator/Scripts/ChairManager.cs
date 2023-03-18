using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;

public class ChairManager : MonoBehaviour
{
    public static ChairManager chairManagerInstance;

    public CoreBlock coreBlock;

    //Object Settings (1 by blocks are not being made)
    public int maxSeatLength = 5;
    public int maxSeatWidth = 5;

    public int currentSeatLength;
    public int currentSeatWidth;

    public Vector3 startPos = new Vector3(0, 0, 0);

    //Store Generated Blocks
    public Dictionary<Vector2, CoreBlock> gridBlocks;

    //Store list of edges
    public List<Transform> edgePosition = new List<Transform>();

    //Store list of corner blocks
    public List<Transform> cornerPosition = new List<Transform>();

    public GameObject parent = null;

    public float rotateSpeed = 20.0f;

    // Start is called before the first frame update
    void Awake()
    {
        chairManagerInstance = this;
    }

    private void Update()
    {
        edgePosition.RemoveAll(item => item == null);
        cornerPosition.RemoveAll(item => item == null);

        parent.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }

    public void GenerateSeating()
    {
        //Need a minimum of 2 to have a seat
        currentSeatLength = UnityEngine.Random.Range(2, maxSeatLength);
        currentSeatWidth = UnityEngine.Random.Range(2, maxSeatWidth);

        GenerateGrid(currentSeatLength, currentSeatWidth);
    }

    public void ClearPreviousGenerated()
    {
        var children = new List<GameObject>();
        foreach (Transform child in parent.transform)
        {
            children.Add(child.gameObject);
            children.ForEach(child => Destroy(child));
        }

        edgePosition.Clear();
        cornerPosition.Clear();
    }

    public void GenerateGrid(int _length, int _width)
    {
        //Clear the parent for each
        ClearPreviousGenerated();

        gridBlocks = new Dictionary<Vector2, CoreBlock>();    //Instantiate new dictionary to collect all the generated blocks

        for (int x = 0; x < _length; x++)
        {
            for (int y = 0; y < _width; y++)
            {
                var spawnedBlock = Instantiate(coreBlock, new Vector3(x, 0, y), Quaternion.identity);
                spawnedBlock.name = $"{x} {y}";
                spawnedBlock.block_x_loc = x;
                spawnedBlock.block_y_loc = y;

                spawnedBlock.transform.parent = parent.transform;

                //Setting offset color for seat
                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedBlock.Init(isOffset);

                //Checking the edges
                var isEdge = (x == 0 && y == 0) || (x % _length != 0 && y % (_width - 1) == 0) || (x % (_length - 1) == 0 && y % _width != 0);
                spawnedBlock.CheckEdge(isEdge);

                gridBlocks[new Vector2(x, y)] = spawnedBlock;
            }
        }

        AppendManager.appendManagerInstance.GenerateLegs(cornerPosition);
        AppendManager.appendManagerInstance.GenerateSides(edgePosition);
    }

    public CoreBlock GetEdgeBlock(Dictionary<Vector2, CoreBlock> _activeBlockDictionary)
    {
        return _activeBlockDictionary.Where(t => t.Value.currBlockType.Equals("edge")).First().Value;
    }

    public CoreBlock GetCornerBlock(Dictionary<Vector2, CoreBlock> _activeBlockDictionary)
    {
        return _activeBlockDictionary.Where(t => t.Value.currBlockType.Equals("corner")).First().Value;
    }

    public void SetMaxLengthSlider(System.Single _maxLength)
    {
        maxSeatLength = (int)_maxLength;
    }

    public void SetMaxWidthSlider(System.Single _maxWidth)
    {
        maxSeatWidth = (int)_maxWidth;
    }

    public void SetRotateSpeed(System.Single _speed)
    {
        rotateSpeed = (float)_speed;
    }
}
