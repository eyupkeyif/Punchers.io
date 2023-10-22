using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    private static ViewManager s_instance;

    [SerializeField] View startinView;
    [SerializeField] View[] views;
    private View currentView;
    private readonly Stack<View> history = new Stack<View>(); 

    public T GetView<T>() where T : View
    {
        for (int i = 0; i < s_instance.views.Length; i++)
        {
            if (s_instance.views[i] is T tView)
            {
                return tView;
            }
            
        }
        return null;
    }

    public void Show<T>(bool remember=true) where T : View
    {
        for (int i = 0; i < s_instance.views.Length; i++)
        {
            if (s_instance.views[i] is T)
            {
                if (s_instance.currentView!=null)
                {
                    if (remember)
                    {
                        s_instance.history.Push(s_instance.currentView);
                    }
                    s_instance.currentView.Hide();
                }

                s_instance.views[i].Show();

                s_instance.currentView = s_instance.views[i];
            }
        }
    }

    public void Show(View view, bool remember = true)
    {

        if (s_instance.currentView!=null)
        {
            if (remember)
            {
                s_instance.history.Push(s_instance.currentView);

            }
            s_instance.currentView.Hide();

        }

        view.Show();
        s_instance.currentView = view;
    }

    public void ShowLast()
    {

        if (s_instance.history.Count!=0)
        {
            Show(s_instance.history.Pop(), false);
        }
    }
    private void Awake() => s_instance = this;

    public void ShowStartView()
    {
        for (int i = 0; i < s_instance.views.Length; i++)
        {
            views[i].Initialize();

            views[i].Hide();
        }

        if (startinView!=null)
        {
            Show(startinView, true);
        }
    }

}
