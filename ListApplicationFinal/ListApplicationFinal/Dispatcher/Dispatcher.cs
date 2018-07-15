using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ListApplicationFinal.Dispatcher
{
    public static class Dispatcher
    {
        private static Thread _dispatcherThread = null;
        private static Thread DispatcherThread => _dispatcherThread ?? (_dispatcherThread = new Thread(ThreadRunner){ IsBackground = true });

        private static readonly BlockingCollection<DispatcherSubscribtion> QueueActions;

        private static void ThreadRunner()
        {
            TaskCompletionSource<bool> completionSource = null;

            foreach (var dispatcherSubscribtion in QueueActions.GetConsumingEnumerable())
            {
                var state = dispatcherSubscribtion.EarlyAction.Invoke(dispatcherSubscribtion.State);
                completionSource = new TaskCompletionSource<bool>();

                Device.BeginInvokeOnMainThread(() =>
                {
                    dispatcherSubscribtion.GuiAction.Invoke(state);
                    completionSource.SetResult(true);
                });

                completionSource.Task.Wait();
                dispatcherSubscribtion.CallBack.Invoke(state);
            }
        }

        static Dispatcher()
        {
            QueueActions = new BlockingCollection<DispatcherSubscribtion>();            
        }

        public static void RunOnGui(Action<object> action, Func<object, object> earlyAction = null, Action<object> callBack = null, object state = null)
        {
            if(!DispatcherThread.IsAlive)
                DispatcherThread.Start();

            var subscribtion = new DispatcherSubscribtion()
            {
                State = state,
                EarlyAction = earlyAction ?? (s => null),
                GuiAction = action,
                CallBack = callBack ?? (s => {})
            };

            QueueActions.Add(subscribtion);
        }

    }

    internal struct DispatcherSubscribtion
    {
        public object State { get; set; }
        public Func<object, object> EarlyAction { get; set; }
        public Action<object> GuiAction { get; set; }
        public Action<object> CallBack { get; set; }
    }
}