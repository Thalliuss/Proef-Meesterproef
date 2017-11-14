using UnityEngine;
using DataManagement;
using System.Collections.Generic;
using System.Collections;

public class Player : MonoBehaviour
{
    public GameObject prefab;

    [SerializeField]
    private GameObject _buildingUI;

    private SceneManager _sceneManager;
    private DataReferences _dataReferences;
    
    [SerializeField] List<GameObject> _colonists = new List<GameObject>();

    private void Update()
    {
        _sceneManager = SceneManager.Instance;
        if (_sceneManager != null) _dataReferences = _sceneManager.DataReferences;
    
        StartCoroutine(SelectPlayer());

        if (_buildingUI.activeSelf == true)
        {
            ResourceManager t_resourceManager = ResourceManager.Instance;
            if (Input.GetKeyDown(KeyCode.B) && t_resourceManager.Wood >= 500 && t_resourceManager.Rock >= 500)
            {
                PlaceBuilding();
                t_resourceManager.Wood -= 500;
                t_resourceManager.Rock -= 500;

                var t_resourceInfo = _dataReferences.FindElement<ResourceInfo>("RESOURCE_DATA");
                t_resourceInfo.Wood = t_resourceManager.Wood;
                t_resourceInfo.Rock = t_resourceManager.Rock;
                t_resourceInfo.Save();
            }
        }
    }

    private IEnumerator SelectPlayer()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit t_hit;
            Ray t_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(t_ray, out t_hit) && Time.timeScale == 1)
            {
                if (t_hit.collider.CompareTag("Colonist"))
                {
                    for (int i = 0; i < _colonists.Count; i++)
                    {
                        _colonists[i].GetComponent<MeshRenderer>().material.color = Color.white;
                        _colonists[i].GetComponent<CollonistController>().CanMove = false;
                        _buildingUI.SetActive(false);
                    }

                    t_hit.transform.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                    t_hit.transform.gameObject.GetComponent<CollonistController>().CanMove = true;
                    _buildingUI.SetActive(true);

                    yield return null;
                }
                else
                {
                    for (int i = 0; i < _colonists.Count; i++)
                    {
                        _colonists[i].GetComponent<MeshRenderer>().material.color = Color.white;
                        _colonists[i].GetComponent<CollonistController>().CanMove = false;
                        _buildingUI.SetActive(false);
                    }
                }

            }
        }
    }

    private const string _buildingInfoID = "BUILDING_DATA";
    private void PlaceBuilding()
    {
        if (_dataReferences != null)
            _dataReferences.AddElement<BuildingInfo>(_buildingInfoID);

        RaycastHit t_hit;
        Ray t_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(t_ray, out t_hit) && Time.timeScale == 1)
        {
            BuildingInfo t_buildingInfoArray = null;
            if (_dataReferences != null)
                t_buildingInfoArray = _dataReferences.FindElement<BuildingInfo>(_buildingInfoID);

            if (!t_hit.collider.CompareTag("Tree") && !t_hit.collider.CompareTag("Tower") && !t_hit.collider.CompareTag("Tree"))
            {
                if (UIManager.Instance != null && !UIManager.Instance.MenuOpened)
                {
                    GenerationManager.Instance.Buildings.Add(Instantiate(prefab, t_hit.point, Quaternion.identity));

                    Building t_obj = new Building(t_hit.point, Quaternion.identity, prefab);

                    if (t_buildingInfoArray != null)
                    {
                        t_buildingInfoArray.Buildings.Add(t_obj);
                        t_buildingInfoArray.Save();
                    }
                }
                else if (UIManager.Instance == null)
                {
                    GenerationManager.Instance.Buildings.Add(Instantiate(prefab, t_hit.point, Quaternion.identity));

                    Building t_obj = new Building(t_hit.point, Quaternion.identity, prefab);

                    if (t_buildingInfoArray != null)
                    {
                        t_buildingInfoArray.Buildings.Add(t_obj);
                        t_buildingInfoArray.Save();
                    }
                }
            }
        }
    }
}
