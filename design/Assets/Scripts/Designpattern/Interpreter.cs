using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//解释器模式
public abstract class Interpreter
{
    public abstract void Interpret();
}
public class TerminalInterpreter : Interpreter
{
    public override void Interpret()
    {
        Debug.Log("TerminalInterpreter interpret");
    }
}
public class NonterminalInterpreter : Interpreter
{
    public override void Interpret()
    {
        Debug.Log("NonterminalInterpreter interpret");
    }
}
