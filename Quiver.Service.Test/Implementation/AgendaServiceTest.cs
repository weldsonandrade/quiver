using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Quiver.Core.Models;
using Quiver.Data.Interfaces;
using Quiver.DTO.Avaliacao;
using Quiver.Service.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Service.Test.Implementation
{
    [TestClass]
    public class AgendaServiceTest
    {
        private IUnitOfWork _uow;

        [TestInitialize]
        public void Initialize()
        {
            var mock = new Mock<IUnitOfWork>();
            var mockQuestionarioRepository = new Mock<IAvaliacaoRepository>();

            var avaliacao01 = new Avaliacao { Id = 1, Agendada = false, DataCriacao = new DateTime(2017, 08, 04), Situacao = SituacaoAvaliacao.AVALIADO };
            var avaliacao02 = new Avaliacao { Id = 2, Agendada = false, DataCriacao = new DateTime(2017, 07, 04), Situacao = SituacaoAvaliacao.NAO_AVALIADO };

            mock.Setup(uow => uow.AvaliacaoRepository).Returns(mockQuestionarioRepository.Object);
            mock.Setup(uow => uow.AvaliacaoRepository.GetByID(1)).Returns(avaliacao01);
            mock.Setup(uow => uow.AvaliacaoRepository.GetByID(2)).Returns(avaliacao02);

            this._uow = mock.Object;
        }

        #region void UpdateDataProgramada(AvaliacaoDTO avaliacao)

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestAtualizarDataProgramadaDeAvaliacaoJaRealizada()
        {
            var agendaService = new AgendaService(_uow);

            // Assim fará o teste com a avaliacao01.
            AvaliacaoDTO avaliacaoDTO = new AvaliacaoDTO { Id = 1 };

            agendaService.UpdateDataProgramada(avaliacaoDTO);
        }

        #endregion

    }
}
