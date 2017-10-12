using UnityEngine;
using DataManagement;
using System.IO;

public class SceneManger : MonoBehaviour
{
    private static SceneManger _instance;
    public static SceneManger Instance
    {
        get
        {
            return _instance;
        }

        set
        {
            _instance = value;
        }
    }

    [SerializeField] private string _sceneID;

    public DataReferences DataReferences
    {
        get
        {
            return _dataReferences;
        }

        set
        {
            _dataReferences = value;
        }
    }
    [SerializeField] private DataReferences _dataReferences;

    private void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);

        _instance = this;

        if (DataManager.Instance == null) return;

        _dataReferences.ID = _sceneID;

        string t_path = Application.persistentDataPath + "/" + DataManager.Instance.SaveID + "/" + _dataReferences.ID + "/";
        if (!Directory.Exists(t_path))
            Directory.CreateDirectory(t_path);

        Build();
    }

    private void OnDestroy()
    {
        ClearAllData();
    }

    public void ClearAllData()
    {
        _dataReferences.SaveData.ids.Clear();
        _dataReferences.SaveData.info.Clear();
        _dataReferences.SaveData.types.Clear();
    }

    private void Build()
    {
        DataBuilder.BuildDataReferences();

        DataBuilder.BuildElementsOfType<TreeInfo>(_dataReferences.SaveData);
        DataBuilder.BuildElementsOfType<BuildingInfo>(_dataReferences.SaveData);
    }
}
