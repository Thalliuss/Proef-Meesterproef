using DataManagement;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommandPrompt : MonoBehaviour
{
    [SerializeField] private InputField _input;
    [SerializeField] private Text _field;

    private DataManager _dataManager;
    private const string _save = "/save ";
    private const string _exit = "/exit";
    private const string _godmode = "/godmode";
    private const string _pause = "/pause";
    private const string _unpause = "/unpause";

    private List<string> _commands = new List<string>();

    private void Start()
    {
        _dataManager = DataManager.Instance;

        _commands.Add(_save);
        _commands.Add(_exit);
        _commands.Add(_godmode);
        _commands.Add(_pause);
        _commands.Add(_unpause);

        EventSystem.current.SetSelectedGameObject(_input.gameObject, null);
        _input.OnPointerClick(new PointerEventData(EventSystem.current));
    }

    private void Command()
    {
        _field.text += _input.text + "\n";

        if (_input.text.Contains(_save))
        {
            string t_saveInput = _input.text.Replace(_save, "");
            t_saveInput = t_saveInput.TrimEnd();
            if (t_saveInput == "")
            {
                _field.text += new NullReferenceException() + "\n";
                return;
            }

            _dataManager.GenerateSave(t_saveInput.ToUpper());
            _input.text = "";
            _field.text += "Succesfull. \n";
        }
        else if (_input.text == _godmode)
        {
            ResourceManager t_resourceManager = ResourceManager.Instance;
            _field.text += "Succesfully initiated godmode. \n";
            t_resourceManager.Wood = 100000;
            t_resourceManager.Rock = 100000;

            SceneManager t_sceneManager = SceneManager.Instance;
            DataReferences t_dataReferences = t_sceneManager.DataReferences;

            ResourceInfo t_resourceInfo = t_dataReferences.FindElement<ResourceInfo>("RESOURCE_DATA");
            t_resourceInfo.Wood = 100000;
            t_resourceInfo.Rock = 100000;

            t_resourceInfo.Save();
        }
        else if (_input.text == _exit)
        {
            Application.Quit();
        }
        else if (_input.text == _pause)
        {
            UIManager t_uiManager = UIManager.Instance;

            t_uiManager.SetTimeScale(0);
            t_uiManager.ClosePrompt();
            t_uiManager.LockCursor(true);
            t_uiManager.CommandsOpened = !t_uiManager.CommandsOpened;

        }
        else if (_input.text == _unpause)
        {
            UIManager t_uiManager = UIManager.Instance;

            t_uiManager.SetTimeScale(1);
            t_uiManager.ClosePrompt();
            t_uiManager.LockCursor(true);
            t_uiManager.CommandsOpened = !t_uiManager.CommandsOpened;

        }
        else _field.text += new InvalidOperationException() + "\n"; ;
    }

    private int _temp = 0;
    private void ShowCommands()
    {
        _input.text = _commands[_temp];
        _input.MoveTextEnd(false);

        _temp++;

        if (_temp > _commands.Count - 1) _temp = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) && _input.text != "") Command();

        if (Input.GetKeyDown(KeyCode.UpArrow)) ShowCommands();
    }
}
