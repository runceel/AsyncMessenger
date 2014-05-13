using GalaSoft.MvvmLight.Messaging;
using System;
using System.Threading.Tasks;

namespace AsyncMvvmMessenger
{
    /// <summary>
    /// awaitable message
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    class AsyncMessage<TMessage> : MessageBase
        where TMessage : MessageBase
    {
        private readonly TaskCompletionSource<object> source = new TaskCompletionSource<object>();
        public TMessage InnerMessage { get; private set; }

        public Task<object> Task { get { return this.source.Task; } }

        public AsyncMessage(TMessage innerMessage)
        {
            this.InnerMessage = innerMessage;
        }

        public void SetResult(object result)
        {
            this.source.SetResult(result);
        }

        public void SetException(Exception ex)
        {
            this.source.SetException(ex);
        }
    }
}
