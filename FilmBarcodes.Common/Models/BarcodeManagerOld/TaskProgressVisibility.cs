namespace FilmBarcodes.Common.Models.BarcodeManagerOld
{
    public class TaskProgressVisibility
    {
        public bool QueuedVisible { get; set; }
        public bool RunningVisible { get; set; }
        public bool SuccessVisible { get; set; }
        public bool FailureVisible { get; set; }
        public bool CancelledVisible { get; set; }
    }
}