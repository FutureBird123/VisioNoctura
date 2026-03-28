using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorOpening : MonoBehaviour
{
    //float degreesPerSecond = -30f;
    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        if (transform.rotation.y != 270f)
    //        {
    //            transform.Rotate(Vector3.up * degreesPerSecond);
    //            Debug.Log("Player entered the trigger");
    //        }
    //    }
    //}

    [SerializeField] float targetY = 270f; // 270° == -90°
    float startY;
    [SerializeField] float speedDegreesPerSecond = 90f;
    [SerializeField] float stopToleranceDegrees = 0.5f;

    bool opening = false;
    void Start()
    {
        startY = transform.eulerAngles.y;
        targetY = startY + targetY; // convert to relative
    }
    void Update()
    {
        if (!opening) return;

        float currentY = transform.eulerAngles.y;
        float nextY = Mathf.MoveTowardsAngle(currentY, targetY, speedDegreesPerSecond * Time.deltaTime);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, nextY, transform.eulerAngles.z);

        if (Mathf.Abs(Mathf.DeltaAngle(nextY, targetY)) <= stopToleranceDegrees)
        {
            // Snap to target and stop
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, targetY, transform.eulerAngles.z);
            opening = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            opening = true;
            Debug.Log("Player collided - starting door open");
        }
    }
}
