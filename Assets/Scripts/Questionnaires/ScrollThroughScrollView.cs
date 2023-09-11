using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ScrollThroughScrollView : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    public InputActionReference actionScrollUp, actionScrollDown;
    bool scrollUpModeActive, scrollDownModeActive = false;
    float speed = 50;

    Vector2 touchPosition = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        actionScrollUp.action.Enable();
        actionScrollDown.action.Enable();

        actionScrollUp.action.performed += (ctx) =>
        {
            var control = actionScrollUp.action.activeControl;
            if (null == control)
                return;


            scrollUpModeActive = true;
            scrollDownModeActive = false;
        };

        actionScrollUp.action.canceled += (ctx) =>
        {
            var control = actionScrollUp.action.activeControl;
            if (null == control)
                return;

            scrollUpModeActive = false;
        };


        actionScrollDown.action.performed += (ctx) =>
        {
            var control = actionScrollDown.action.activeControl;
            if (null == control)
                return;


            scrollUpModeActive = false;
            scrollDownModeActive = true;
        };

        actionScrollDown.action.canceled += (ctx) =>
        {
            var control = actionScrollDown.action.activeControl;
            if (null == control)
                return;

            scrollDownModeActive = false;
        };

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow) || scrollDownModeActive)
        {
            scrollDown();
        }

        if (Input.GetKey(KeyCode.UpArrow) || scrollUpModeActive)
        {
            scrollUp();
        }
    }
    
    public void scrollDown()
    {
        float contentHeight = scrollRect.content.sizeDelta.y - scrollRect.viewport.rect.height;
        float contentShift = speed * -1 * Time.deltaTime;                           //-1 = downwards
        scrollRect.verticalNormalizedPosition += contentShift / contentHeight;
    }

    public void scrollUp()
    {
        float contentHeight = scrollRect.content.sizeDelta.y - scrollRect.viewport.rect.height;
        float contentShift = speed * 1 * Time.deltaTime;                           //1 = upwards
        scrollRect.verticalNormalizedPosition += contentShift / contentHeight;
    }

    private void OnDisable()
    {
        actionScrollUp.action.Disable();
        actionScrollDown.action.Disable();
    }
}
