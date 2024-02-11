namespace Calculator.Common.Messaging;

public class Result
{
    public bool IsSuccessful => Errors.Any();
    public List<string> Errors { get; set; } = new();
    public double Value { get; set; }
}
