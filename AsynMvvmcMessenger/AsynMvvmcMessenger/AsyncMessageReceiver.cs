using GalaSoft.MvvmLight.Messaging;
using System;
using System.Threading.Tasks;

namespace AsyncMvvmMessenger
{
    /// <summary>
    /// AsyncMessage Receiver
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    class AsyncMessageReceiver<TMessage> : IDisposable
        where TMessage : MessageBase
    {
        private IMessenger messenger;
        private Func<TMessage, Task<object>> callback;
        private object token;

        public AsyncMessageReceiver(IMessenger messenger, 
            object token,
            bool receiveDerivedMessagesToo,
            Func<TMessage, Task<object>> callback)
        {
            this.messenger = messenger;
            this.token = token;
            this.callback = callback;
            messenger.Register<AsyncMessage<TMessage>>(
                this,
                token,
                receiveDerivedMessagesToo,
                this.ReceiveAsyncMessage)
;
        }

        private async void ReceiveAsyncMessage(AsyncMessage<TMessage> m)
        {
            try
            {
                var result = await this.callback(m.InnerMessage);
                m.SetResult(result);
            }
            catch (Exception ex)
            {
                m.SetException(ex);
            }
        }

        public void Dispose()
        {
            if (this.callback == null)
            {
                return;
            }
            this.messenger.Unregister<AsyncMessage<TMessage>>(this, this.token, this.ReceiveAsyncMessage);
            this.callback = null;
            this.token = null;
            this.messenger = null;
        }
    }
}
