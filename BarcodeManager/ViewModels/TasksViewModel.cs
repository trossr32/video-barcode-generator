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
        private readonly Logger _logger;
        private readonly SettingsWrapper _settings;
        private ObservableCollection<TaskProgressViewModel> _tasks;
        private bool _taskListVisible;

        public ObservableCollection<TaskProgressViewModel> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
                RaisePropertyChangedEvent("Tasks");
            }
        }

        public bool TaskListVisible
        {
            get => _taskListVisible;
            set
            {
                _taskListVisible = value;
                RaisePropertyChangedEvent("TaskListVisible");
            }
        }

        public TasksViewModel(SettingsWrapper settings)
        {
            _logger = LogManager.GetCurrentClassLogger();

            _settings = settings;

            _tasks = new ObservableCollection<TaskProgressViewModel>();
        }

        public async void AddTask(VideoFile videoFile, VideoCollection videoCollection)
        {
            int taskId = Tasks.Any() ? Tasks.Max(t => t.Id) + 1 : 1;

            _logger.Info($"Creating task id:{taskId}, {videoFile.VideoFilenameWithoutExtension}");
            
            Tasks.Add(new TaskProgressViewModel(this, videoFile, videoCollection, taskId));

            TaskListVisible = true;

            if (Tasks.Count(t => t.State == State.Running) >= _settings.BarcodeManager.NumberOfConcurrentTasks)
                return;

            await RunTask();
        }

        private async void TaskComplete()
        {
            if (Tasks.Any(t => t.State == State.Queued))
                await RunTask();
        }

        private async Task RunTask()
        {
            await Tasks.OrderBy(t => t.Id).First(t => t.State == State.Queued).RunTask();

            TaskComplete();
        }

        internal void DeleteTask(TaskProgressViewModel task)
        {
            Tasks.Remove(task);

            TaskListVisible = Tasks.Any();

            RaisePropertyChangedEvent("Tasks");
        }
    }
}