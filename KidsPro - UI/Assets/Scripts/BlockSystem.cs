using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BlockSystem : MonoBehaviour
{
    [SerializeField]
    private BlockType[] allBlockTypes;

    [HideInInspector]
    public Dictionary<int, Block> allBlocks = new Dictionary<int, Block>();

    private void Awake()
    {
        for(int i = 0; i < allBlockTypes.Length; i++)
        {
            BlockType newBlockType = allBlockTypes[i];
            Block newBlock = new Block(i,newBlockType.blockName,newBlockType.blockMat);
            allBlocks[i] = newBlock;
            Debug.Log("Block added to Dictionary : " + allBlocks[i].blockName);
        }
    }
}

public class Block
{
    public int blockID;
    public string blockName;
    public Material blockMaterial;

public Block(int blockID, string blockName, Material blockMaterial)
    {
        this.blockID = blockID;
        this.blockName = blockName;
        this.blockMaterial = blockMaterial;
    }
}

[Serializable]
public struct BlockType
{
    public string blockName;
    public Material blockMat;
}