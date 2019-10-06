﻿using System;
using System.Collections.Generic;
using System.Linq;
using NCalc2;

namespace GoryMoon.StreamEngineer.Data
{
    public abstract class BaseAction
    {
        public string Message { get; set; }
        public List<Condition> Conditions { get; set; }
        
        public abstract void Execute(Data data);

        protected double GetEventValue(string property, int defaultVal, Data data)
        {
            if (property == null) return defaultVal;
            var expression = new Expression(property) {Parameters = {["event"] = data.Amount}};
            var result = expression.Evaluate();
            if (expression.HasErrors())
            {
                ActionHandler.Logger.WriteLine("Expression error: " + expression.Error);
                return defaultVal;
            }
            return Convert.ToDouble(result);

        }
        
        public bool Test(Data eventData)
        {
            return Conditions.Count <= 0 || Conditions.Select(c => c.Test(eventData)).Aggregate((b, b1) => b || b1);
        }
        
        public override string ToString()
        {
            return $"{nameof(Message)}: {Message}, {nameof(Conditions)}: {string.Join(",", Conditions)}";
        }
    }
}