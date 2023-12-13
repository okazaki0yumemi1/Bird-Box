namespace Bird_Box.Models;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class ErrorViewModel
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public string? RequestId { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
