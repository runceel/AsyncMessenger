using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GalaSoft.MvvmLight.Messaging;
using System.Threading.Tasks;

namespace AsyncMvvmMessenger
{
    [TestClass]
    public class MessengerExtensionsTest
    {
        [TestMethod]
        public async Task SimpleSendAsyncTest1()
        {
            var messenger = new Messenger();

            var sendedMessage = default(NotificationMessage);
            var token = messenger.RegisterAsyncMessage<NotificationMessage>(m =>
            {
                sendedMessage = m;
                return Task.FromResult<object>(null);
            });

            await messenger.SendAsync(new NotificationMessage("sample"));

            Assert.AreEqual("sample", sendedMessage.Notification);
        }

        [TestMethod]
        public async Task SimpleSendAsyncTest2()
        {
            var messenger = new Messenger();

            var sendedMessage = default(NotificationMessage);
            var token = messenger.RegisterAsyncMessage<NotificationMessage, int>(async m =>
            {
                sendedMessage = m;
                await Task.Delay(1);
                return 100;
            });

            var result = await messenger.SendAsync<NotificationMessage, int>(new NotificationMessage("sample"));

            Assert.AreEqual("sample", sendedMessage.Notification);
            Assert.AreEqual(100, result);
        }


        [TestMethod]
        public async Task DisposeTest()
        {
            var messenger = new Messenger();

            {
                var sendedMessage = default(NotificationMessage);
                var token = messenger.RegisterAsyncMessage<NotificationMessage, int>(async m =>
                {
                    sendedMessage = m;
                    await Task.Delay(1);
                    return 100;
                });

                var result = await messenger.SendAsync<NotificationMessage, int>(new NotificationMessage("sample"));

                Assert.AreEqual("sample", sendedMessage.Notification);
                Assert.AreEqual(100, result);

                // unregister message
                token.Dispose();
            }

            {
                var sendedMessage = default(NotificationMessage);
                var token = messenger.RegisterAsyncMessage<NotificationMessage, int>(async m =>
                {
                    sendedMessage = m;
                    await Task.Delay(1);
                    return 200;
                });
                var result = await messenger.SendAsync<NotificationMessage, int>(new NotificationMessage("sample2"));

                Assert.AreEqual("sample2", sendedMessage.Notification);
                Assert.AreEqual(200, result);
            }
        }

    }
}
