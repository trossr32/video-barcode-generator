using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FilmBarcodes.Common;
using FilmBarcodes.Common.Enums;
using FilmBarcodes.Common.Models.BarcodeManager;
using FilmBarcodes.Common.Models.Settings;
using NLog;

namespace BarcodeManager.ViewModels
{
    public class TasksViewModel : ObservableObject
    {
        private Logger _logger;
        private ObservableCollection<TaskProgressViewModel> _tasks;

        public ObservableCollection<TaskProgressViewModel> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                RaisePropertyChangedEvent("Tasks");
            }
        }

        public TasksViewModel(SettingsWrapper settings)
        {
            _logger = LogManager.GetCurrentClassLogger();

            _tasks = new ObservableCollection<TaskProgressViewModel>();
        }

        public async void AddTask(VideoFile videoFile, VideoCollection videoCollection)
        {
            int taskId = Tasks.Any() ? Tasks.Max(t => t.Id) + 1 : 1;

            _logger.Info($"Creating task id:{taskId}, {videoFile.FilenameWithoutExtension}");
            
            Tasks.Add(new TaskProgressViewModel(videoFile, videoCollection, taskId));

            if (Tasks.Any(t => t.State == State.Running))
                return;

            await RunTask();
        }

        private async void TaskComplete()
        {
            if (Tasks.All(t => t.State != State.Queued))
                return;

            await RunTask();
        }

        private async Task RunTask()
        {
            await Tasks.OrderBy(t => t.Id).First(t => t.State == State.Queued).RunTask();

            TaskComplete();
        }
    }
}