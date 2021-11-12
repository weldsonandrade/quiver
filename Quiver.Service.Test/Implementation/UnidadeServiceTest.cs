using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Quiver.Core.Models;
using Quiver.Data.Interfaces;
using Quiver.Service.Implementation;
using System;
using System.Collections.Generic;

namespace Quiver.Service.Test.Implementation
{
    [TestClass]
    public class UnidadeServiceTest
    {
        #region void Delete(int unidadeToDelete)

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestDeleteComIdIgualAZeroExceptionIsThrown()
        {
            var mock = new Mock<IUnitOfWork>();
            var unidadeService = new UnidadeService(mock.Object);
            unidadeService.Delete(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestDeleteComIdMenorQueZeroExceptionIsThrown()
        {
            var mock = new Mock<IUnitOfWork>();
            var unidadeService = new UnidadeService(mock.Object);
            unidadeService.Delete(-1);
        }

        /// <summary>
        /// Deve mudar o flag excluido para true, e deletar as avaliações com situação NAO_AVALIADO.
        /// </summary>
        [TestMethod]
        public void TestDeleteComIdUmEComDuasAvaliacoesQueNaoForamAvaliadadas()
        {
            // Arrange
            var unidadeId = 1;
            var avaliacoes = new List<Avaliacao>()
            {
                new Avaliacao() { Id = 1, Situacao = SituacaoAvaliacao.NAO_AVALIADO },
                new Avaliacao() { Id = 2, Situacao = SituacaoAvaliacao.NAO_AVALIADO }
            };
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(uow => uow.UnidadeRepository.GetByID(1)).Returns(new Unidade() { Id = unidadeId, Excluido = false, Nome = "Teste Delete" });
            mock.Setup(uow => uow.UnidadeRepository.Update(It.IsAny<Unidade>()));
            mock.Setup(uow => uow.AvaliacaoRepository.GetByUnidadeAndSituacao(unidadeId, SituacaoAvaliacao.NAO_AVALIADO)).Returns(avaliacoes);
            mock.Setup(uow => uow.AvaliacaoRepository.Delete(It.IsAny<Avaliacao>()));
            var unidadeService = new UnidadeService(mock.Object);

            // Act
            unidadeService.Delete(unidadeId);

            // Assert
            mock.Verify(uow => uow.UnidadeRepository.GetByID(1), Times.Once);
            mock.Verify(uow => uow.AvaliacaoRepository.Delete(It.IsAny<Avaliacao>()), Times.Exactly(2));
            mock.Verify(uow => uow.UnidadeRepository.Update(It.IsAny<Unidade>()), Times.Once);
        }

        [TestMethod]
        public void TestDeleteComIdUmIfExcluidoIgualAVerdadeiro()
        {
            // Arrange
            var unidadeId = 1;
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(uow => uow.UnidadeRepository.GetByID(1)).Returns(new Unidade() { Id = unidadeId, Excluido = false, Nome = "Teste Delete" });
            mock.Setup(uow => uow.AvaliacaoRepository.GetByUnidadeAndSituacao(unidadeId, SituacaoAvaliacao.NAO_AVALIADO)).Returns(new List<Avaliacao>());
            var unidadeService = new UnidadeService(mock.Object);

            // Act
            unidadeService.Delete(unidadeId);

            // Assert
            Assert.IsTrue(mock.Object.UnidadeRepository.GetByID(unidadeId).Excluido);
        }

        #endregion
    }
}
