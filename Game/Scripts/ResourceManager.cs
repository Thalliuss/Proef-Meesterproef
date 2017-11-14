using UnityEngine;
using DataManagement;

public class ResourceManager : MonoBehaviour
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
    private int _wood;

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
    private int _rock;

    private SceneManager _sceneManager;
    private DataReferences _dataReferences;

    private ResourceInfo _resourceInfo = null;

    private static ResourceManager _instance;
    public static ResourceManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private const string _resourceInfoID = "RESOURCE_DATA";
    private void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);

        _instance = this;
    }

    private void Start()
    {
        _sceneManager = SceneManager.Instance;
        _dataReferences = _sceneManager.DataReferences;

        if (_dataReferences != null) _resourceInfo = _dataReferences.FindElement<ResourceInfo>(_resourceInfoID);
        if (_resourceInfo == null && _dataReferences != null)
        {
            _dataReferences.AddElement<ResourceInfo>(_resourceInfoID);
            _resourceInfo = _dataReferences.FindElement<ResourceInfo>(_resourceInfoID);
        }
        else
        {
            _wood = _resourceInfo.Wood;
            _rock = _resourceInfo.Rock;
        }
    }

    public void AddWood(int p_input)
    {
        _wood += p_input;
        _resourceInfo.Wood = _wood;
        _resourceInfo.Save();
    }

    public void AddRock(int p_input)
    {
        _rock += p_input;
        _resourceInfo.Rock = _rock;
        _resourceInfo.Save();
    }
}
