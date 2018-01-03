using System;

namespace FilmBarcodes.Common.Models
{
    public class TaskResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }
}