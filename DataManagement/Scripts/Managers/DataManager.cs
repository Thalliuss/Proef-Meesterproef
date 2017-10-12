#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

using System.IO;
using System;

namespace DataManagement
{
    public class DataManager : MonoBehaviour
    {
        private static DataManager _instance;
        public static DataManager Instance
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

        public string SaveID
        {
            get
            {
                return _saveID;
            }

            set
            {
                _saveID = value;
            }
        }
        [SerializeField]
        private string _saveID;

        public string IntitialID
        {
            get
            {
                return _intitialID;
            }

            set
            {
                _intitialID = value;
            }
        }
        private string _intitialID;

        [Header("Enable/Disable Encryption.")]
        public bool encrypt;

        [Header("Enable/Disable Multiple Save Files."), SerializeField]
        private bool multipleSaves;

        public SaveReferences SaveReferences
        {
            get
            {
                return _saveReferences;
            }
        }
        [SerializeField] private SaveReferences _saveReferences;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            _intitialID = _saveID;

            string t_path = Application.persistentDataPath + "/" + _saveID + "/";
            if (!Directory.Exists(t_path))
                Directory.CreateDirectory(t_path);

            if (_instance != null)
                Destroy(gameObject);

            _instance = this;

            if (!multipleSaves)
            {
                if (_saveReferences.save != null)
                    _saveReferences.save.gameObject.SetActive(false);

                if (_saveReferences.load != null)
                    _saveReferences.load.gameObject.SetActive(false);
            }
            else SaveReferences.Init();
        }

        private void Build()
        {
            SceneManger t_sceneManager = SceneManger.Instance;

            DataBuilder.BuildDataReferences();
            DataBuilder.BuildElementsOfType<TreeInfo>(t_sceneManager.DataReferences.SaveData);
            DataBuilder.BuildElementsOfType<BuildingInfo>(t_sceneManager.DataReferences.SaveData);
        }

        [ContextMenu("Manual New Save.")]
        public void GenerateSave()
        {
            if (multipleSaves)
            {
                string t_time = DateTime.Now.ToString();

                t_time = t_time.Replace('/', '-');
                t_time = t_time.Replace(' ', '_');
                t_time = t_time.Replace(':', '-');

                string _path = Application.persistentDataPath + "/";
                if (Directory.Exists(_path + _intitialID + "/"))
                {
                    Directory.CreateDirectory(_path + _intitialID + "_" + t_time);

                    for (uint i = 0; i < Directory.GetDirectories(_path + _saveID).Length; i++)
                    {
                        string t_name = Directory.GetDirectories(_path + _saveID)[i];
                        Directory.CreateDirectory(t_name.Replace(_saveID, _intitialID + "_" + t_time));

                        for (uint a = 0; a < Directory.GetFiles(t_name).Length; a++)
                            File.Copy(Directory.GetFiles(t_name)[a], Directory.GetFiles(t_name)[a].Replace(_saveID, _intitialID + "_" + t_time));
                    }

                    Debug.Log("Saving Data to: " + _path + _intitialID + "_" + t_time);

                    SaveReferences.Init();
                    _saveID = _intitialID + "_" + t_time;
                }
            }
        }

        public void Load()
        {
            if (multipleSaves)
            {
                _saveID = SaveReferences.saveData[SaveReferences.load.value];

                GenerationManager t_generationManager = GenerationManager.Instance;
                SceneManger t_sceneManager = SceneManger.Instance;

                if (t_sceneManager != null)
                {
                    t_sceneManager.ClearAllData();
                    Build();
                }
                if (t_generationManager != null) t_generationManager.Init();
            }
        }

        [ContextMenu("Clear All Data.")]
        public void ClearAllData()
        {
            string t_path = Application.persistentDataPath + "/";
            string[] t_data = Directory.GetDirectories(t_path);
            for (uint i = 0; i < t_data.Length; i++)
            {
                if (t_data[i].Contains(_saveID))
                {
                    #if UNITY_EDITOR
                    if (Directory.Exists(t_data[i]))
                    {
                        FileUtil.DeleteFileOrDirectory(t_data[i]);
                        Debug.Log("Cleaning Data from: " + t_data[i]);
                    }
                    AssetDatabase.Refresh();
                    #endif
                }
            }
            ClearPlayerPrefs();
        }

        private void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}


