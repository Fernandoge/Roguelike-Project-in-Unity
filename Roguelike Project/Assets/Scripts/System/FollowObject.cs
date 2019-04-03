using UnityEngine;

public class FollowObject : MonoBehaviour
{

    [SerializeField]
    private GameObject objectToFollow;
    bool followingObject = true;

    // Update is called once per frame
    void Update()
    {
        if (followingObject == true)
        {
            FollowingObjectPosition();
        }

    }

    void FollowingObjectPosition()
    {
        Vector3 newPos = new Vector3(objectToFollow.transform.position.x, objectToFollow.transform.position.y, this.transform.position.z);
        transform.position = newPos;
    }
}