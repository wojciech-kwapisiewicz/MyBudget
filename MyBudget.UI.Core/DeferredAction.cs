using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;

namespace MyBudget.UI.Core
{
    public class DeferredAction
    {
        Action _action;
        Timer _timer;
        double _delay;

        Dispatcher _originalDispatcher;
        public DeferredAction(Dispatcher dispatcher, Action action, double msDelay)
        {
            _originalDispatcher = dispatcher;
            _action = action;
            _delay = msDelay;
        }

        void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            _originalDispatcher.Invoke(_action);
        }

        public void Go()
        {
            if (_timer != null)
            {
                _timer.Dispose();
            }
            _timer = new Timer(_delay);
            _timer.AutoReset = false;
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
        }
    }
}
