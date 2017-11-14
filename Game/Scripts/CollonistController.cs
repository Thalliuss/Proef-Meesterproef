using DataManagement;
using System.Collections;
using UnityEngine;

public class CollonistController : MonoBehaviour
{
    public bool CanMove
    {
        get
        {
            return _canMove;
        }

        set
        {
            _canMove = value;
        }
    }
    private bool _canMove = false;

    private Vector3 _currentPosition;

    private void Awake()
    {
        GetComponent<LineRenderer>().SetPosition(0, transform.position);
        GetComponent<LineRenderer>().SetPosition(1, transform.position);
    }

    private void Start()
    {
        _currentPosition = transform.position;

    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1) && _canMove)
        {
            Ray t_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            StartCoroutine(MoveTowards(t_ray));

            GetComponent<LineRenderer>().enabled = true;
        }
    }

    [SerializeField] private LayerMask _layerMask;
    private IEnumerator MoveTowards(Ray p_ray)
    {
        RaycastHit t_hit;
        if (Physics.Raycast(p_ray, out t_hit, _layerMask))
        {
            Vector3 targetPostition = new Vector3(t_hit.point.x, t_hit.point.y, t_hit.point.z);
            transform.LookAt(targetPostition);

            yield return new WaitForSeconds(.1f);

            while (true)
            {
                GetComponent<LineRenderer>().SetPosition(0, transform.position);
                GetComponent<LineRenderer>().SetPosition(1, t_hit.point);

                transform.Translate(transform.forward / .2f * Time.deltaTime, Space.World);

                Ray t_ray = new Ray(transform.position, transform.forward);
                RaycastHit t_hitJump;
                if (Physics.Raycast(t_ray, out t_hitJump, 1.5f)) {
                    GetComponent<Rigidbody>().AddForce(transform.up * 2000);
                }

                _canMove = false;

                if (Vector3.Distance(transform.position, t_hit.point) <= 10) transform.position = Vector3.MoveTowards(transform.position, t_hit.point, 5f * Time.deltaTime);

                yield return new WaitForSeconds(.01f);

                if ((int)transform.position.x == (int)t_hit.point.x && (int)transform.position.z == (int)t_hit.point.z)
                {
                    GetComponent<LineRenderer>().enabled = false;
                    SceneManager t_sceneManager = SceneManager.Instance;
                    DataReferences t_dataReferences = t_sceneManager.DataReferences;

                    for (int i = 0; i < t_dataReferences.FindElement<CollonistInfo>("COLLONIST_DATA").Collonists.Count; i++)
                    {
                        if (t_dataReferences.FindElement<CollonistInfo>("COLLONIST_DATA") != null && t_dataReferences.FindElement<CollonistInfo>("COLLONIST_DATA").Collonists[i].Position == _currentPosition)
                        {
                            _currentPosition = transform.position;
                            t_dataReferences.FindElement<CollonistInfo>("COLLONIST_DATA").Collonists[i].Position = transform.position;
                            t_dataReferences.FindElement<CollonistInfo>("COLLONIST_DATA").Save();
                        }
                    }
      
                    break;
                } 
            }
        }
    }

    private bool _trigger = false;
    private void OnTriggerEnter(Collider p_other)
    {
        if (p_other.CompareTag("Tree"))
        {
            _trigger = true;
            StartCoroutine(ChopWood(p_other));
        }
        if (p_other.CompareTag("Rock"))
        {
            _trigger = true;
            StartCoroutine(MineRock(p_other));
        }
    }

    private IEnumerator ChopWood(Collider p_other)
    {
        while (_trigger)
        {
            yield return new WaitForSeconds(5f);
            ResourceManager.Instance.AddWood(UnityEngine.Random.Range(4, 11));
        }
    }

    private IEnumerator MineRock(Collider p_other)
    {
        while (_trigger)
        {
            yield return new WaitForSeconds(5f);
            ResourceManager.Instance.AddRock(UnityEngine.Random.Range(4, 11));
        }
    }

    private void OnTriggerExit(Collider p_other)
    {
        if (p_other.CompareTag("Tree")) _trigger = false;
    }
}