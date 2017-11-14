using System;
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
    [SerializeField] private GameObject _commands;
    [SerializeField] private GameObject _gui;

    [SerializeField] private Text _wood;
    [SerializeField] private Text _rock;

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

    public bool CommandsOpened
    {
        get
        {
            return _commandsOpened;
        }

        set
        {
            _commandsOpened = value;
        }
    }
    private bool _commandsOpened = false;

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
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void SetTimeScale(int p_input)
    {
        Time.timeScale = p_input;
    }

    public void Pause() { LockCursor(false); }
    public void UnPause() { LockCursor(true); }

    private void OpenMenu()
    {
        if (Input.GetKeyDown(KeyCode.E) && !LoadingscreenManager.Instance.IsLoading)
        {
            _menuOpened = !_menuOpened;
            _menu.SetActive(_menuOpened);

            if (_menuOpened == true)
            {
                Pause();
                SetTimeScale(0);
            }
            else
            {
                UnPause();
                SetTimeScale(1);
            }
        }
    }

    private void OpenPrompt()
    {
        if (Input.GetKeyDown(KeyCode.P) && !LoadingscreenManager.Instance.IsLoading)
        {
            _commandsOpened = !_commandsOpened;
            _commands.SetActive(_commandsOpened);

            if (_commandsOpened == true) Pause();
            else UnPause();
        }
    }

    private void OpenGUI()
    {
        if (!LoadingscreenManager.Instance.IsLoading && DataManagement.SceneManager.Instance != null)
        {
            _gui.SetActive(true);
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

    public void CloseGUI()
    {
        _gui.SetActive(false);
    }

    public void ClosePrompt()
    {
        _commands.SetActive(false);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void Update()
    {
        OpenMenu();
        OpenPrompt();
        OpenGUI();

        ResourceManager t_resourceManager = ResourceManager.Instance;
        if (t_resourceManager != null)
        {
            _wood.text = t_resourceManager.Wood.ToString();
            _rock.text = t_resourceManager.Rock.ToString();
        } 
    }
}
