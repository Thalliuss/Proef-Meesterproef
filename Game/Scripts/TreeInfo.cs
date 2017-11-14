using DataManagement;
using System.Collections.Generic;
using UnityEngine;

public class TreeInfo : DataElement
{
    public List<Tree> Trees
    {
        get
        {
            return _trees;
        }

        set
        {
            _trees = value;
        }
    }
    [SerializeField] private List<Tree> _trees = new List<Tree>();

    public TreeInfo(string p_id) : base(p_id)
    {
        ID = p_id;
    }
}
