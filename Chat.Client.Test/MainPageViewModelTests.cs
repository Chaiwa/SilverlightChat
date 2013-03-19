using System;
using System.Linq;
using Chat.Client.EventArguments;
using Chat.Client.Models;
using Chat.Client.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Chat.Client.Test
{
    [TestClass]
    public class MainPageViewModelTests
    {
        #region Method tests

        [TestMethod]
        public void Open_Calls_ModelOpenChat()
        {
            var modelMock = new Mock<IMainChatModel>();
            var viewModel = new MainPageViewModel(modelMock.Object);
            viewModel.Open();
            modelMock.Verify(x => x.OpenChat(), Times.Exactly(1), "Model method 'OpenChat' wasn't called.");
        }

        [TestMethod]
        public void Close_Calls_ModelCloseChat()
        {
            var modelMock = new Mock<IMainChatModel>();
            var viewModel = new MainPageViewModel(modelMock.Object);
            viewModel.Close(null);
            modelMock.Verify(x => x.CloseChat(), Times.Exactly(1), "Model method 'CloseChat' wasn't called.");
        }

        [TestMethod]
        public void GetPersonalChat_Calls_ModelGetNewPersonalChat()
        {
            const string userName = "test";
            var chatModelMock = new Mock<IChatModel>();
            var modelMock = new Mock<IMainChatModel>();
            modelMock.Setup(x => x.GetNewPersonalChat(userName)).Returns(chatModelMock.Object);
            var viewModel = new MainPageViewModel(modelMock.Object);
            viewModel.GetPersonalChat(userName);
            modelMock.Verify(x => x.GetNewPersonalChat(userName), Times.Exactly(1), "Model method 'GetNewPersonalChat' wasn't called.");
        }

        [TestMethod]
        public void ClosePersonalChat_Fires_ModelClosePersonal()
        {
            const string userName = "test";
            var modelMock = new Mock<IMainChatModel>();
            var viewModel = new MainPageViewModel(modelMock.Object);
            // don't know how to verify event firing
            viewModel.ClosePersonal += (sender, args) => Assert.AreEqual(userName, args.UserName, "User name mismatch.");
            viewModel.ClosePersonalChat(userName);
        }

        #endregion

        #region Event tests

        [TestMethod]
        public void RefreshUsersTest()
        {
            const string userName = "test";
            var modelMock = new Mock<IMainChatModel>();
            var viewModel = new MainPageViewModel(modelMock.Object);
            Assert.AreEqual(0, viewModel.Users.Count, "Incorrect initial users count.");
            modelMock.Raise(x => x.RefreshUsers += null, new RefreshUsersEventArgs{Users = new[] {userName}});
            Assert.AreEqual(1, viewModel.Users.Count, "Incorrect initial users count.");
            Assert.AreEqual(userName, viewModel.Users.First());
        }

        [TestMethod]
        public void NewPersonalChatTest()
        {
            const string userName = "test";
            var chatModelMock = new Mock<IChatModel>();
            var modelMock = new Mock<IMainChatModel>();
            modelMock.Setup(x => x.GetNewPersonalChat(userName)).Returns(chatModelMock.Object);
            var viewModel = new MainPageViewModel(modelMock.Object);
            modelMock.Raise(x => x.NewPersonalChat += null, new NewMessageEventArgs{UserName = userName});
            modelMock.Verify(x => x.GetNewPersonalChat(userName), Times.Exactly(1), "Model method 'GetNewPersonalChat' wasn't called.");
        }

        #endregion
    }
}