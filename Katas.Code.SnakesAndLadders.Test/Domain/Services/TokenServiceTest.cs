using System;
using Katas.Code.SnakesAndLadders.Domain.Entities;
using Katas.Code.SnakesAndLadders.Domain.Services;
using NUnit.Framework;
using System.Collections.Generic;
using Moq;

namespace Katas.Code.SnakesAndLadders.Test
{
    public class TokenServiceTest
    {
        private Mock<Token> _token;

        [SetUp]
        public void Setup()
        {
            _token = new Mock<Token>();
        }

        [Test]
        public void Create_Token_Return_Token()
        {
            var token = new Token();
            var response = new TokenService().CreateToken();
            Assert.AreEqual(token.GetType(), response.GetType());
        }

        [Test]
        public void List_Tokens_When_Call_Method()
        {
            var listTokens = new List<Token>();
            var response = new TokenService().GetListTokens();
            Assert.AreEqual(listTokens.GetType(), response.GetType());
        }

        [Test]
        public void Update_Token_Position_When_Player_Rolls_A_Die()
        {
            var newPosition = 5;
            var response = new TokenService().UpdateTokenPosition(_token.Object, newPosition);
            Assert.AreEqual(_token.Object.GetType(), response.GetType());
        }
    }
}
