using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Bird_Box.Models;
using Bird_Box.Utilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NuGet.Common;

namespace Bird_Box.Services
{
    public class RecordingService
    {
        private List<Task<int>> _listeningTasks = new List<Task<int>>();
        //CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource(); 
        private Dictionary<int, CancellationTokenSource> _tokenAndTaskIDs = new Dictionary<int, CancellationTokenSource>();
        public RecordingService()
        {

        }
        public void StartRecording(TimeSpan hours, AnalyzerOptions optionsInput)
        {
            RecordingSchedule scheduleRecording = new RecordingSchedule(hours);
            var tokenSource = new CancellationTokenSource();
            _listeningTasks.Add(scheduleRecording.RecordAndRecognize(optionsInput, tokenSource.Token));
            _tokenAndTaskIDs.Add(_listeningTasks.Last().Id, tokenSource);
        }
        public TaskStatus RecordingStatus (int id)
        {
            if (_listeningTasks is null) return TaskStatus.Faulted;
            else 
            {
                var task = _listeningTasks.Where(x => x.Id == id).FirstOrDefault();
                if (task is null) return TaskStatus.Faulted;
                return task.Status;
            }
        } 
        public bool StopRecording(int id)
        {
            if (_listeningTasks is null) return true;
            var tokenSource = _tokenAndTaskIDs.Where(x => x.Key == id).Select(x => x.Value).FirstOrDefault();
            if (tokenSource is null) return false;
            tokenSource.Cancel();
            _tokenAndTaskIDs.Remove(id);
            tokenSource.Dispose();
            return true;
        }
        public List<int> GetRunningRecordingServices()
        {
            return _tokenAndTaskIDs.Select(x => x.Key).ToList();
        }
    }
}