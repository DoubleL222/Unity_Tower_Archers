using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GenericCircularMenu : MonoBehaviour {

    public GenericCircualMenuItem[] menuItems;
    // Use this for initialization
    protected int itemSelected = -1;
    public float firstItemAngle;
    public Image pointer;
    public float pointerDistance;

    private bool following = false;
    private Vector2 nextPointerPosition;

    protected RectTransform pointerTransform;
    protected Action[] ButtonActions;
    public virtual void Start()
    {
        pointerTransform = pointer.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (pointerTransform != null)
        {
            if (pointerTransform.anchoredPosition != nextPointerPosition)
            {
                
                Vector2 nextPos = Vector2.MoveTowards(pointerTransform.anchoredPosition, nextPointerPosition, Time.deltaTime * 1000f);
                //  float mag = nextPos.magnitude;
                if(nextPointerPosition!= Vector2.zero)
                    nextPos = nextPos.normalized * pointerDistance;
                pointerTransform.anchoredPosition = nextPos;
                pointerTransform.localRotation = Quaternion.LookRotation(Vector3.forward, nextPos);
            }
        }
    }

    public void HandleButtonA()
    {
        if (itemSelected >= 0 && itemSelected < ButtonActions.Length)
        {
            if(ButtonActions[itemSelected]!=null)
                ButtonActions[itemSelected]();
        }
    }

    public virtual void SetupButtonActions()
    {
        ButtonActions = new Action[menuItems.Length];
        //TO DO FOR EVERY MENU;
    }

    public void MovePointer(AirConsolePlayer player, Vector2 pos)
    {
        Vector2 newPosition = -pos.normalized * (float)pointerDistance;
        newPosition = new Vector2(-newPosition.x, newPosition.y);
        float angle = Vector2.Angle(Vector2.right, newPosition);
        float angleTan = Mathf.Atan2(newPosition.y, newPosition.x)*Mathf.Rad2Deg;
        if (angleTan < 0)
        {
            angleTan = 360f + angleTan;
        }
        GetMenuItemFromAngle(angleTan);
        //Debug.Log("ANGLE :" + angle + " ; ANGLE ATAN :" + angleTan);
        //pointerTransform.anchoredPosition = newPosition;
        nextPointerPosition = newPosition;
      //  nextPointerRotation = Quaternion.LookRotation(Vector3.forward, newPosition);
        following = true;
    }

    void GetMenuItemFromAngle(float _angle)
    {
        float singleItemAngle = 360f / (float)menuItems.Length;
        itemSelected =  Mathf.FloorToInt(_angle / singleItemAngle);
        menuItems[itemSelected].HighlightButton(true);
    }

    public void ResetPointer()
    {
       // UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        Debug.Log("ResetPointer");
       // itemSelected = -1;
        nextPointerPosition = Vector2.zero;
        //following = false;
    }
    public void HighlightAll(bool _show)
    {
        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i].HighlightButton(_show);
        }
    }
}
