using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class facade : MonoBehaviour {

    private course _course;
    private student _student;
    public facade()
    {
        _course = new course();
        _student = new student();
    }
    public bool registerfacade(string mode)
    {
        _course.registercourse(mode);
        _student.notify(mode);
        return true;
    }
}
