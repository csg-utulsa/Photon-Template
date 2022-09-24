using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace net.EthanTFH.BTSGameJam
{
    public class PlayerController : MonoBehaviourPun
    {
        [SerializeField]
        [field: Tooltip("Player NameTag GameObject")]
            private GameObject nameTag;

        [field: Tooltip("The Local Player Instance, can be accessed by other scripts.")]
            public static GameObject LocalPlayerInstance;

        [field: Header("Player Variables")]
        [field: SerializeField, Tooltip("Player Turn Speed"), Range(5.0f, 30.0f)] protected float turnSpeed { get; private set; }
        [field: SerializeField, Tooltip("Player Walk Speed"), Range(3.0f, 15.0f)] protected float walkSpeed { get; private set; }

        void Awake()
        {
            if (photonView.IsMine)
                PlayerController.LocalPlayerInstance = this.gameObject;

            Debug.LogFormat("Player Name: {0}", PhotonNetwork.NickName);
            Debug.LogFormat("PhotonView.IsMine: {0}", photonView.IsMine);
            Debug.LogFormat("LocalPlayerInstance: {0}", LocalPlayerInstance);

            DontDestroyOnLoad(this.gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            if (nameTag == null)
                nameTag = this.transform.Find("NameTag").gameObject;

            nameTag.GetComponent<TMPro.TextMeshPro>().text = photonView.Owner.NickName;
#if UNITY_5_4_OR_NEWER
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
#endif
        }

        // Update is called once per frame
        void FixedUpdate()
        {

            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
                return;

            // Code to Control Player Movemenet.
            if (Input.GetKey(KeyCode.W))
                transform.position = transform.position + transform.forward * walkSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.S))
                transform.position = transform.position - transform.forward * walkSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.A))
                transform.Rotate(Vector3.down * turnSpeed * 10 * Time.deltaTime);
            if (Input.GetKey(KeyCode.D))
                transform.Rotate(Vector3.up * turnSpeed * 10 * Time.deltaTime);

            GameObject[] nameTags = GameObject.FindGameObjectsWithTag("NameTag");
            for (int i = 0; i < nameTags.Length; i++)
            {
                nameTags[i].transform.LookAt(Camera.main.transform);
            }
        }
        
#if UNITY_5_4_OR_NEWER
        void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
        {
            this.CalledOnLevelWasLoaded(scene.buildIndex);
        }

        void OnLevelWasLoaded(int level)
        {
            this.CalledOnLevelWasLoaded(level);
        }

        void CalledOnLevelWasLoaded(int level)
        {
            if(!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                if (GameObject.Find("Floor"))
                    transform.position = new Vector3(0f, GameObject.Find("Floor").transform.position.y + 5, 0f);
                else
                    transform.position = new Vector3(0f, 5f, 0f);
            }
        }
#endif
    }
}