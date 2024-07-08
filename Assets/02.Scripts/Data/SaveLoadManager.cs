using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace _02.Scirpts.Data
{
    public class SaveLoadManager : Singleton<SaveLoadManager>
    {
        [SerializeField] private string filename = "SaveData.json";
        private string FullPath => Path.Combine(Application.persistentDataPath,filename);
        public SaveData SaveData = default;
        
        /// <summary>
        /// 데이터를 불러와서 매니저에 저장
        /// </summary>
        public void Load()
        {
            if (File.Exists(FullPath))
            {
                string json = File.ReadAllText(FullPath);
                SaveData = JsonUtility.FromJson<SaveData>(json);
            }
            else
            {
                SaveData = new SaveData(){};
            }
;       }

        public void Save()
        {
            string json = JsonUtility.ToJson(SaveData);
            
            if (File.Exists(FullPath))
            {
                File.Delete(FullPath);
            }
            File.WriteAllText(FullPath,json);
        }
    }
}
