using UnityEngine;
using DataManagement;


public class Player : MonoBehaviour
{
    public GameObject prefab;

    private DataManager _dataManager;
    private DataReferences _dataReferences;

    private void Awake()
    {
        _dataManager = DataManager.Instance;
        _dataReferences = _dataManager.DataReferences;
    }

    private void Start()
    {
    }

    private void Update()
    {
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

                if (!t_hit.collider.CompareTag("Tower") && !UIManager.Instance.MenuOpened) {
                    Instantiate(prefab, t_hit.point, Quaternion.identity);

                    Building t_obj = new Building(t_hit.point, Quaternion.identity, prefab);

                    t_buildingInfoArray.Buildings.Add(t_obj);
                    t_buildingInfoArray.Save();
                }
            }
        }
    }
}
