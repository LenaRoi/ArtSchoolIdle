using UnityEngine;
using System.Collections;

namespace EKTemplate
{
    public class CameraManager : MonoBehaviour
    {
        [HideInInspector] public Camera cam;

        public Transform target;
        public Vector3 offset;
        public float camSpeed = 10;
        public float additionalPos;

        #region Singleton
        public static CameraManager instance = null;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            cam = GetComponent<Camera>();
        }
        #endregion

        private void Start()
        {
           // target = GameManager.instance.Player.transform;
        }
        void FixedUpdate()
        {
            Vector3 pos = new Vector3(target.position.x, target.position.y, target.position.z) +
            (Vector3.left * offset.x) + (Vector3.forward * offset.z) + (target.up * offset.y) + new Vector3(0, 0, 0);
            transform.position = Vector3.Lerp(transform.position, pos, Time.fixedDeltaTime * camSpeed);

            Vector3 dir = new Vector3(target.position.x, target.position.y, target.position.z + additionalPos) + new Vector3(0, 0, 0f) - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.fixedDeltaTime * camSpeed);
        }
    }
}