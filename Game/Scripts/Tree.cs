using UnityEngine;

[System.Serializable]
public class Tree
{
    public Vector3 Position
    {
        get
        {
            return _position;
        }

        set
        {
            _position = value;
        }
    }
    [SerializeField]
    private Vector3 _position;

    public Quaternion Rotation
    {
        get
        {
            return _rotation;
        }

        set
        {
            _rotation = value;
        }
    }
    [SerializeField]
    private Quaternion _rotation;

    public GameObject Prefab
    {
        get
        {
            return _prefab;
        }

        set
        {
            _prefab = value;
        }
    }
    [SerializeField]
    private GameObject _prefab;

    public Tree(Vector3 p_position, Quaternion p_rotation, GameObject p_prefab)
    {
        _position = p_position;
        _rotation = p_rotation;
        _prefab = p_prefab;
    }
}
