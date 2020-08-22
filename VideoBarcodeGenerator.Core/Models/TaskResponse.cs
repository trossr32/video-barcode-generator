using System;

namespace VideoBarcodeGenerator.Core.Models
{
    public class TaskResponse
    {
        public bool Success { get; set; }
        public bool Cancelled { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }
}