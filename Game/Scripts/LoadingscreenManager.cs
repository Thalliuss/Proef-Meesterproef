using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingscreenManager : MonoBehaviour
{
    private static LoadingscreenManager _instance;
    public static LoadingscreenManager Instance
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

    public bool IsLoading
    {
        get
        {
            return _isLoading;
        }

        set
        {
            _isLoading = value;
        }
    }
    private bool _isLoading = false;

    private string _levelToLoad;

    private void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);

        _instance = this;

        DontDestroyOnLoad(this);
    }

    public void LoadScene(string p_input)
    {
        _levelToLoad = p_input;
        SceneManager.LoadScene(_levelToLoad);
    }

    public void OpenLoadingscreen(string p_text)
    {
        UIManager t_uiManager = UIManager.Instance;

        t_uiManager.OpenLoading(p_text);
        _isLoading = true;

        if (SceneManager.GetActiveScene().buildIndex == 1) t_uiManager.OpenHUD();
        else t_uiManager.CloseHUD();
    }

    public void CloseLoadingscreen()
    {
        UIManager t_uiManager = UIManager.Instance;

        t_uiManager.CloseLoading();
        _isLoading = false;
    }
}

