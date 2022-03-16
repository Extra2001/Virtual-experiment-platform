/************************************************************************************
    作者：张峻凡
    描述：控制物体选择和移动逻辑
*************************************************************************************/
using HT.Framework;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTG;

public class RTGController : SingletonBehaviourBase<RTGController>
{
    private enum GizmoId
    {
        Move = 1,
        Rotate = 2
    }
    private bool _IsUse;
    /// <summary>
    /// 是否启用RTG
    /// </summary>
    public bool IsUse
    {
        get { return _IsUse; }
        set
        {
            _IsUse = value;
            OnTargetObjectChanged(null);
        }
    }


    private ObjectTransformGizmo _objectMoveGizmo;
    private ObjectTransformGizmo _objectRotationGizmo;
    private GizmoId _workGizmoId;
    private ObjectTransformGizmo _workGizmo;
    public GameObject _targetObject;

    //启用自动化
    protected override bool IsAutomate => true;
    
    // Start is called before the first frame update
    void Start()
    {
        // Create the 4 gizmos
        _objectMoveGizmo = RTGizmosEngine.Get.CreateObjectMoveGizmo();
        _objectRotationGizmo = RTGizmosEngine.Get.CreateObjectRotationGizmo();

        _objectMoveGizmo.Gizmo.SetEnabled(false);
        _objectRotationGizmo.Gizmo.SetEnabled(false);

        _workGizmo = _objectMoveGizmo;
        _workGizmoId = GizmoId.Move;


        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsUse)
        {
            return;
        }
        if (RTInput.WasLeftMouseButtonPressedThisFrame() && RTGizmosEngine.Get.HoveredGizmo == null)
        {
            GameObject pickedObject = PickGameObject();
            if (pickedObject != _targetObject)
            {
                OnTargetObjectChanged(pickedObject);
            }
        }

        if (RTInput.WasKeyPressedThisFrame(KeyCode.Q))
        {
            SetWorkGizmoId(GizmoId.Move);
        }
        else if (RTInput.WasKeyPressedThisFrame(KeyCode.E)) 
        {
            SetWorkGizmoId(GizmoId.Rotate); 
        }

    }

    private GameObject PickGameObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(RTInput.MousePosition);

        // Check if the ray intersects a game object. If it does, return it
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit, float.MaxValue))
        {
            if (rayHit.collider.gameObject.layer == 11)
            {
                return rayHit.collider.gameObject;
            }            
        }            

        // No object is intersected by the ray. Return null.
        return null;
    }

    private void SetWorkGizmoId(GizmoId gizmoId)
    {

        // Start with a clean slate and disable all gizmos
        _objectMoveGizmo.Gizmo.SetEnabled(false);
        _objectRotationGizmo.Gizmo.SetEnabled(false);

        _workGizmoId = gizmoId;
        if (gizmoId == GizmoId.Move) _workGizmo = _objectMoveGizmo;
        else if (gizmoId == GizmoId.Rotate) _workGizmo = _objectRotationGizmo;

        if (_targetObject != null)
        {
            _workGizmo.Gizmo.SetEnabled(true);
            _workGizmo.RefreshPositionAndRotation();
        }
    }

    private void OnTargetObjectChanged(GameObject newTargetObject)
    {
        // Store the new target object
        _targetObject = newTargetObject;

        // Is the target object valid?
        if (_targetObject != null)
        {
            _objectMoveGizmo.SetTargetObject(_targetObject);
            _objectRotationGizmo.SetTargetObject(_targetObject);
            _workGizmo.Gizmo.SetEnabled(true);
        }
        else
        {
            _objectMoveGizmo.Gizmo.SetEnabled(false);
            _objectRotationGizmo.Gizmo.SetEnabled(false);
        }
    }

}
