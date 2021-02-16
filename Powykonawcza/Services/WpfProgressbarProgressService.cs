using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Controls;

namespace Powykonawcza.Services
{
    public class WpfProgressbarProgressService : IProgressService
    {
        public WpfProgressbarProgressService(ProgressBar progressbar)
        {
            _progressbar = progressbar;
            _subject     = new Subject<double>();
            _subscribtion = _subject
                .ObserveOn(ImmediateScheduler.Instance)
                .Buffer(TimeSpan.FromMilliseconds(100))
                .Where(a => a.Any())
                .Select(a=> a.Last())
                .ObserveOnDispatcher()
                .Subscribe(progressbarValue =>
                {
                    _progressbar.Value = progressbarValue;
                });

            if (_progressbar.Dispatcher.CheckAccess())
                Update();
            else
                _progressbar.Dispatcher.Invoke(Update);
        }

        public void ReportProgress(double percent)
        {
            _subject.OnNext(percent);
        }

        private void Update()
        {
            _progressbar.Minimum = 0;
            _progressbar.Maximum = 100;
            _progressbar.Value   = 0;
        }

        private readonly ProgressBar _progressbar;

        private readonly Subject<double> _subject;
        private IDisposable _subscribtion;
    }
}