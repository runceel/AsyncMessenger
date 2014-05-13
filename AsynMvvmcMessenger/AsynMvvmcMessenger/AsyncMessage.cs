using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsynMvvmcMessenger
{
    class AsyncMessage<TMessage> : MessageBase
        where TMessage : MessageBase
    {
        private readonly TaskCompletionSource<object> source = new TaskCompletionSource<object>();
        public TMessage InnerMessage { get; set; }

        public Task<object> Task { get { return this.source.Task; } }

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
