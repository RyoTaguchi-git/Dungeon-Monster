using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Vector3 Cameradistance;
    [SerializeField] private float cameraRotateSpeed = 90f;
    [SerializeField] private Transform target;
    [SerializeField] private LayerMask obstacleLayer;

    [SerializeField] private Vector3 Up;
    Renderer rendererhit;
    void Start()
    {
     
      
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + Cameradistance;
        RaycastHit hit;
        //　キャラクターとカメラの間に障害物があったら障害物の位置にカメラを移動させる
        if (Physics.Linecast(target.position + Up, transform.position, out hit, obstacleLayer))
        {
            transform.position = hit.point;
           
        }
        //　レイを視覚的に確認
        Debug.DrawLine(target.position, transform.position, Color.red, 0f, false);

       
          transform.rotation = Quaternion.Slerp  (transform.rotation, Quaternion.LookRotation(target.position - transform.position + Up), cameraRotateSpeed * Time.deltaTime);

        
    }
}
