using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public Transform AimVec, PlayerVec;         //准心当前所在坐标 & 玩家当前所在坐标
    public float MouseSpeed, SmoothSpeed,AimSpeed;          //鼠标灵敏度 & 准心和相机到达目标坐标所需的近似时间
    public Vector3 SmoothVelocity,SmoothVelocityAim, Drift;  //准心和相机平滑插值时的当前速度 & 准心将要达到的坐标(相对与玩家)

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   //锁定光标
        Cursor.visible = false;     //隐藏光标
    }

    void Update()
    {                                               //将准心的x轴移动距离限制在两个摄像机水平视体之内；
        Drift = new Vector3(                          
            Mathf.Clamp(
            Input.GetAxis("Mouse X") * MouseSpeed * Time.deltaTime + Drift.x,
            -Camera.main.orthographicSize * Camera.main.aspect * 2,
            Camera.main.orthographicSize * Camera.main.aspect * 2),
            Mathf.Clamp(
            Input.GetAxis("Mouse Y") * MouseSpeed * Time.deltaTime + Drift.y,
            -Camera.main.orthographicSize * 2,
            Camera.main.orthographicSize * 2), 0);                      //将准心的y轴移动距离限制在两个摄像机垂直视体之内；

        AimVec.position = Vector3.SmoothDamp(AimVec.position,new Vector3(
           Drift.x + PlayerVec.position.x,
           Drift.y + PlayerVec.position.y, 0),ref SmoothVelocityAim,AimSpeed);//在准心的当前坐标和将要达到的坐标之间做平滑插值，并将结果赋值给准心当前坐标；
        
        transform.position = Vector3.SmoothDamp(transform.position,AimVec.position,ref SmoothVelocity, SmoothSpeed);        //在相机的当前坐标和将要达到的坐标之间做平滑插值，并将结果赋值给相机当前坐标；
        transform.position =        
            new Vector3(
                Mathf.Clamp(
                transform.position.x,
                PlayerVec.position.x-Camera.main.orthographicSize * Camera.main.aspect ,
                PlayerVec.position.x+Camera.main.orthographicSize * Camera.main.aspect ),
                Mathf.Clamp(
                transform.position.y,
                PlayerVec.position.y-Camera.main.orthographicSize ,
                PlayerVec.position.y+Camera.main.orthographicSize ), -10);          //对相机位置做出限制，xy轴分别限制在一个摄像机水平\垂直视体之内；
        
    }
}
