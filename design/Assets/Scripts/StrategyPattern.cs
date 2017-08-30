using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITaxStragegy
{
    double CalculateTex(double income);
}
public class PersonalTaxStragegy : ITaxStragegy
{
    public double CalculateTex(double income)
    {
        return income * 0.12;
    }
}
public class EnterpriseTaxStragegy : ITaxStragegy
{
    public double CalculateTex(double income)
    {
        return (income - 3500) > 0 ? (income - 3500) * 0.045 : 0.0;
    }
}
public class InterestOperation
{
    public ITaxStragegy stragegy;
    public InterestOperation(ITaxStragegy stragegy)
    {
        this.stragegy = stragegy;
    }
    public double GetTax(double income)
    {
        return this.stragegy.CalculateTex(income);
    }
}
public class StrategyPattern : MonoBehaviour {


	// Use this for initialization
	public void init () {
        InterestOperation operation = new InterestOperation(new PersonalTaxStragegy());
        Debug.Log("Person Tax is " + operation.GetTax(5000));
        operation = new InterestOperation(new EnterpriseTaxStragegy());
        Debug.Log("Enterprise tax is " + operation.GetTax(5000));
    }
	
}
