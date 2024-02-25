namespace Calculator.Common.Messaging;

public class Result
{
    public bool IsFailed => Errors.Any();
    public bool IsSuccessful => !IsFailed;
    public List<string> Errors { get; set; } = [];
    public Guid CreatedId { get; set; }
    public double Value { get; set; }
}
