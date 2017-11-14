using UnityEngine;
using DataManagement;
using System.IO;

namespace DataManagement
{
    public class SceneManager : MonoBehaviour
    {
        private static SceneManager _instance;
        public static SceneManager Instance
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

        [SerializeField]
        private string _sceneID;

        public DataReferences DataReferences
        {
            get
            {
                return _dataReferences;
            }

            set
            {
                _dataReferences = value;
            }
        }
        [SerializeField]
        private DataReferences _dataReferences;

        private void Awake()
        {
            if (_instance != null)
                Destroy(gameObject);

            _instance = this;

            if (DataManager.Instance == null) return;

            _dataReferences.ID = _sceneID;

            string t_path = Application.persistentDataPath + "/" + DataManager.Instance.ID + "/" + _dataReferences.ID + "/";
            if (!Directory.Exists(t_path))
                Directory.CreateDirectory(t_path);

            Build();
        }

        private void OnDestroy()
        {
            ClearAllData();
        }

        public void ClearAllData()
        {
            _dataReferences.SaveData.ids.Clear();
            _dataReferences.SaveData.info.Clear();
            _dataReferences.SaveData.types.Clear();
        }

        private void Build()
        {
            DataManager t_dataManager = DataManager.Instance;
            if (t_dataManager != null) t_dataManager.Build();
        }
    }
}
