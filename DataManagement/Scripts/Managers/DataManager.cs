using UnityEngine;
using System.IO;
using System;
using System.Linq;

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

        public string ID
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }
        private string _id;

        private const string _tempID = "temp";

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
        [SerializeField]
        private SaveReferences _saveReferences;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            _id = (CheckForLastFile() == null ? _tempID : CheckForLastFile());

            string t_path = Application.persistentDataPath + "/" + _id + "/";
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

        private string CheckForLastFile()
        {
            string t_path = Application.persistentDataPath + "/";

            var t_root = new DirectoryInfo(t_path);
            var t_dir = t_root.GetDirectories().OrderByDescending(f => f.LastWriteTime).FirstOrDefault();

            if (t_dir.Name != "Unity") return t_dir.Name;
            else return null;
        }

        public void Build()
        {
            SceneManager t_sceneManager = SceneManager.Instance;

            DataBuilder.BuildDataReferences();
            DataBuilder.BuildElementsOfType<TreeInfo>(t_sceneManager.DataReferences.SaveData);
            DataBuilder.BuildElementsOfType<BuildingInfo>(t_sceneManager.DataReferences.SaveData);
            DataBuilder.BuildElementsOfType<RockInfo>(t_sceneManager.DataReferences.SaveData);
            DataBuilder.BuildElementsOfType<ResourceInfo>(t_sceneManager.DataReferences.SaveData);
            DataBuilder.BuildElementsOfType<CollonistInfo>(t_sceneManager.DataReferences.SaveData);
        }

        public void GenerateSave()
        {
            if (multipleSaves)
            {
                string t_time = DateTime.Now.ToString();

                t_time = t_time.Replace("/", "-");
                t_time = t_time.Replace(" ", "_");
                t_time = t_time.Replace(":", "-");

                string _path = Application.persistentDataPath + "/";
                if (Directory.Exists(_path + _id + "/"))
                {
                    string t_temp = _path + (_id == _tempID ? "SAVE" : _id);
              
                    t_temp = t_temp.Replace(_path, "");
                    t_temp = t_temp.Replace("-", "");
                    t_temp = t_temp.Replace("_", "");
                    t_temp = t_temp.Replace(":", "");
                    t_temp = t_temp.Replace("PM", "");
                    t_temp = t_temp.Replace("AM", "");

                    for (int i = 0; i < t_temp.Length; i++)
                    {
                        if (char.IsDigit(t_temp[i]))
                            t_temp = t_temp.Replace(t_temp[i], ' ');
                    }
                    t_temp = t_temp.TrimEnd();

                    string t_newPath = t_temp + "_" + t_time;
                    t_newPath = t_newPath.Replace(" ", "_");

                    Directory.CreateDirectory(_path + t_newPath);

                    for (uint i = 0; i < Directory.GetDirectories(_path + _id).Length; i++)
                    {
                        string t_name = Directory.GetDirectories(_path + _id)[i];
                        Directory.CreateDirectory(t_name.Replace(_id, t_newPath));

                        for (uint a = 0; a < Directory.GetFiles(t_name).Length; a++)
                            File.Copy(Directory.GetFiles(t_name)[a], Directory.GetFiles(t_name)[a].Replace(_id, t_newPath));
                    }

                    Debug.Log("Saving Data to: " + t_newPath + "_" + t_time);

                    _id = t_newPath;
                    SaveReferences.Init();
                }
            }
        }
        public void GenerateSave(string p_input)
        {
            if (multipleSaves)
            {
                string t_time = DateTime.Now.ToString();

                t_time = t_time.Replace("/", "-");
                t_time = t_time.Replace(" ", "_");
                t_time = t_time.Replace(":", "-");

                string _path = Application.persistentDataPath + "/";
                if (Directory.Exists(_path + _id + "/"))
                {
                    Directory.CreateDirectory(_path + p_input.Replace(" ","_") + "_" + t_time);

                    for (uint i = 0; i < Directory.GetDirectories(_path + _id).Length; i++)
                    {
                        string t_name = Directory.GetDirectories(_path + _id)[i];
                        Directory.CreateDirectory(t_name.Replace(_id, p_input + "_" + t_time));

                        for (uint a = 0; a < Directory.GetFiles(t_name).Length; a++)
                            File.Copy(Directory.GetFiles(t_name)[a], Directory.GetFiles(t_name)[a].Replace(_id, p_input + "_" + t_time));
                    }

                    Debug.Log("Saving Data to: " + _path + p_input + "_" + t_time);

                    _id = p_input + "_" + t_time;
                    SaveReferences.Init();
                }
            }
        }

        public void Load()
        {
            if (multipleSaves)
            {
                _id = SaveReferences.saveData[SaveReferences.load.value];

                GenerationManager t_generationManager = GenerationManager.Instance;
                SceneManager t_sceneManager = SceneManager.Instance;

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
                if (!t_data[i].Contains("Unity"))
                {
                    if (Directory.Exists(t_data[i]))
                    {
                        Directory.Delete(t_data[i], true);
                        Debug.Log("Cleaning Data from: " + t_data[i]);
                    }
                }
            }
        }

        private void OnDestroy()
        {
            string t_temp = Application.persistentDataPath + "/" + _tempID + "/";
            if (Directory.Exists(t_temp))
                Directory.Delete(t_temp, true);
        }
    }
}


