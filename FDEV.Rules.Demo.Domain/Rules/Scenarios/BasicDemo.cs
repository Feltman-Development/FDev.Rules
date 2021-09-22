using System;
using System.Collections.Generic;
using System.Dynamic;

namespace FDEV.Rules.Demo.Domain.Rules.Control
{
    public class BasicDemo
    {
        public void Run()
        {
            //Console.WriteLine($"Running {nameof(BasicDemo)}....");
            //List<Workflow> workflows = new List<Workflow>();
            //Workflow workflow = new Workflow();
            //workflow.WorkflowName = "Test Workflow Rule 1";

            //List<Rule> rules = new List<Rule>();

            //Rule rule = new Rule();
            //rule.RuleName = "Test Rule";
            //rule.SuccessEvent = "Count is within tolerance.";
            //rule.ErrorMessage = "Over expected.";
            //rule.RuleExpression = "count < 3";
            //rule.RuleExpressionType = RuleExpressionType.LambdaExpression;

            //rules.Add(rule);

            //workflow.Rules = rules;
            //workflows.Add(workflow);
            //var bre = new RulesEngine.RulesEngine(workflows.ToArray(), null);

            //dynamic datas = new ExpandoObject();
            //datas.count = 1;
            //var inputs = new dynamic[]
            //{
            //        datas
            //};
            //List<RuleResultTree> resultList = bre.ExecuteAllRulesAsync("Test Workflow Rule 1", inputs).Result;

            //bool outcome = false;
            //outcome = resultList.TrueForAll(r => r.IsSuccess);
            //resultList.OnSuccess((eventName) => { Console.WriteLine($"Result '{eventName}' is as expected."); outcome = true; });
            //resultList.OnFail(() => { outcome = false; });
        }
    }
}
