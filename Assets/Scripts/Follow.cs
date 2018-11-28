using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour
{

    public GameObject objectToFollow;

    public float speed = 2.0f;
    public float distance;

    public bool follow = true;

    void Update()
    {
        if (!follow)
            return;

        float interpolation = speed * Time.fixedUnscaledDeltaTime;

        Vector3 position = this.transform.position;
        position.y = Mathf.Lerp(this.transform.position.y, objectToFollow.transform.position.y + distance, interpolation);
        position.x = Mathf.Lerp(this.transform.position.x, objectToFollow.transform.position.x, interpolation);

        this.transform.position = position;
    }
}