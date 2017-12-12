using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class Builder
{
    public abstract void BuildPartCPU();
    public abstract Computer GetComputer();
}
class ConcreteBuilderA_Builder : Builder
{
    Computer computer = new Computer();
    public override void BuildPartCPU()
    {
        //Debug.Log("rextest ConcreteBuilderA_Builder BuildPartCpu !!! ");
        computer.Add("CPU1");
    }
    public override Computer GetComputer()
    {
        return computer;
    }
}
class ConcreteBuilderB_Builder : Builder
{
    Computer computer = new Computer();
    public override void BuildPartCPU()
    {
        //Debug.Log("rextest ConcreteBuilderB_Builder BuildPartCpu !!! ");
        computer.Add("CPU2");
    }
    public override Computer GetComputer()
    {
        return computer;
    }
}

public class Computer
{
    List<String> mPartList = new List<string>();
    public void Add(String strPartName)
    {
        mPartList.Add(strPartName);
    }
    public void show()
    {
        Debug.Log("电脑开始在组装.......");
        foreach (string part in mPartList)
        {
            Debug.Log("组件" + part + "已装好");
        }
        Debug.Log("电脑组装好了");
    }
}
public class Director
{
    public void Construct(Builder builder)
    {
        builder.BuildPartCPU();
    }
}
