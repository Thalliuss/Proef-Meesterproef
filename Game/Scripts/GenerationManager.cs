using DataManagement;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;
using System.IO;

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

    private SceneManager _sceneManager;
    private DataReferences _dataReferences;

    public List<GameObject> Buildings
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

    public List<GameObject> Trees
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

    [Serializable]
    public class Generator
    {
        public int amount;
        public GameObject[] obj;
        public float radius;
    }
    [SerializeField] private Generator _treeGen;
    [SerializeField] private Generator _rockGen;

    [SerializeField] private Player _player;

    [SerializeField]
    private List<GameObject> _trees = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _buildings = new List<GameObject>();
    [SerializeField]
    private List<GameObject> _rocks = new List<GameObject>();

    private void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);

        _instance = this;
    }

    private void Start()
    {
        _sceneManager = SceneManager.Instance;
        if (_sceneManager != null) _dataReferences = _sceneManager.DataReferences;

        Init();
    }

    public void Init()
    {
        ClearExistingObjects();
        if (_dataReferences != null) StartCoroutine(PlaceExistingObjects());

        if (_dataReferences != null && _dataReferences.SaveData.ids.Count > 0) return;

        StartCoroutine(PlaceTrees(Orientation.Up, _generationLocations[0]));
        StartCoroutine(PlaceTrees(Orientation.Down, _generationLocations[1]));

        StartCoroutine(PlaceRocks(Orientation.Up, _generationLocations[0]));
        StartCoroutine(PlaceRocks(Orientation.Down, _generationLocations[1]));
    }

    private const string _rockDataArrayID = "ROCK_DATA";
    private IEnumerator PlaceRocks(Orientation p_orientation, Transform p_position)
    {
        LoadingscreenManager t_loadingscreenManager = LoadingscreenManager.Instance;
        UIManager t_uiManager = UIManager.Instance;
        RockInfo t_rockInfoArray = null;
        if (_dataReferences != null) t_rockInfoArray = _dataReferences.FindElement<RockInfo>(_rockDataArrayID);
        if (t_rockInfoArray == null && _dataReferences != null)
        {
            _dataReferences.AddElement<RockInfo>(_rockDataArrayID);
            t_rockInfoArray = _dataReferences.FindElement<RockInfo>(_rockDataArrayID);
        }

        if (t_loadingscreenManager != null) t_loadingscreenManager.OpenLoadingscreen("Generating World.");

        for (int i = 0; i < _rockGen.amount; i++)
        {
            if (i % (_rockGen.amount / 25) == 0 && i != 0)
                yield return new WaitForSecondsRealtime(0.01f);

            if (t_loadingscreenManager != null) t_uiManager.LoadingBar.value = t_uiManager.LoadingBar.maxValue * i / _rockGen.amount;

            GameObject t_tree = _rockGen.obj[UnityEngine.Random.Range(0, _rockGen.obj.Length)];
            Vector2 t_spawnPosV2 = UnityEngine.Random.insideUnitCircle * _rockGen.radius;
            Vector3 t_spawnPos = new Vector3(t_spawnPosV2.x, 0, t_spawnPosV2.y);
            Vector3 t_offset = p_position.position + t_spawnPos;

            Vector3 t_axis = new Vector3();
            if (p_orientation == Orientation.Up) t_axis = Vector3.up;
            if (p_orientation == Orientation.Down) t_axis = Vector3.down;

            RaycastHit t_hit;
            if (Physics.Raycast(t_offset, t_axis, out t_hit))
            {
                Vector3 t_finalSpawnPos = t_hit.point;
                if (!t_hit.collider.CompareTag("Tree") && !t_hit.collider.CompareTag("Rock"))
                {
                    _rocks.Add(Instantiate(t_tree, t_finalSpawnPos, Quaternion.identity));
                    if (t_rockInfoArray != null) t_rockInfoArray.Rocks.Add(new Rock(t_finalSpawnPos, Quaternion.identity, t_tree));
                }
            }
        }

        if (t_rockInfoArray != null) t_rockInfoArray.Save();
        if (t_loadingscreenManager != null) t_loadingscreenManager.CloseLoadingscreen();
        yield return null;
    }

    private IEnumerator Wait(float p_input)
    {
        yield return new WaitForSeconds(p_input);
    }

    private const string _treeDataArrayID = "TREE_DATA";
    IEnumerator PlaceTrees(Orientation p_orientation, Transform p_position)
    {
        LoadingscreenManager t_loadingscreenManager = LoadingscreenManager.Instance;
        UIManager t_uiManager = UIManager.Instance;
        TreeInfo t_treeInfoArray = null;
        if (_dataReferences != null) t_treeInfoArray = _dataReferences.FindElement<TreeInfo>(_treeDataArrayID);
        if (t_treeInfoArray == null && _dataReferences != null)
        {
            _dataReferences.AddElement<TreeInfo>(_treeDataArrayID);
            t_treeInfoArray = _dataReferences.FindElement<TreeInfo>(_treeDataArrayID);
        }

        if (t_loadingscreenManager != null) t_loadingscreenManager.OpenLoadingscreen("Generating World.");

        for (int i = 0; i < _treeGen.amount; i++)
        {
            if (i % (_treeGen.amount / 25) == 0 && i != 0)
                yield return new WaitForSecondsRealtime(0.01f);

            if (t_loadingscreenManager != null) t_uiManager.LoadingBar.value = t_uiManager.LoadingBar.maxValue * i / _treeGen.amount;

            GameObject t_tree = _treeGen.obj[UnityEngine.Random.Range(0, _treeGen.obj.Length)];
            Vector2 t_spawnPosV2 = UnityEngine.Random.insideUnitCircle * _treeGen.radius;
            Vector3 t_spawnPos = new Vector3(t_spawnPosV2.x, 0, t_spawnPosV2.y);
            Vector3 t_offset = p_position.position + t_spawnPos;

            Vector3 t_axis = new Vector3();
            if (p_orientation == Orientation.Up) t_axis = Vector3.up;
            if (p_orientation == Orientation.Down) t_axis = Vector3.down;

            RaycastHit t_hit;
            if (Physics.Raycast(t_offset, t_axis, out t_hit))
            {
                Vector3 t_finalSpawnPos = t_hit.point;
                if (!t_hit.collider.CompareTag("Tree") && !t_hit.collider.CompareTag("Rock"))
                {
                    _trees.Add(Instantiate(t_tree, t_finalSpawnPos, Quaternion.identity));
                    if (t_treeInfoArray != null) t_treeInfoArray.Trees.Add(new Tree(t_finalSpawnPos, Quaternion.identity, t_tree));
                }
            }
        }

        if (t_treeInfoArray != null) t_treeInfoArray.Save();
        if (t_loadingscreenManager != null) t_loadingscreenManager.CloseLoadingscreen();
        yield return null;
    }

    IEnumerator PlaceExistingObjects()
    {
        LoadingscreenManager t_loadingscreenManager = LoadingscreenManager.Instance;
        UIManager t_uiManager = UIManager.Instance;

        TreeInfo t_treeInfoArray = _dataReferences.FindElement<TreeInfo>(_treeDataArrayID);
        BuildingInfo t_buildingInfoArray = _dataReferences.FindElement<BuildingInfo>("BUILDING_DATA");
        RockInfo t_rockInfoArray = _dataReferences.FindElement<RockInfo>(_rockDataArrayID);

        if (t_treeInfoArray != null)
        {
            if(t_treeInfoArray.Trees.Count > 0) t_loadingscreenManager.OpenLoadingscreen("Placing Trees.");

            for (int i = 0; i < t_treeInfoArray.Trees.Count; i++)
            {
                t_uiManager.LoadingBar.value = t_uiManager.LoadingBar.maxValue * i / t_treeInfoArray.Trees.Count;

                GameObject t_obj = Instantiate(_treeGen.obj[UnityEngine.Random.Range(0, _treeGen.obj.Length)], t_treeInfoArray.Trees[i].Position, t_treeInfoArray.Trees[i].Rotation);
                Trees.Add(t_obj);

                if (i % (t_treeInfoArray.Trees.Count / 50) == 0 && i != 0)
                    yield return new WaitForSecondsRealtime(0.01f);
            }
            t_uiManager.LoadingBar.value = 0;
        }

        if (t_rockInfoArray != null)
        {
            if (t_rockInfoArray.Rocks.Count > 0) t_loadingscreenManager.OpenLoadingscreen("Placing Rocks.");

            for (int i = 0; i < t_rockInfoArray.Rocks.Count; i++)
            {
                t_uiManager.LoadingBar.value = t_uiManager.LoadingBar.maxValue * i / t_rockInfoArray.Rocks.Count;

                GameObject t_obj = Instantiate(_rockGen.obj[UnityEngine.Random.Range(0, _rockGen.obj.Length)], t_rockInfoArray.Rocks[i].Position, t_rockInfoArray.Rocks[i].Rotation);
                Trees.Add(t_obj);

                if (i % (t_rockInfoArray.Rocks.Count / 50) == 0 && i != 0)
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
                Buildings.Add(t_obj);
                yield return new WaitForSecondsRealtime(0.01f);
            }
            t_uiManager.LoadingBar.value = 0;
        }
        if (t_loadingscreenManager != null) t_loadingscreenManager.CloseLoadingscreen();
        yield return null;
    }

    private void ClearExistingObjects()
    {
        for(int i = 0; i < Trees.Count; i++) 
        {
            if (Trees.Count != 0)
                Destroy(Trees[i].gameObject);
        }
        for (int i = 0; i < Buildings.Count; i++)
        {
            if (Buildings.Count != 0)
                Destroy(Buildings[i].gameObject);
        }

        Buildings.Clear();
        Trees.Clear();
    }
}

