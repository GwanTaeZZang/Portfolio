using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private List<UIBaseController> UIList = new List<UIBaseController>();
    private Stack<UIBaseController> UIStack = new Stack<UIBaseController>(4);
    private const int LAYER_INCREASE = 10;
    private const int BASE_LAYER_AMOUNT = 100;

    public T Show<T>(string _path) where T : UIBaseController
    {
        int count = UIList.Count;
        for (int i = 0; i < count; i++)
        {
            if (UIList[i].GetType().Equals(typeof(T)))
            {
                Debug.Log("Same Canvas in list");
                if (UIList[i].IsShow())
                {
                    Debug.Log("Canvas is show");
                    return (T)UIList[i];
                }
                PushStack(UIList[i]);
                UIList[i].Show();
                return (T)UIList[i];
            }
        }

        T newCanvas = CreateCanvas<T>(_path);
        PushStack(newCanvas);
        newCanvas.Show();

        return newCanvas;
    }

    public T CreateCanvas<T>(string _path) where T : UIBaseController
    {
        T canvas = Object.Instantiate(Resources.Load<T>(_path));
        UIList.Add(canvas);
        return canvas;
    }

    public void Hide()
    {
        if (UIStack.Count == 0)
        {
            Debug.Log("UIStack count is 0");
            return;
        }

        UIBaseController canvas = UIStack.Pop();
        canvas.Hide();
    }

    public void PushStack(UIBaseController _controller)
    {
        UIStack.Push(_controller);
        _controller.SetCanvasSortingOrder((UIStack.Count * LAYER_INCREASE) + BASE_LAYER_AMOUNT);
    }

}
