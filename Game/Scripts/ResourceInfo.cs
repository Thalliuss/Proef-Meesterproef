using DataManagement;
using UnityEngine;

public class ResourceInfo : DataElement
{
    public int Wood
    {
        get
        {
            return _wood;
        }

        set
        {
            _wood = value;
        }
    }
    [SerializeField] private int _wood;

    public int Rock
    {
        get
        {
            return _rock;
        }

        set
        {
            _rock = value;
        }
    }
    [SerializeField] private int _rock;

    public ResourceInfo(string p_id) : base(p_id)
    {
        ID = p_id;
    }
}
