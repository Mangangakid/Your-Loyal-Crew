using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TouchController : MonoBehaviour
{
    
    public Transform ScrollablePlane;
    public float TouchTolerance = 0.5f;
    public float BorderLimit = 1f;
    public float RotationSpeed = 0.7f;
    public float ZoomSpeed = 1f;
    public bool Rotate=true;

    private Vector2 lastTouch0;
    private Vector2 lastTouch1;
    private Vector2 newTouch0;
    private Vector2 newTouch1;

    private Vector3 newTouchDrag;
    private Vector3 lastTouchDrag;
    private Vector3 direction;

    private bool StaticTouch=false;
    private bool moving = false;
    private int zoomingAndRotating0 = 0;
    private int zoomingAndRotating1 = 0;
    private Transform _focus;
    private GameManager _gameManager;
    private CameraFollow _camFollow;

    private enum TouchState
    {
        Idle, //No touch or more than two touches +++Goes to: Select, Order, Swipe, ZoomAndRotate+++
        Swipe,//One Touch and drag over scrollable plane +++Goes to: Idle, ZoomAndRotate+++
        ZoomAndRotate //Two finger moving touches +++Goes to: Idle+++
    }

    TouchState CurrentState = TouchState.Idle;

    #region Finite State Machine Setup
    private void Start()
    {
        _gameManager = GameManager.instance;
        _camFollow = gameObject.GetComponent<CameraFollow>();
        _focus = _camFollow.target;
        StartCoroutine("FSM");
    }

    IEnumerator FSM() //This is the state machine
    {
        while (true)
        {
            yield return StartCoroutine(CurrentState.ToString());
        }
    }

    private void ChangeState(TouchState NextState) //This is the changer of states
    {
        CurrentState = NextState;
    }
    #endregion

    #region Idle

    IEnumerator Idle() //This loops in the Idle state
    {
        while(CurrentState == TouchState.Idle)
        {
            switch (Input.touchCount)
            {
                case 1:
                    lastTouch0 = newTouch0;
                    if (Input.GetTouch(0).phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    {
                        newTouch0 = Camera.main.ScreenToViewportPoint(Input.GetTouch(0).position);
                        StaticTouch = true;
                    }
                    if (Input.GetTouch(0).phase == TouchPhase.Moved&& !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    {
                        if (!NearTouches(Camera.main.ScreenToViewportPoint(Input.GetTouch(0).position), lastTouch0))
                        {
                            StaticTouch = false;
                            ChangeState(TouchState.Swipe);
                        }                        
                    }
                    if (Input.GetTouch(0).phase == TouchPhase.Ended && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    {
                        if (NearTouches(Camera.main.ScreenToViewportPoint(Input.GetTouch(0).position), lastTouch0)||StaticTouch)
                        {
                            OrderOrSelection();
                        }
                        StaticTouch = false;
                    }
                    if (Input.GetTouch(0).phase == TouchPhase.Canceled)
                    {
                        newTouch0 = Vector2.zero;
                        lastTouch0 = Vector2.zero;
                        StaticTouch = false;
                    }
                    break;
                case 2:
                    ChangeState(TouchState.ZoomAndRotate);
                    break;
                default:
                    break;
            }
            yield return 0; 
        }
    }

    //Sorts the touch as an order or a selection
    private void OrderOrSelection()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;
            if (objectHit.tag == "Player") //What happens if I touch a player Character
            {
                Character CharacterHit = objectHit.GetComponent<Character>();
                if (CharacterHit.selected)
                {
                    _gameManager.UnselectAll();
                }
                else
                {
                    _gameManager.UnselectAll();
                    CharacterHit.Select(true);
                }
            }
            else
            {
                if (_gameManager.SelectedCharacter != 0) //If a character is already selected
                {
                    if (objectHit.tag == "Enemy")
                    {
                        //What happens if I touch an Enemy
                    }
                    else
                    {
                        if (objectHit.tag == "Room")
                        {
                            objectHit.parent.GetComponent<Room>().OnRoomTouched();
                        }
                        else
                        {
                            if (objectHit.tag == "Hallway")
                            {
                                if (_gameManager.Characters[_gameManager.SelectedCharacter - 1].Job != 0)
                                {
                                    _gameManager.Characters[_gameManager.SelectedCharacter - 1].GoIdle();
                                    _gameManager.UnselectAll();
                                }
                                else
                                {
                                    _gameManager.UnselectAll();
                                    Debug.Log("No estoy trabajando");
                                }
                            }
                        }
                    }
                }
                else //If a character is not selected
                {
                    if (objectHit.tag == "Room") //selecting a room
                    {
                        objectHit.parent.GetComponent<Room>().OnRoomSelected();
                    }
                    else
                    {
                        _gameManager.UnselectAll();
                    }
                }
            }
        }
    }

    //Moves the selected Character to a target destination
    public void WalkTo(Vector3 target)
    {
        target = new Vector3(target.x, 1f, target.z);
        _gameManager.Characters[_gameManager.SelectedCharacter - 1].MoveTo(target);
    }

    //Returns true if two touches are near from each other
    private bool NearTouches(Vector2 aux1, Vector2 aux2)
    {
        return (Vector2.Distance(aux1, aux2) < TouchTolerance);
    }
    #endregion

    #region Swipe

    IEnumerator Swipe() //This loops in Swipe state
    {
        switch (Input.touchCount)
        {
            case 1:
                switch (Input.GetTouch(0).phase)
                {
                    case TouchPhase.Moved:
                        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                        LayerMask mask = LayerMask.GetMask("Floor");
                        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity,mask))
                        {
                            if (moving)
                            {
                                lastTouchDrag = hit.point;
                                direction = newTouchDrag - lastTouchDrag;
                                direction = new Vector3(direction.x, 0f, direction.z);
                                Move(_focus, direction);
                            }
                            else
                            {
                                newTouchDrag = hit.point;
                            }
                        }
                        moving = true;
                        break;
                    case TouchPhase.Stationary:
                        break;
                    default:
                        ChangeState(TouchState.Idle);
                        moving = false;
                        break;
                }
                break;
            case 2:
                ChangeState(TouchState.ZoomAndRotate);
                moving = false;
                break;
            default:
                ChangeState(TouchState.Idle);
                moving = false;
                break;
        }
        yield return 0;     
    }

    private void Move(Transform focus,Vector3 dir)
    {
        Vector3 newPosition = focus.position + dir;
        float xedge = ScrollablePlane.transform.lossyScale.x * 5f;
        float zedge = ScrollablePlane.transform.lossyScale.z * 5f;
        if (newPosition.x > xedge-BorderLimit)
            newPosition.x = xedge-BorderLimit;
        if (newPosition.x < -xedge+BorderLimit)
            newPosition.x = -xedge+BorderLimit;
        if (newPosition.z > zedge-BorderLimit)
            newPosition.z = zedge-BorderLimit;
        if (newPosition.z < -zedge+BorderLimit)
            newPosition.z = -zedge+BorderLimit;
        focus.position = newPosition;
    }

    #endregion

    #region Zoom And Rotate

    IEnumerator ZoomAndRotate()
    {
        StartZoomingAndRotating();
        while (CurrentState == TouchState.ZoomAndRotate)
        {
            switch (Input.touchCount)
            {
                case 1:
                    ChangeState(TouchState.Idle);
                    StartZoomingAndRotating();
                    break;
                case 2:
                    if (Input.GetTouch(0).phase == TouchPhase.Moved)
                    {
                        lastTouch0 = Camera.main.ScreenToViewportPoint(Input.GetTouch(0).position);
                        zoomingAndRotating0++;
                    }
                    if (Input.GetTouch(1).phase == TouchPhase.Moved)
                    {
                        lastTouch1 = Camera.main.ScreenToViewportPoint(Input.GetTouch(1).position);
                        zoomingAndRotating1++;
                    }
                    if (zoomingAndRotating0>1&&zoomingAndRotating1>1)
                    {
                        Zooming();
                        if (Rotate)
                        {
                            Rotating();
                        }
                    }
                    newTouch0 = lastTouch0;
                    newTouch1 = lastTouch1;
                    break;
                default:
                    ChangeState(TouchState.Idle);
                    StartZoomingAndRotating();
                    break;
            }
            yield return 0;
        }
    }

    private void StartZoomingAndRotating()
    {
        zoomingAndRotating0 = 0;
        zoomingAndRotating1 = 0;
    }

    private void Rotating()
    {
        Vector2 C1 = (newTouch0 + newTouch1) / 2f;
        Vector2 C2 = (lastTouch0 + lastTouch1) / 2f;
        Vector2 V1 = newTouch1 - C1;
        if (V1 == Vector2.zero)
        {
            V1 = C1 - newTouch0;
        }
        Vector2 V2 = lastTouch1 - C2;
        if (V2 == Vector2.zero)
        {
            V2 = C2 - lastTouch0;
        }
        float alpha = Mathf.Atan2(V1.y, V1.x);
        float beta = Mathf.Atan2(V2.y, V2.x);
        float gamma = beta - alpha;
        _camFollow.angle = _camFollow.angle - gamma;
    }

        private void Zooming()
    {
        float zoom = Vector3.Magnitude(lastTouch0 - lastTouch1) - Vector3.Magnitude(newTouch0 - newTouch1);
        if (_camFollow.zoom - zoom > 5)
        {
            _camFollow.zoom = 5f;
        }
        else
        {
            if (_camFollow.zoom - zoom < 1)
            {
                _camFollow.zoom = 1f;
            }
            else
            {
                _camFollow.zoom -= zoom*ZoomSpeed;
            }
        }
    }
    #endregion
}
