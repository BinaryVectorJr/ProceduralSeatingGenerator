using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreBlock : MonoBehaviour
{
    public int block_x_loc = 0;
    public int block_y_loc = 0;

    [SerializeField] private Color baseColor, offsetColor, edgeColor, cornerColor;
    [SerializeField] private MeshRenderer meshRenderer;

    public bool isCorner = false;

    public enum BlockType
    {
        normal = 0,
        edge = 1,
        corner = 2,
        hasLegs = 3,
        hasBack = 4,
    }

    public BlockType currBlockType = BlockType.normal;

    public void Init(bool isOffset)
    {
        meshRenderer.material.color = isOffset ? offsetColor : baseColor;
    }
    public void CheckEdge(bool isEdge)
    {
        currBlockType = isEdge ? BlockType.edge : BlockType.normal;

        if(currBlockType == BlockType.edge)
        {
            meshRenderer.material.color = edgeColor;
            ChairManager.chairManagerInstance.edgePosition.Add(this.gameObject.transform);

            //Checking for corners
            var checkCorner = (block_x_loc == 0 && block_y_loc == 0) || (block_x_loc % (ChairManager.chairManagerInstance.currentSeatLength - 1) == 0 && block_y_loc % (ChairManager.chairManagerInstance.currentSeatWidth - 1) == 0);
            if(checkCorner)
            {
                isCorner = true;
                currBlockType = BlockType.corner;
                meshRenderer.material.color = cornerColor;
                ChairManager.chairManagerInstance.cornerPosition.Add(this.gameObject.transform);
                ChairManager.chairManagerInstance.edgePosition.Remove(this.gameObject.transform);
            }
        }
    }

}
