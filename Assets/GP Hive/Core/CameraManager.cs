using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace GPHive.Core
{
    public class CameraManager : Singleton<CameraManager>
    {
        [System.Serializable]
        public class SerializableDictionary
        {
            public string key;
            public CinemachineVirtualCamera value;
        }

        public List<SerializableDictionary> cameras;
        Dictionary<string, CinemachineVirtualCamera> cameraList = new Dictionary<string, CinemachineVirtualCamera>();




        private void Awake()
        {
            foreach (SerializableDictionary item in cameras)
            {
                cameraList.Add(item.key, item.value);
            }
        }


        public void SwitchCamera(string _key)
        {
            if (!cameraList.ContainsKey(_key))
            {
                Debug.LogError($"Virtual Camera {_key} doesn't exist.", gameObject);
                return;
            }

            foreach (CinemachineVirtualCamera cam in cameraList.Values)
            {
                cam.Priority = 0;
            }

            cameraList[_key].Priority = 1;
        }
    }
}
