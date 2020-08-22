using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using VideoBarcodeGenerator.Core;
using VideoBarcodeGenerator.Core.Enums;
using VideoBarcodeGenerator.Core.Helpers;
using VideoBarcodeGenerator.Core.Models.BarcodeGenerator;
using VideoBarcodeGenerator.Core.Models.Settings;

namespace VideoBarcodeGenerator.Wpf.ViewModels
{
    public class TasksViewModel : ObservableObject
    {
        private readonly Logger _logger;
        private readonly SettingsWrapper _settings;
        private ObservableCollection<TaskProgressViewModel> _tasks;
        private bool _taskListVisible;
        private int _cacheClearCounter;

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

        public async void AddTask(BarcodeConfig videoFile, VideoCollection videoCollection)
        {
            int taskId = Tasks.Any() ? Tasks.Max(t => t.Id) + 1 : 1;

            _logger.Info($"Creating task id:{taskId}, {videoCollection.Config.FilenameWithoutExtension}");
            
            Tasks.Add(new TaskProgressViewModel(this, videoFile, videoCollection, _settings, taskId));

            TaskListVisible = true;

            if (Tasks.Count(t => t.State == State.Running) >= _settings.CoreSettings.NumberOfConcurrentTasks)
                return;

            await RunTask();
        }

        private async void TaskComplete()
        {
            _cacheClearCounter++;

            // if the number of running tasks and completed tasks matches the clear cache counter, then don't kick off anything new
            if (_cacheClearCounter + Tasks.Count(t => t.State == State.Running) >= _settings.CoreSettings.NumberOfTasksToRunBetweenCacheClear)
            {
                // if there's nothing running then clear the cache
                if (_cacheClearCounter >= _settings.CoreSettings.NumberOfTasksToRunBetweenCacheClear && Tasks.All(t => t.State != State.Running))
                {
                    Cache.ClearMagickImageCache(_settings, _logger);

                    // kick off number of current tasks again
                    for (int i = 0; i < _settings.CoreSettings.NumberOfConcurrentTasks; i++)
                    {
                        RunTask();
                    }
                }

                return;
            }

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