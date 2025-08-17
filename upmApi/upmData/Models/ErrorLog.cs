using System;
using System.Collections.Generic;

namespace upmData.Models;

public partial class ErrorLog
{
    public int Id { get; set; }

    public DateTime Timestamp { get; set; }

    public string Level { get; set; } = null!;

    public string Message { get; set; } = null!;

    public string ExceptionType { get; set; } = null!;

    public string StackTrace { get; set; } = null!;

    public string Source { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string RequestId { get; set; } = null!;

    public string AdditionalData { get; set; } = null!;

    public string MachineName { get; set; } = null!;

    public string ApplicationName { get; set; } = null!;
}
