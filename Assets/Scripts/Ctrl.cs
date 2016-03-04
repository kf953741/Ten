using UnityEngine;
using System.Collections;
using GDGeek;
using System;

public class Ctrl : MonoBehaviour
{
    private FSM fsm_ = new FSM();
    public View _view = null;
    public Model _model = null;
    public void fsmPost(string msg)
    {
        fsm_.post(msg);
        Debug.Log(msg);
    }
    // Use this for initialization
    void Start()
    {
        fsm_.addState("begin", beginState());
        fsm_.addState("play", playState());
        fsm_.addState("input", intputState(), "play");
        fsm_.addState("fall", fallState(), "play");
        fsm_.addState("remove", removeState(), "play");
        fsm_.addState("end", endState());
        fsm_.init("input");
    }

    private State removeState()
    {
        StateWithEventMap state = new StateWithEventMap();
        return state;
    }

    private State fallState()
    {
        StateWithEventMap state = new StateWithEventMap();
        return state;

    }

    private State intputState()
    {
        StateWithEventMap state = new StateWithEventMap();
        state.onStart += delegate
        {
            Debug.Log("int input");
        };
        state.addAction("1", delegate (FSMEvent evt)
             { Debug.Log("i get one~"); });
        state.addAction("2", delegate (FSMEvent evt)
        { Debug.Log("i get 2~"); });
        state.addAction("3", delegate (FSMEvent evt)
        { Debug.Log("i get 3~"); });
        state.addAction("4", delegate (FSMEvent evt)
        { Debug.Log("i get 4~"); });

        return state;
    }

    private State endState()
    {
        StateWithEventMap state = new StateWithEventMap();
        state.addEvent("end", "begin");
        state.onStart += delegate
        {
            _view.end.gameObject.SetActive(true);
        };
        state.onOver += delegate
        {
            _view.end.gameObject.SetActive(false);
        };
        return state;
    }

    private State playState()
    {
        StateWithEventMap state = new StateWithEventMap();
        //         StateWithEventMap state = TaskState.Create(delegate
        //         {
        //             TaskWait tw = new TaskWait();
        //             tw.setAllTime(3f); return tw;
        //         }, fsm_, "end");

        state.onStart += delegate
        {
            _view.play.gameObject.SetActive(true);
            refreshModel2View();
            //             Square square = _view.play.getSquare(0,1);
            //             square.number = 7;
            //             Square square2 = _view.play.getSquare(1, 1);
            //             square2.number = 9;
            //             square.show();
            //             square2.show();
        };
        state.onOver += delegate
        {
            _view.play.gameObject.SetActive(false);
        };
        return state;
    }

    public void refreshModel2View()
    {
        for (int x = 0; x < _model.Width; x++)
        {
            for (int y = 0; y < _model.height; y++)
            {
                Square s = _view.play.getSquare(x, y);
                Cube c = _model.getCube(x, y);
                if (c.isEnable)
                {
                    s.number = c.number;
                    s.show();
                }
                else
                {
                    s.hide();
                }
            }
        }
    }

    private State beginState()
    {
        StateWithEventMap state = new StateWithEventMap();
        state.onStart += delegate
        {
            _view.begin.gameObject.SetActive(true);
        };
        state.onOver += delegate
        {
            _view.begin.gameObject.SetActive(false);
        };
        state.addEvent("begin", "input");
        return state;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
