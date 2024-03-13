using Calculator.API.Events;
using MassTransit;

namespace Calculator.API.StateMachines;

public class CalculateExpressionStateMachine : MassTransitStateMachine<CalculateExpressionState>
{
    public CalculateExpressionStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Initially(When(ExpressionReceivedEvent)
            .Initialize()
            .ConvertExpression()
            .TransitionTo(Converting)
        );

        During(Converting,
            When(ConversionCompletedEvent)
                .CalculateExpression()
                .TransitionTo(Calculating)
            ,
            When(ConversionFailedEvent)
                .ConversionFailed()
                .TransitionTo(Suspended)
            );

        During(Calculating,
            When(CalculationCompletedEvent)
                .Calculated()
                .TransitionTo(Calculated)
            ,
            When(CalculationFailedEvent)
                .CalculationFailed()
                .TransitionTo(Suspended)
            );
    }


    public State Converting { get; }
    public State Calculating { get; }
    public State Calculated { get; }
    public State Suspended { get; }

    public Event<ExpressionReceived> ExpressionReceivedEvent { get; }
    public Event<ConversionCompleted> ConversionCompletedEvent { get; }
    public Event<ConversionFailed> ConversionFailedEvent { get; }
    public Event<CalculationCompleted> CalculationCompletedEvent { get; }
    public Event<CalculationFailed> CalculationFailedEvent { get; }
}

static class CalculateExpressionStateMachineExtensions
{
    public static EventActivityBinder<CalculateExpressionState, ExpressionReceived> Initialize(
        this EventActivityBinder<CalculateExpressionState, ExpressionReceived> binder)
    {
        return binder.Then(context =>
        {
            context.Saga.OperationId = context.Message.OperationId.ToString();
            context.Saga.Expression = context.Message.Expression;
            context.Saga.StartedOn = DateTime.UtcNow;
        });
    }

    public static EventActivityBinder<CalculateExpressionState, ExpressionReceived> ConvertExpression(
        this EventActivityBinder<CalculateExpressionState, ExpressionReceived> binder)
    {
        return binder.PublishAsync(context => context.Init<ConvertExpression>(context.Message));
    }

    public static EventActivityBinder<CalculateExpressionState, ConversionCompleted> CalculateExpression(
        this EventActivityBinder<CalculateExpressionState, ConversionCompleted> binder)
    {
        return binder.PublishAsync(context => context.Init<CalculateExpression>(context.Message));
    }

    public static EventActivityBinder<CalculateExpressionState, ConversionFailed> ConversionFailed(
        this EventActivityBinder<CalculateExpressionState, ConversionFailed> binder)
    {
        return binder.Then(context => { context.Saga.Reason = "Conversion failed"; });
    }

    public static EventActivityBinder<CalculateExpressionState, CalculationCompleted> Calculated(
        this EventActivityBinder<CalculateExpressionState, CalculationCompleted> binder)
    {
        return binder.Then(context =>
        {
            var calculationResult = Convert.ToDouble(context.GetVariable<List<object>>("Results")!.First());
            context.Saga.Result = Math.Round(calculationResult, 3);
            context.Saga.FinishedOn = DateTime.UtcNow;
            context.Saga.Duration = context.Saga.FinishedOn - context.Saga.StartedOn;
        });
    }

    public static EventActivityBinder<CalculateExpressionState, CalculationFailed> CalculationFailed(
        this EventActivityBinder<CalculateExpressionState, CalculationFailed> binder)
    {
        return binder.Then(context =>
        {
            context.Saga.Reason = "Calculation Failed. " + context.Message.ExceptionInfo;
        });
    }
}
