using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace net.EthanTFH.BTSGameJam
{
    public class Respawner : MonoBehaviour
    {
        private BoxCollider collider;

        void Start()
        {
            collider = this.GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            GameObject obj = other.gameObject;
            if(obj.CompareTag("Player"))
            {
                if (GameObject.Find("Floor"))
                    obj.transform.position = new Vector3(0f, GameObject.Find("Floor").transform.position.y + 5, 0f);
                else
                    obj.transform.position = new Vector3(0f, 5f, 0f);
            }
        }

    }
}