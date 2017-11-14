using DataManagement;
using System.Collections.Generic;
using UnityEngine;

public class RockInfo : DataElement
{
    public List<Rock> Rocks
    {
        get
        {
            return _rocks;
        }

        set
        {
            _rocks = value;
        }
    }
    [SerializeField] private List<Rock> _rocks = new List<Rock>();

    public RockInfo(string p_id) : base(p_id)
    {
        ID = p_id;
    }
}
