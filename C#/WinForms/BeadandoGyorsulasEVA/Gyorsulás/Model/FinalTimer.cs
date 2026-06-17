using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gyorsulás.Model
{
    public class FinalTimer : ITimer, IDisposable //Copied from blueprint

    {
        public event EventHandler? Tick;
        private readonly System.Timers.Timer _timer;

        public FinalTimer(Double interval)
        {
            _timer = new System.Timers.Timer(interval);
            _timer.Elapsed += (s, e) =>
            {
                Tick?.Invoke(s, e);
            };
        }
        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }
         public void Dispose()
        {
            _timer.Dispose();
        }
    }
}
