using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{

    public Transform player;

    //物件Layer屬性(通常設成Ground/地板)
    [Header("碰撞判定對象 :")]
    public LayerMask collideMask;

    //相機與地形的碰撞判定位置(通常掛一個於相機後方偏下的父子物件<==直接相機右鍵Create Empty，然後拉至相機後方偏下)
    [Header("碰撞判定物件 :")]
    public Transform cameraCollider;

    //掛Player(通常掛一個Player胸口位置的父子物件)
    [Header("跟隨目標 :")]
    public Transform target;

    //相機變數 pitch=>垂直位置 yaw=>水平位置 zoom=>與目標距離 
    [Header("相機初始位置 :")]
    public float firstPitch = 40f;
    public float firstYaw = 180f;
    public float firstZoom = 10f;
    float pitch;
    float yaw;
    float zoom;

    //pitch跟zoom的上下限
    public Vector2 minMaxPitch = new Vector2(-10f, 80f);
    public Vector2 minMaxZoom = new Vector2(2f, 15f);

    //靈敏度
    public float pitchSensivity = 4f;
    public float yawSensivity = 4f;
    public float zoomSensivity = 4f;

    //旋轉smooth變數，讓相機旋轉平滑
    public float rotationSmoothTime = 0.08f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    //距離smooth變數
    public float zoomSmoothTime = 0.3f;
    float zoomSmoothVelocity;
    float currentZoom;

    //位置smooth變數
    public float positionSmoothTime = 0.12f;
    Vector3 positionSmoothVelocity;
    Vector3 currentPosition;

    //碰撞距離
    float collideZoom;
    //當前是否碰撞
    bool hasCollide = false;
    bool inputLock = false;


    void Start()
    {
        //設定初值
        pitch = firstPitch;
        yaw = firstYaw;
        zoom = firstZoom;
        currentZoom = zoom;
        currentRotation = new Vector3(pitch, yaw);//此z座標沒設定 => 自動為0 (new Vector3(pitch, yaw, 0)的意思)  
    }

    void Update()
    {
        if ((Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0)) && !EventSystem.current.IsPointerOverGameObject())
        {
            inputLock = true;
        }
        else if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(0))
        {
            inputLock = false;
        }
        if (inputLock) return;

        //處理Input
        zoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSensivity;
        zoom = Mathf.Clamp(zoom, minMaxZoom.x, minMaxZoom.y);//Mathf.Clamp => zoom超過min或max 直接設為min或max

        //左右鍵旋轉
        if (Input.GetMouseButton(1) || Input.GetMouseButton(0))
        {
            yaw += Input.GetAxis("Mouse X") * yawSensivity;
            pitch -= Input.GetAxis("Mouse Y") * pitchSensivity;
            pitch = Mathf.Clamp(pitch, minMaxPitch.x, minMaxPitch.y);//Mathf.Clamp => pitch超過min或max 直接設為min或max
        }
        CheckCollide();
        //處理角度
        //SmoothDamp(當前座標,結果座標,速度,smooth時間) => 此行效果為currentRotation慢慢變至結果值
        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        //本物件(相機)transform.旋轉角度 => 給向量值變動transform.rotation
        transform.eulerAngles = currentRotation;

        //處理距離
        
        //判定是否碰撞中決定採用zoom值還是collidezoom值
        if (hasCollide)
        {
            currentZoom = collideZoom;
        }
        else
        {
            currentZoom = Mathf.SmoothDamp(currentZoom, zoom, ref zoomSmoothVelocity, zoomSmoothTime);
        }
        
        //處理位置
        currentPosition = Vector3.SmoothDamp(currentPosition, target.position, ref positionSmoothVelocity, positionSmoothTime);
        //本物件(相機)位置 = 跟隨目標位置 - "相機面對方向"前方1單位向量 * 距離 (後方的意思)
        transform.position = currentPosition - transform.forward * currentZoom;
    }

    //判斷碰撞
    void CheckCollide()
    {
        RaycastHit hit;
        //判定相機有無碰撞地形 => Physics.Linecast(開始位置, 結束位置, 結果放到hit, 只判斷此Layer)
        if (Physics.Linecast(target.position, cameraCollider.position, out hit, collideMask))
        {
            hasCollide = true;

            //碰撞距離 = 跟隨目標 至 碰撞點  
            float distance = Vector3.Distance(target.position, hit.point); //兩點直線距離
            collideZoom = Mathf.Clamp(distance, minMaxZoom.x, minMaxZoom.y);

            //如果使用者縮放讓zoom小於當前碰撞距離 => 離開碰撞狀態
            if (zoom < collideZoom)
            {
                hasCollide = false;
            }
        }
        else
        {
            hasCollide = false;
        }
    }

    //bool IsMouseOverUIWithIgnores()
    //{
    //    PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
    //    pointerEventData.position = Input.mousePosition;

    //    List<RaycastResult> raycastResultList = new List<RaycastResult>();
    //    EventSystem.current.RaycastAll(pointerEventData, raycastResultList);

    //    for (int i = 0; i < raycastResultList.Count; i++)
    //    {
    //        if (raycastResultList[i].gameObject.GetComponent<MouseUIClickThrough>() != null)
    //        {
    //            raycastResultList.RemoveAt(i);
    //            i--;
    //        }
    //    }
    //    return raycastResultList.Count > 0;
    //}
}
