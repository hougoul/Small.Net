using System;
using System.Linq.Expressions;
using Small.Net.Expressions;
using Small.Net.Expressions.Converter;
using Small.Net.Extensions;

namespace Small.Net.Test.Resources
{
    /// <summary>
    /// Count the expression tree depth 
    /// </summary>
    public class SampleExpressionConverter : ExpressionConverter<int>
    {
        private const string PaddingChar = "\t";
        private int _padCount = 0;


        public override LambdaNode<int> BeginLambda()
        {
            Console.WriteLine($"{PaddingChar.Repeat(_padCount++)} Begin Lambda");
            return new SampleLambdaNode();
        }

        public override void EndLambda(LambdaNode<int> lambda)
        {
            Console.WriteLine(
                $"{PaddingChar.Repeat(--_padCount)} End Lambda with {lambda.Parameters.Length} parameters");
        }

        public override ParameterNode<int> BeginParameter()
        {
            return new SampleParameterNode();
        }

        public override void EndParameter(ParameterNode<int> parameter)
        {
            Console.WriteLine(
                $"{PaddingChar.Repeat(_padCount)} Parameter named {parameter.Name} of {parameter.Type}, Is By Ref: {parameter.IsByRef}");
        }

        public override ConstantNode<int> BeginConstant()
        {
            return new SampleConstantNode();
        }

        public override void EndConstant(ConstantNode<int> constant)
        {
            Console.WriteLine($"{PaddingChar.Repeat(_padCount)} Constant {constant.Value} of {constant.Type}");
        }

        public override UnaryNode<int> BeginUnary(ExpressionType unaryType)
        {
            Console.WriteLine($"{PaddingChar.Repeat(_padCount++)} Begin Unary {unaryType}");
            return new SampleUnaryNode();
        }

        public override void EndUnary(UnaryNode<int> unary)
        {
            Console.WriteLine($"{PaddingChar.Repeat(--_padCount)} Unary ");
        }

        public override MethodCallNode<int> BeginMethodCall()
        {
            Console.WriteLine($"{PaddingChar.Repeat(_padCount++)} Begin Method Call");
            return new SampleMethodCallNode();
        }

        public override void EndMethodCall(MethodCallNode<int> methodCall)
        {
            Console.WriteLine(
                $"{PaddingChar.Repeat(--_padCount)} End Method Call {methodCall.Method.Name} (Static {methodCall.IsStatic}, {methodCall.Arguments.Length} Arguments)");
        }

        public override BinaryNode<int> BeginBinary(ExpressionType binaryType)
        {
            Console.WriteLine($"{PaddingChar.Repeat(_padCount++)} Begin Binary {binaryType}");
            return new SampleBinaryNode();
        }

        public override void EndBinary(BinaryNode<int> binary)
        {
            Console.WriteLine($"{PaddingChar.Repeat(--_padCount)} End Binary");
        }

        public override ConditionalNode<int> BeginConditional()
        {
            Console.WriteLine($"{PaddingChar.Repeat(_padCount++)} Begin If");
            return new SampleConditionalNode();
        }

        public override void EndConditional(ConditionalNode<int> condition)
        {
            Console.WriteLine($"{PaddingChar.Repeat(--_padCount)} End If");
        }
    }
}