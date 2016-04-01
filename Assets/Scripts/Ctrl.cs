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
    }
    // Use this for initialization
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

    private State playState()
    {
        StateWithEventMap state = new StateWithEventMap();

        state.onStart += delegate
        {
            _view.play.gameObject.SetActive(true);
            refreshModel2View();

        };
        state.onOver += delegate
        {
            _view.play.gameObject.SetActive(false);
        };
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

    private void input(int x, int number)
    {
        Cube c = _model.getCube(1, 0);
        c.isEnable = false;
        c = _model.getCube(x, 0);

        c.number = number;
        c.isEnable = true;
        refreshModel2View();
    }
    private State intputState()
    {
        StateWithEventMap state = new StateWithEventMap();
        int number = 0;
        state.onStart += delegate
        {
            number = UnityEngine.Random.Range(3, 8);
            Cube c = _model.getCube(1, 0);
            c.isEnable = true;
            c.number = number;
            refreshModel2View();
        };
        state.addAction("1", delegate (FSMEvent evt)
        {

            input(0, number);
            return "fall";
        });
        state.addAction("2", delegate (FSMEvent evt)
        {

            input(1, number);
            return "fall";
        });
        state.addAction("3", delegate (FSMEvent evt)
        {

            input(2, number);
            return "fall";
        });
        state.addAction("4", delegate (FSMEvent evt)
        {
            input(3, number);
            return "fall";
        });

        return state;
    }

    private Task doFallTask()
    {
        Debug.Log("fasdfdsaf");
        TaskSet ts = new TaskSet();
        //ts.push
        for (int x = 0; x < _model.Width; x++)
        {
            for (int y = _model.height - 1; y >= 0; --y)
            {
                Cube c = _model.getCube(x, y);
                Cube end = null;
                int endY = 0;
                if (c.isEnable)
                {
                    for (int n = y + 1; n < _model.height; ++n)
                    {
                        Cube fall = _model.getCube(x, n);
                        if (fall == null || fall.isEnable == true)
                        {
                            break;
                        }
                        else
                        {
                            end = fall;
                            endY = n;
                        }
                    }
                    if (end != null)
                    {
                        end.number = c.number;
                      
                        end.isEnable = true;
                        c.isEnable = false;
                        ts.push(_view.play.moveTask(c.number, new Vector2(x, y), new Vector2(x, endY)));

                    }
                }
            }
        }
        TaskManager.PushBack(ts, delegate () { refreshModel2View(); });

        return ts;
    }

    private State fallState()
    {
        StateWithEventMap state = TaskState.Create(delegate
        {
            Task fall = doFallTask();
            return fall;
        }, fsm_, "remove");
        return state;

    }

    bool checkAndRemove()
    {
        bool s = false;
        for (int x = 0; x < _model.Width; x++)
        {
            for (int y = 0; y < _model.height; y++)
            {
                Cube c = _model.getCube(x, y);
                if (c.isEnable == true)
                {
                    Cube up = _model.getCube(x, y - 1);
                    if (up != null && up.isEnable == true && up.number + c.number == 10)
                    {
                        c.isEnable = false;
                        up.isEnable = false;
                        s = true;
                        break;
                    }
                    Cube down = _model.getCube(x, y + 1);
                    if (down != null && down.isEnable == true && down.number + c.number == 10)
                    {
                        c.isEnable = false;
                        down.isEnable = false;
                        s = true;
                        break;
                    }
                    Cube left = _model.getCube(x - 1, y);
                    if (left != null && left.isEnable == true && left.number + c.number == 10)
                    {
                        c.isEnable = false;
                        left.isEnable = false;
                        s = true;
                        break;
                    }
                    Cube right = _model.getCube(x + 1, y);
                    if (right != null && right.isEnable == true && right.number + c.number == 10)
                    {
                        c.isEnable = false;
                        right.isEnable = false;
                        s = true;
                        break;
                    }
                }
            }
        }
        refreshModel2View();
        return s;
    }





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
        bool s = false;
        StateWithEventMap state = TaskState.Create(delegate
        {
            Task task = new Task();
            TaskManager.PushFront(task, delegate
            {

                s = checkAndRemove();
            });
            return task;
        }, fsm_, delegate
        {
            if (s)
            {
                return "fall";
            }
            else
            {
                return "input";
            }
        });
        return state;
    }


}
