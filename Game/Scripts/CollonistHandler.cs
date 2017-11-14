using DataManagement;
using UnityEngine;
using UnityEngine.UI;

public class CollonistHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] _collonists;
    [SerializeField] private Text _text;

    [SerializeField] private GameObject _prefab;

    private SceneManager _sceneManager;
    private DataReferences _dataReferences;

    private const string _collonistDataArrayID = "COLLONIST_DATA";
    private void Start()
    {
        _sceneManager = SceneManager.Instance;
        _dataReferences = _sceneManager.DataReferences;

        CollonistInfo t_collonistInfoArray = null;
        if (_dataReferences != null) t_collonistInfoArray = _dataReferences.FindElement<CollonistInfo>(_collonistDataArrayID);
        if (t_collonistInfoArray != null && _dataReferences != null)
        {
            for (int i = 0; i < _collonists.Length; i++)
            {
                _collonists[i].transform.position = t_collonistInfoArray.Collonists[i].Position;
                _collonists[i].SetActive(true);
                _collonists[i].transform.parent = _collonists[i].transform.parent.parent;

                _collonists[i].GetComponent<CollonistController>().enabled = true;
                _collonists[i].GetComponent<GravityHandler>().enabled = true;

                Destroy(gameObject);
            }
            _text.enabled = false;
        }
    }

    private void Update ()
    {
        Ray t_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit t_hit;
        if (Physics.Raycast(t_ray, out t_hit))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                for (uint i = 0; i < _collonists.Length; i++)
                {
                    transform.position = t_hit.point;
                    _collonists[i].SetActive(true);
                    _collonists[i].transform.parent = _collonists[i].transform.parent.parent;
                    Destroy(gameObject);

                    _collonists[i].GetComponent<CollonistController>().enabled = true;
                    _collonists[i].GetComponent<GravityHandler>().enabled = true;

                    _text.enabled = false;

                    CollonistInfo t_collonistInfoArray = null;
                    if (_dataReferences != null) t_collonistInfoArray = _dataReferences.FindElement<CollonistInfo>(_collonistDataArrayID);
                    if (t_collonistInfoArray == null && _dataReferences != null)
                    {
                        _dataReferences.AddElement<CollonistInfo>(_collonistDataArrayID);
                        t_collonistInfoArray = _dataReferences.FindElement<CollonistInfo>(_collonistDataArrayID);
                    }
                    if (_dataReferences.FindElement<CollonistInfo>(_collonistDataArrayID) != null)
                        _dataReferences.FindElement<CollonistInfo>(_collonistDataArrayID).Collonists.Add(new Collonist(_collonists[i].transform.position, Quaternion.identity, _prefab));

                    t_collonistInfoArray.Save();
                }
            }
        }
    }
}
