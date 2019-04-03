using UnityEngine;

public class FollowParent : MonoBehaviour
{

    [SerializeField]
    private GameObject parent;
    bool followingParent = true;

    // Update is called once per frame
    void Update()
    {
        if (followingParent == true)
        {
            FollowParentPosition();
        }

    }

    void FollowParentPosition()
    {
        Vector3 newPos = new Vector3(parent.transform.position.x, parent.transform.position.y, this.transform.position.z);
        transform.position = newPos;
    }
}