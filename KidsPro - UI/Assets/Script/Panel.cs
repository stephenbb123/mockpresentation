using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Panel : MonoBehaviour
{
    private List<BlockPanelItem> mItems = new List<BlockPanelItem>();
    public event EventHandler<PanelEventArgs> ItemRemoved;


    public void RemoveItem(BlockPanelItem item)
    {
        if (mItems.Contains(item))
        {
            item.OnDrop();
            Collider collider = (item as MonoBehaviour).GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = true;
            }
        }
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
