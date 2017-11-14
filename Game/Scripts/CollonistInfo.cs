using DataManagement;
using System.Collections.Generic;
using UnityEngine;

public class CollonistInfo : DataElement
{
    public List<Collonist> Collonists
    {
        get
        {
            return _collonists;
        }

        set
        {
            _collonists = value;
        }
    }
    [SerializeField]
    private List<Collonist> _collonists = new List<Collonist>();

    public CollonistInfo(string p_id) : base(p_id)
    {
        ID = p_id;
    }
}
