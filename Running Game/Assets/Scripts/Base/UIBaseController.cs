using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBaseController : MonoBehaviour
{
    [SerializeField] private Canvas myCanvas = null;
    private bool isShow = false;

    private void Awake()
    {
        if(myCanvas == null)
        {
            myCanvas = this.GetComponent<Canvas>();
        }

        Initialized();
    }

    protected abstract void Initialized();

    public void Show()
    {
        myCanvas.enabled = true;
        isShow = true;
    }

    public void Hide()
    {
        myCanvas.enabled = false;
        isShow = false;
    }

    public void SetCanvasSortingOrder(int _amount)
    {
        myCanvas.sortingOrder = _amount;
    }

    public bool IsShow()
    {
        return isShow;
    }
}
