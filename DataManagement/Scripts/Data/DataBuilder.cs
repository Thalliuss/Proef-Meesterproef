﻿using UnityEngine;

using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DataManagement
{
    public static class DataBuilder
    {
        public static string Decrypt(string p_input)
        {
            if (DataManager.Instance.encrypt)
            {
                byte[] t_inputbuffer = System.Convert.FromBase64String(p_input);
                byte[] t_outputBuffer = DES.Create().CreateDecryptor(DataReferences.key, DataReferences.iv).TransformFinalBlock(t_inputbuffer, 0, t_inputbuffer.Length);
                return Encoding.Unicode.GetString(t_outputBuffer);
            }
            else return p_input;
        }

        public static void BuildDataReferences()
        {
            SceneManager t_sceneManager = SceneManager.Instance;
            string t_path = Application.persistentDataPath + "/" + DataManager.Instance.ID + "/" + t_sceneManager.DataReferences.ID + "/" + t_sceneManager.DataReferences.ID + ".json";

            if (File.Exists(t_path))
            {
                JsonUtility.FromJsonOverwrite(Decrypt(File.ReadAllText(t_path)), t_sceneManager.DataReferences);
                Debug.Log("Building Data from: " + t_path);
            }
        }

        public static void BuildElementsOfType<T>(DataReferences.SavedElement p_saveData) where T : DataElement
        {
            for (int i = 0; i < p_saveData.ids.Count; i++)
            {
                if (p_saveData.types[i] == typeof(T).Name)
                    BuildElementOfType<T>(p_saveData, i);

                if (p_saveData.info[i] != null)
                    p_saveData.info[i].Build<T>();
            }
        }

        public static void BuildElementOfType<T>(DataReferences.SavedElement p_saveData, int p_index) where T : DataElement
        {
            string t_id = p_saveData.ids[p_index].ToString();
            string t_path = Application.persistentDataPath + "/" + DataManager.Instance.ID + "/" + SceneManager.Instance.DataReferences.ID + "/" + t_id + ".json";

            if (File.Exists(t_path))
            {
                T t_element = DataParser.CreateAsset<T>(t_id) as T;
                JsonUtility.FromJsonOverwrite(Decrypt(File.ReadAllText(t_path)), t_element);

                p_saveData.info[p_index] = t_element as T;
            }
        }
    }
}

