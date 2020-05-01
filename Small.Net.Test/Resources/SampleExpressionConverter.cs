using System;
using System.Linq;
using System.Text;
using Small.Net.Expressions;
using Small.Net.Extensions;

namespace Small.Net.Test.Resources
{
    /// <summary>
    /// Count the expression tree depth 
    /// </summary>
    public class SampleExpressionConverter : ExpressionConverter<int>
    {
        private const string PaddingChar = "\t";
        private int _depth = -1; // avoid first node
        private int padCount = 0;

        public override void Add(ExpressionLambda lambda)
        {
            _depth++;
            Console.WriteLine(
                $"{PaddingChar.Repeat(padCount++)}Lambda Expression with {lambda.ParameterCount:D} parameters and returning {lambda.ReturnType}");
        }

        public override void Add(ExpressionParameter parameter)
        {
            _depth++;
            Console.WriteLine(
                $"{PaddingChar.Repeat(padCount)}Parameter named {parameter.Name} of {parameter.Type}, byRef: {parameter.IsByRef}");
        }

        public override void Add(ExpressionConstant constant)
        {
            _depth++;
            Console.WriteLine($"{PaddingChar.Repeat(padCount)}Constant type {constant.Type}, value: {constant.Value}");
        }

        public override void Add(ExpressionUnary unary)
        {
            _depth++;
            Console.WriteLine($"{PaddingChar.Repeat(padCount)}Unary {unary.UnaryType} of {unary.Type}");
        }

        public override void Add(ExpressionMethodCall methodCall)
        {
            _depth++;
            Console.WriteLine(
                $"{PaddingChar.Repeat(padCount++)}{(methodCall.IsStatic ? "Static" : "Instance")} call of {methodCall.Method.DeclaringType}.{methodCall.Method.Name}");
        }

        public override void BeginMethodArgument()
        {
            Console.WriteLine($"{PaddingChar.Repeat(padCount++)}Method Arguments:");
        }

        public override void EndMethodArgument()
        {
            Console.WriteLine($"{PaddingChar.Repeat(--padCount)}End Method Arguments");
        }

        public override void Add(ExpressionBinary binary)
        {
            _depth++;
            Console.WriteLine($"{PaddingChar.Repeat(padCount)}Binary Operation {binary.BinaryType}");
        }

        public override void BeginLeftPart()
        {
            Console.WriteLine($"{PaddingChar.Repeat(padCount++)}Start Left Part");
        }

        public override void EndLeftPart()
        {
            Console.WriteLine($"{PaddingChar.Repeat(--padCount)}Finish Left Part");
        }

        public override void BeginRightPart()
        {
            Console.WriteLine($"{PaddingChar.Repeat(padCount++)}Start Right Part");
        }

        public override void EndRightPart()
        {
            Console.WriteLine($"{PaddingChar.Repeat(--padCount)}Finish Right Part");
        }

        public override void BeginConditional()
        {
            Console.WriteLine($"{PaddingChar.Repeat(padCount++)}If");
        }

        public override void EndConditional()
        {
            Console.WriteLine($"{PaddingChar.Repeat(--padCount)}End If");
        }

        public override void BeginTruePart()
        {
            Console.WriteLine($"{PaddingChar.Repeat(padCount++)}True:");
        }

        public override void EndTruePart()
        {
            Console.WriteLine($"{PaddingChar.Repeat(--padCount)}End True");
        }

        public override void BeginFalsePart()
        {
            Console.WriteLine($"{PaddingChar.Repeat(padCount++)}False:");
        }

        public override void EndFalsePart()
        {
            Console.WriteLine($"{PaddingChar.Repeat(--padCount)}End False");
        }

        protected override void Initialize()
        {
            _depth = -1;
            padCount = 0;
        }

        protected override int FinishConversion()
        {
            return _depth;
        }
    }
}