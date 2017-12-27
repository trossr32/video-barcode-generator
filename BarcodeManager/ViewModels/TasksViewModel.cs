using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FilmBarcodes.Common.Enums;
using FilmBarcodes.Common.Models.BarcodeManager;

namespace BarcodeManager.ViewModels
{
    public class TasksViewModel : ObservableObject
    {
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

        public TasksViewModel()
        {
            _tasks = new ObservableCollection<TaskProgressViewModel>();
        }
        
        private void CreateData()
        {
            Tasks = new ObservableCollection<TaskProgressViewModel>
            {
                new TaskProgressViewModel
                {
                    State = State.Queued
                },
                new TaskProgressViewModel
                {
                    State = State.Running
                },
                new TaskProgressViewModel
                {
                    State = State.Success
                },
                new TaskProgressViewModel
                {
                    State = State.Failure
                }
            };
        }

        public async void AddTask(VideoFile videoFile)
        {
            int taskId = Tasks.Any() ? Tasks.Max(t => t.Id) + 1 : 1;
            
            Tasks.Add(new TaskProgressViewModel(videoFile, taskId));

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