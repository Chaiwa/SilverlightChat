using Chat.Client.EventArguments;
using Chat.Client.Models;
using Chat.Client.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Chat.Client.Test
{
    [TestClass]
    public class PersonalChatViewModelTests
    {
        [TestMethod]
        public void SendMessage_Calls_ModelSendMessage()
        {
            const string userName = "testUser";
            const string message = "test message";
            var mainModelMock = new Mock<IMainChatModel>();
            mainModelMock.SetupGet(x => x.UserName).Returns(userName);
            var mainViewModel = new MainPageViewModel(mainModelMock.Object);
            var chatModelMock = new Mock<IChatModel>();
            var viewModel = new PersonalChatViewModel(chatModelMock.Object, mainViewModel);
            viewModel.SendMessage(message);
            chatModelMock.Verify(x => x.SendMessage(userName, message), Times.Exactly(1), "Model method 'SendMessage' wasn't called.");
        }

        [TestMethod]
        public void NewMessageTest()
        {
            const string userName = "testUser";
            const string message = "test message";
            var mainModelMock = new Mock<IMainChatModel>();
            var mainViewModel = new MainPageViewModel(mainModelMock.Object);
            var chatModelMock = new Mock<IChatModel>();
            var viewModel = new PersonalChatViewModel(chatModelMock.Object, mainViewModel);
            Assert.AreEqual(0, viewModel.Messages.Count, "Incorrect initial messages count.");
            chatModelMock.Raise(x => x.NewMessage += null, new NewMessageEventArgs{Message = message, UserName = userName});
            Assert.AreEqual(1, viewModel.Messages.Count, "Incorrect messages count.");
        }
    }
}
