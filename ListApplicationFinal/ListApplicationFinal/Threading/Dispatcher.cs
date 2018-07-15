using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ListApplicationFinal.Threading
{
    internal delegate object DispatcherInitFunc();
    internal delegate object DispatcherFunc(object state);
    internal delegate void DispatcherPostAction(object state);

    internal struct DispatcherInfo
    {
        public DispatcherInitFunc Pre;
        public DispatcherFunc Target;
        public DispatcherPostAction Post;
    }

    public static class Dispatcher
    {
        private static Thread _dispatcherThread = null;
        private static Thread DispatcherThread => _dispatcherThread ?? (_dispatcherThread = new Thread(ThreadRunner){ IsBackground = true });

        private static readonly BlockingCollection<DispatcherInfo> ActionRequests;

        static Dispatcher()
        {
            ActionRequests = new BlockingCollection<DispatcherInfo>();
        }

        private static void ThreadRunner()
        {
            foreach (var invocation in ActionRequests.GetConsumingEnumerable())
            {
                var completionSource = new TaskCompletionSource<bool>();

                Debug.WriteLine("Executing Pre");
                var state = invocation.Pre.Invoke();
                Debug.WriteLine("Executed Pre");

                Device.BeginInvokeOnMainThread(() =>
                {
                    Debug.WriteLine("Executing Target");
                    state = invocation.Target.Invoke(state);
                    completionSource.SetResult(true);
                    Debug.WriteLine("Executed Target");
                });

                completionSource.Task.Wait();
                Debug.WriteLine("Executing Post");
                invocation.Post.Invoke(state);
                Debug.WriteLine("Executed Post");
            }
        }

        public static void BeginInvoke<TState>
            (Func<TState> preAction,
            Func<TState, TState> action,
            Action<TState> postAction)
        {

            var pre = new DispatcherInitFunc(() => preAction());
            var target = new DispatcherFunc(s => action((TState)s));
            var post = new DispatcherPostAction(s => postAction((TState)s));

            PrependInvocation(pre, target, post);
        }
        
        private static void PrependInvocation(DispatcherInitFunc preAction, DispatcherFunc targetAction, DispatcherPostAction postAction)
        {
            if (!DispatcherThread.IsAlive)
                DispatcherThread.Start();

            var info = new DispatcherInfo
            {
                Pre = preAction,
                Target = targetAction,
                Post = postAction
            };

            ActionRequests.Add(info);
        }
    }
}