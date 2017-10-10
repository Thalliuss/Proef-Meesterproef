using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance
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

    private static UIManager _instance;
 
    public bool MenuOpened
    {
        get
        {
            return _menuOpened;
        }

        set
        {
            _menuOpened = value;
        }
    }

    private bool _menuOpened = false;

    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _loading;
    [SerializeField] private GameObject _hud;
    [SerializeField] private Text _loadingText;

    public Slider LoadingBar
    {
        get
        {
            return _loadingBar;
        }

        set
        {
            _loadingBar = value;
        }
    }
    [SerializeField] private Slider _loadingBar;

    private void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);

        _instance = this;

        DontDestroyOnLoad(this);
    }

    public void LockCursor(bool p_input)
    {
        if (p_input)
        {
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            Time.timeScale = 1;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
        }
    }

    private void OpenMenu()
    {
        if (Input.GetKeyDown(KeyCode.E) && !LoadingscreenManager.Instance.IsLoading)
        {
            _menuOpened = !MenuOpened;

            LockCursor(!_menuOpened);
            _menu.SetActive(_menuOpened);
        }
    }

    public void CloseMenu()
    {
        LockCursor(true);
        _menu.SetActive(false);
        _menuOpened = false;
    }

    public void OpenLoading(string p_input)
    {
        _loading.SetActive(true);
        _loadingText.text = p_input;
    }

    public void CloseLoading()
    {
        _loading.SetActive(false);
        _loadingText.text = "";
    }

    public void OpenHUD()
    {
        _hud.SetActive(true);
    }

    public void CloseHUD()
    {
        _hud.SetActive(false);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void Update()
    {
        OpenMenu();
    }
}
