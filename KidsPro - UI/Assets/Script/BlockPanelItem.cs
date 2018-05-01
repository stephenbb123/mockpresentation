using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public interface BlockPanelItem
{

    Sprite Image { get; }
    void OnPickup();
    void OnDrop();
}

public class PanelEventArgs : EventArgs
{
    public PanelEventArgs(BlockPanelItem item)
    {
        Item = item;
    }
    public BlockPanelItem Item;
}
