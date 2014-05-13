using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncMvvmMessenger
{
    public static class MessengerExtensions
    {
        public static Task SendAsync<TMessage>(this IMessenger self, TMessage message)
            where TMessage : MessageBase
        {
            var asyncMessage = new AsyncMessage<TMessage>(message);
            self.Send(asyncMessage);
            return asyncMessage.Task;
        }

        public static async Task<TResult> SendAsync<TMessage, TResult>(this IMessenger self, TMessage message)
            where TMessage : MessageBase
        {
            var asyncMessage = new AsyncMessage<TMessage>(message);
            self.Send(asyncMessage);
            return (TResult)await asyncMessage.Task;
        }

        public static IDisposable RegisterAsyncMessage<TMessage>(this IMessenger self,
            object token,
            Func<TMessage, Task> callback)
            where TMessage : MessageBase
        {
            return new AsyncMessageReceiver<TMessage>(
                self,
                token,
                false,
                async m =>
                {
                    await callback(m);
                    return null;
                });
        }
        public static IDisposable RegisterAsyncMessage<TMessage>(this IMessenger self,
            Func<TMessage, Task> callback)
            where TMessage : MessageBase
        {
            return new AsyncMessageReceiver<TMessage>(
                self,
                null,
                false,
                async m =>
                {
                    await callback(m);
                    return null;
                });
        }

        public static IDisposable RegisterAsyncMessage<TMessage>(this IMessenger self,
            object token,
            bool receiveDerivedMessagesToo,
            Func<TMessage, Task> callback)
            where TMessage : MessageBase
        {
            return new AsyncMessageReceiver<TMessage>(
                self,
                token,
                receiveDerivedMessagesToo,
                async m =>
                {
                    await callback(m);
                    return null;
                });
        }


        public static IDisposable RegisterAsyncMessage<TMessage, TResult>(this IMessenger self,
            object token,
            Func<TMessage, Task<TResult>> callback)
            where TMessage : MessageBase
        {
            return new AsyncMessageReceiver<TMessage>(
                self,
                token,
                false,
                async m => (TResult)await callback(m));
        }

        public static IDisposable RegisterAsyncMessage<TMessage, TResult>(this IMessenger self,
            Func<TMessage, Task<TResult>> callback)
            where TMessage : MessageBase
        {
            return new AsyncMessageReceiver<TMessage>(
                self,
                null,
                false,
                async m => (TResult)await callback(m));
        }


        public static IDisposable RegisterAsyncMessage<TMessage, TResult>(this IMessenger self,
            object token,
            bool receiveDerivedMessagesToo,
            Func<TMessage, Task<TResult>> callback)
            where TMessage : MessageBase
        {
            return new AsyncMessageReceiver<TMessage>(
                self,
                token,
                receiveDerivedMessagesToo,
                async m => (TResult)await callback(m));
        }
    }
}
