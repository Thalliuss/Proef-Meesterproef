using UnityEngine;
using DataManagement;


public class Player : MonoBehaviour
{
    public GameObject prefab;

    private SceneManger _sceneManager;
    private DataReferences _dataReferences;

    private void Update()
    {
        _sceneManager = SceneManger.Instance;
        _dataReferences = _sceneManager.DataReferences;

        PlaceBuilding();
    }

    private void PlaceBuilding()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _dataReferences.AddElement<BuildingInfo>(new BuildingInfo("BUILDING_DATA"));

            RaycastHit t_hit;
            Ray t_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(t_ray, out t_hit))
            {
                BuildingInfo t_buildingInfoArray = _dataReferences.FindElement<BuildingInfo>("BUILDING_DATA");

                if (!t_hit.collider.CompareTag("Tower")) {

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
}
