#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

using System;
using System.IO;

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

        [Header("Enable/Disable Encryption.")]
        public bool encrypt;

        [Header("Enable/Disable Multiple Save Files."), SerializeField]
        private bool multipleSaves;

        public DataReferences DataReferences
        {
            get
            {
                return _dataReferences;
            }
        }
        [Header("Data."), SerializeField]
        private DataReferences _dataReferences;

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
            _dataReferences.initialID = _dataReferences.ID;

            string t_path = Application.persistentDataPath + "/" + _dataReferences.ID + "/";
            if (!Directory.Exists(t_path))
                Directory.CreateDirectory(t_path);

            if (_instance != null)
                Destroy(gameObject);

            _instance = this;

            Build();

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
            DataBuilder.BuildDataReferences(); 

            DataBuilder.BuildElementsOfType<TreeInfo>(_dataReferences.SaveData);
            DataBuilder.BuildElementsOfType<BuildingInfo>(_dataReferences.SaveData);

        }

        private void OnDestroy()
        {
            _dataReferences.SaveData.ids.Clear();
            _dataReferences.SaveData.info.Clear();
            _dataReferences.SaveData.types.Clear();
            _dataReferences.ID = _dataReferences.initialID;
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
                if (Directory.Exists(_path + _dataReferences.initialID + "/"))
                {
                    Directory.CreateDirectory(_path + _dataReferences.initialID + "_" + t_time);

                    for (uint i = 0; i < Directory.GetFiles(_path + _dataReferences.ID).Length; i++)
                        File.Copy(Directory.GetFiles(_path + _dataReferences.ID)[i], Directory.GetFiles(_path + _dataReferences.ID)[i].Replace(_dataReferences.ID, _dataReferences.initialID + "_" + t_time));

                    Debug.Log("Saving Data to: " + _path + _dataReferences.initialID + "_" + t_time);

                    SaveReferences.Init();
                    _dataReferences.ID = _dataReferences.initialID + "_" + t_time;
                    _dataReferences.Save();
                }
            }
        }
        [ContextMenu("Manual Override.")]
        public void OverrideSave() { _dataReferences.Save(); }

        public void Load()
        {
            if (multipleSaves)
            {
                _dataReferences.ID = SaveReferences.saveData[SaveReferences.load.value];

                _dataReferences.SaveData.ids.Clear();
                _dataReferences.SaveData.info.Clear();
                _dataReferences.SaveData.types.Clear();

                Build();
            }
        }

        [ContextMenu("Clear All Data.")]
        public void ClearAllData()
        {
            string t_path = Application.persistentDataPath + "/";
            string[] t_data = Directory.GetDirectories(t_path);
            for (uint i = 0; i < t_data.Length; i++)
            {
                if (t_data[i].Contains(_dataReferences.ID))
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


