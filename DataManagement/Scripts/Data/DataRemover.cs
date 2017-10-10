using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

using System.IO;

namespace DataManagement
{
    public class DataRemover : MonoBehaviour
    {
        public void ClearData()
        {
            DataManager t_dataManager = DataManager.Instance;

            Transform t_parent = transform.parent;
            for (int i = 0; i < t_parent.name.ToCharArray().Length; i++)
            {
                if (char.IsDigit(t_parent.name, i) && i <= 6)
                {
                    var t_value = (int)char.GetNumericValue(t_parent.name.ToCharArray()[i]);

                    Debug.Log("Cleaning Data from: " + Application.persistentDataPath + "/" + t_dataManager.SaveReferences.saveData[t_value]);
                    Directory.Delete(Application.persistentDataPath + "/" + t_dataManager.SaveReferences.saveData[t_value], true);

                    if (t_dataManager.DataReferences.ID == t_dataManager.SaveReferences.saveData[t_value])
                        t_dataManager.DataReferences.ID = t_dataManager.DataReferences.initialID;

                    t_dataManager.SaveReferences.load.options.RemoveAt(t_value);
                    t_dataManager.SaveReferences.Init();
                }
            }
        }
    }
}
