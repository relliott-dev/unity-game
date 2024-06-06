using UnityEngine;
using System.Collections;

namespace K2Framework
{
    public class FollowObject : MonoBehaviour
    {
        public Transform objectToFollow;
        public Vector3 followAxis;//only follow on the given axis. 0 means don't follow and 1 follow
        Vector3 offset;

        void Awake()
        {
            if(objectToFollow != null)
                offset = transform.position - objectToFollow.transform.position;
        }

        void Update()
        {
            if(objectToFollow != null)
            {
                Vector3 target = transform.position;

                if(followAxis.x != 0)
                    target.x = objectToFollow.transform.position.x + offset.x;

                if(followAxis.y != 0)
                    target.y = objectToFollow.transform.position.y + offset.y;

                if(followAxis.z != 0)
                    target.z = objectToFollow.transform.position.z + offset.z;

                transform.position = target;
            }
        }
    }
}