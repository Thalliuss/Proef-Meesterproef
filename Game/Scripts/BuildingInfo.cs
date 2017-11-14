using DataManagement;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInfo : DataElement
{
    public List<Building> Buildings
    {
        get
        {
            return _buildings;
        }

        set
        {
            _buildings = value;
        }
    }
    [SerializeField] private List<Building> _buildings = new List<Building>();

    public BuildingInfo(string p_id) : base(p_id)
    {
        ID = p_id;
    }
}
