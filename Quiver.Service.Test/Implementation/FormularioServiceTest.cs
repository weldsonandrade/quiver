using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Quiver.Core.Models;
using Quiver.Data;
using Quiver.Data.Interfaces;
using Quiver.Service.Implementation;
using System;
using System.Collections.Generic;

namespace Quiver.Service.Test.Implementation
{
    [TestClass]
    public class FormularioServiceTest
    {
        private IUnitOfWork _uow;

        [TestInitialize]
        public void Initialize()
        {
            var mock = new Mock<IUnitOfWork>();
            var mockQuestionarioRepository = new Mock<IQuestionarioRepository>();

            mock.Setup(uow => uow.QuestionarioRepository).Returns(mockQuestionarioRepository.Object);

            mock.Setup(uow => uow.QuestionarioRepository
                .GetAtivoById(It.Is<int>(i => i == 1 || i == 2)))
                .Returns<int>(r => new Questionario
                {
                    Id = r,
                    Nome = string.Format("TESTE {0}", r),
                    Excluido = false
                });

            this._uow = mock.Object;
        }
        

        #region QuestionarioDTO GetAtivoById

        [TestMethod]
        public void TestPesquisarQuestionarioAtivoExistentePorId()
        {
            var formularioService = new FormularioService(_uow);
            var questionarioDTO = formularioService.GetAtivoById(2);

            Assert.IsNotNull(questionarioDTO);
            Assert.AreEqual(2, questionarioDTO.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestPesquisarQuestionarioAtivoPorIdPassandoIdMenorQueZero()
        {
            var formularioService = new FormularioService(_uow);
            var questionarioDTO = formularioService.GetAtivoById(-1);
        }

        #endregion
    }
}
