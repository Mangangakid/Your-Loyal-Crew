using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchController : MonoBehaviour
{
    private GameObject[] characters;
    private Character[] charactersScripts;
    public bool moving = false;
    private bool stationaryTouch = false;
    private bool selectionTouch = false; 
    private Vector3 startPoint;
    private Vector3 endPoint;
    public GameObject camFocus;
    public float speed =5f;
    public float spinSpeed = 0.5f;
    public float touchRadius = 0.1f;

    private float height;
    private float width;

    public Vector2 tempCenter;
    public Vector2 rotationStart;
    public Vector2 rotator;

    public CameraFollow camFollow;

    [Header("Camera Constrains")]
    public float maxZ = 13f;
    public float minZ = -23f;
    public float maxX = 13f;
    public float minX = -13f;


    // Start is called before the first frame update
    void Start()
    {
        width = Screen.width / 2.0f;
        height = Screen.height / 2.0f;
        Refresh();
    }

    //Loads characters and their asociated scripts in two arrays 
    private void Refresh()
    {
        characters = GameObject.FindGameObjectsWithTag("Player");
        charactersScripts = new Character[characters.Length];
        for (int i=0;i<characters.Length;i++)
        {
            charactersScripts[i] = characters[i].GetComponent<Character>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        TouchProcess();
        MoveCamera();
    }

    //Camera move and character interaction
    private void TouchProcess()
    {
        TwoFingers();
        if (Input.touchCount == 1)
        {
            if ((Input.GetTouch(0).phase == TouchPhase.Began)&& (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)))
            {
                Vector3 touch = Input.GetTouch(0).position;
                Ray ray = Camera.main.ScreenPointToRay(touch);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                {
                    if (hit.transform.gameObject.tag == "Player")
                    {
                        Character charScript = hit.transform.gameObject.GetComponent<Character>();
                        moving = false;
                        selectionTouch = true;
                        if (charScript.selected)
                        {
                            UnselectAll();
                        }
                        else
                        {
                            UnselectAll();
                            hit.transform.gameObject.GetComponent<Character>().Select(true);                            
                        }
                    }
                    else
                    {
                        moving = true;
                        TouchableCharacters(false);
                        startPoint = hit.point;
                        startPoint.y = 1f;
                        endPoint = startPoint;
                        stationaryTouch = true;
                        selectionTouch = false;
                    }
                }
            }
            else
            {
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    if (moving)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                        {
                            endPoint = hit.point;
                            endPoint.y = 1f;
                        }
                        if (Vector3.Magnitude(endPoint - startPoint) > touchRadius)
                        {
                            stationaryTouch = false;
                        }
                    }
                }
                if ((Input.GetTouch(0).phase == TouchPhase.Ended) && (stationaryTouch) && (!selectionTouch))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                    {
                        for (int i = 0; i < charactersScripts.Length; i++)
                        {
                            if (charactersScripts[i].selected)
                            {
                                charactersScripts[i].MoveTo(hit.point);
                            }
                        }
                    }
                }
                if (((Input.GetTouch(0).phase == TouchPhase.Ended) || (Input.GetTouch(0).phase == TouchPhase.Canceled)) && moving)
                {
                    StopCamera();
                }
            }    
        }
    }

    private void TwoFingers()
    {
        if (Input.touchCount == 2)
        {
            StopCamera();
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                tempCenter = Input.GetTouch(0).position;
            }
            if (Input.GetTouch(1).phase == TouchPhase.Began)
            {
                rotationStart = Input.GetTouch(1).position - tempCenter;
            }
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                if(Vector3.Magnitude(tempCenter - Input.GetTouch(0).position) > touchRadius)
                {
                    float initialDistance = Vector3.Magnitude(rotationStart);
                    camFollow.zoom += (initialDistance - Vector3.Magnitude(Input.GetTouch(1).position- Input.GetTouch(0).position))*Time.deltaTime;
                }
            }
              if (Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                rotator = Input.GetTouch(1).position - tempCenter;
                rotateCam();
                zoom();
            }
        }


        /*    if (Input.touchCount == 2)
        {
            StopCamera();
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                tempCenter = Input.GetTouch(0).position; 
            }
            if (Input.GetTouch(1).phase == TouchPhase.Began)
            {
                rotationStart = Input.GetTouch(1).position-tempCenter;
            }
            if (Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                rotator = Input.GetTouch(1).position-tempCenter;
                rotateCam();
                zoom();
            }
        }
        */
    }

    private void zoom()
    {
        
    }

    private void rotateCam()
    {
        camFollow.angle += (Mathf.Atan2(rotator.y, rotator.x) - Mathf.Atan2(rotationStart.y, rotationStart.x))*spinSpeed*Time.deltaTime;
    }

    private void StopCamera()
    {
        moving = false;
        startPoint = Vector3.zero;
        endPoint = Vector3.zero;
        TouchableCharacters(true);
    }

    private void MoveCamera()
    {
        if (moving)
        {
             Vector3 direction = startPoint - endPoint;
             if (camFocus.transform.position.z >= maxZ && (direction.z>0))
             {
                 direction.z = 0f;
             }
             if (camFocus.transform.position.x >= maxX && (direction.x > 0))
             {
                 direction.x = 0f;
             }
             if (camFocus.transform.position.z <= minZ && (direction.z < 0))
             {
                 direction.z = 0f;
             }
             if (camFocus.transform.position.x <= minX && (direction.x < 0))
             {
                 direction.x = 0f;
             }
            camFocus.transform.position = camFocus.transform.position + direction;
        }
    }

    private void UnselectAll()
    {
        foreach (Character item in charactersScripts)
        {
            item.Select(false);
        }
    }

    private void TouchableCharacters(bool value)
    {
        if (value)
        {
            for (int i = 0; i < characters.Length; i++)
            {
                characters[i].layer = 0;
            }
        }
        else
        {
            for (int i = 0; i < characters.Length; i++)
            {
                characters[i].layer = 2;
            }
        }
    }
}
