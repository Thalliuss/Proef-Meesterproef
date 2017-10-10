using DataManagement;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;

public class GenerationManager : MonoBehaviour
{
    private static GenerationManager _instance;
    public static GenerationManager Instance
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

    private enum Orientation
    {
        Up,
        Down
    }

    [SerializeField]
    private List<Transform> _generationLocations;

    private DataManager _dataManager;
    private DataReferences _dataReferences;

    public GameObject Tree
    {
        get
        {
            return _tree;
        }

        set
        {
            _tree = value;
        }
    }

    public int Amount
    {
        get
        {
            return _amount;
        }

        set
        {
            _amount = value;
        }
    }
    [SerializeField]
    private int _amount;

    [SerializeField]
    private GameObject _tree;
    [SerializeField]
    private float _radius;
    [SerializeField]
    private Player _player;

    private List<GameObject> _trees = new List<GameObject>();
    private List<GameObject> _buildings = new List<GameObject>();

    private void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);

        _instance = this;

        _dataManager = DataManager.Instance;
        _dataReferences = _dataManager.DataReferences;

        int progress = 100 * 0  / Amount;
    }

    private void Start()
    {
        StartCoroutine(PlaceExistingObjects());

        if (_dataReferences.SaveData.ids.Count > 0) return;

        StartCoroutine(PlaceTrees(Orientation.Up, _generationLocations[0]));
        StartCoroutine(PlaceTrees(Orientation.Down, _generationLocations[1]));
    }

    private IEnumerator Wait(float p_input)
    {
        yield return new WaitForSeconds(p_input);
    }

    IEnumerator PlaceTrees(Orientation p_orientation, Transform p_position)
    {
        LoadingscreenManager t_loadingscreenManager = LoadingscreenManager.Instance;
        UIManager t_uiManager = UIManager.Instance;
        t_loadingscreenManager.OpenLoadingscreen("Generating World.");

        for (uint i = 0; i < Amount; i++)
        {
            if (i % (Amount / 50) == 0 && i != 0)
                yield return new WaitForSecondsRealtime(0.01f);

            t_uiManager.LoadingBar.value = t_uiManager.LoadingBar.maxValue * i / Amount;

            GameObject t_tree = _tree;
            Vector2 t_spawnPosV2 = UnityEngine.Random.insideUnitCircle * _radius;
            Vector3 t_spawnPos = new Vector3(t_spawnPosV2.x, 0, t_spawnPosV2.y);
            Vector3 t_offset = p_position.position + t_spawnPos;

            Vector3 t_axis = new Vector3();
            if (p_orientation == Orientation.Up) t_axis = Vector3.up;
            if (p_orientation == Orientation.Down) t_axis = Vector3.down;

            RaycastHit t_hit;
            if (Physics.Raycast(t_offset, t_axis, out t_hit))
            {
                Vector3 t_finalSpawnPos = t_hit.point;
                if (!t_hit.collider.CompareTag("Tower"))
                    Instantiate(t_tree, t_finalSpawnPos, Quaternion.identity);
            }
        }
        t_loadingscreenManager.CloseLoadingscreen();
        yield return null;
    }

    IEnumerator PlaceExistingObjects()
    {
        LoadingscreenManager t_loadingscreenManager = LoadingscreenManager.Instance;
        UIManager t_uiManager = UIManager.Instance;

        TreeInfo t_treeInfoArray = _dataReferences.FindElement<TreeInfo>("TREE_DATA");
        BuildingInfo t_buildingInfoArray = _dataReferences.FindElement<BuildingInfo>("BUILDING_DATA");

        if (t_treeInfoArray != null)
        {
            if(t_treeInfoArray.Trees.Count > 0) t_loadingscreenManager.OpenLoadingscreen("Placing Tree's.");

            for (int i = 0; i < t_treeInfoArray.Trees.Count; i++)
            {
                t_uiManager.LoadingBar.value = t_uiManager.LoadingBar.maxValue * i / t_treeInfoArray.Trees.Count;

                GameObject t_obj = Instantiate(_tree, t_treeInfoArray.Trees[i].Position, t_treeInfoArray.Trees[i].Rotation);
                _trees.Add(t_obj);

                if (i % (t_treeInfoArray.Trees.Count / 50) == 0 && i != 0)
                    yield return new WaitForSecondsRealtime(0.01f);
            }
            t_uiManager.LoadingBar.value = 0;
        }

        if (t_buildingInfoArray != null)
        {
            if (t_buildingInfoArray.Buildings.Count > 0) t_loadingscreenManager.OpenLoadingscreen("Placing Buildings.");

            for (int i = 0; i < t_buildingInfoArray.Buildings.Count; i++)
            {
                t_uiManager.LoadingBar.value = t_uiManager.LoadingBar.maxValue * i / t_buildingInfoArray.Buildings.Count;

                GameObject t_obj = Instantiate(_player.prefab, t_buildingInfoArray.Buildings[i].Position, t_buildingInfoArray.Buildings[i].Rotation);
                _buildings.Add(t_obj);
                yield return new WaitForSecondsRealtime(0.01f);
            }
            t_uiManager.LoadingBar.value = 0;
        }
        t_loadingscreenManager.CloseLoadingscreen();
        yield return null;
    }

    public void ClearExistingObjects()
    {

    }
}

