using Quiver.DTO.Grupo;
using Quiver.DTO.Questionario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Service.Interfaces
{
    public interface IFormularioService
    {
        /// <summary>
        /// Obtém o formulário.
        /// </summary>
        /// <param name="idQuestionario">id do questionário</param>
        /// <returns></returns>
        QuestionarioDTO GetById(int idQuestionario);

        /// <summary>
        /// Obtém o formulário com excluido == false.
        /// </summary>
        /// <param name="idQuestionario">id do questionário</param>
        /// <returns></returns>
        QuestionarioDTO GetAtivoById(int idQuestionario);

        /// <summary>
        /// Obtém questionários com Excluido == false e filtrado por empresa.
        /// </summary>
        /// <param name="IdEmpresa">Id da empresa</param>
        /// <param name="startWithNome">questionários que começem por este nome</param>
        /// <returns></returns>
        IEnumerable<QuestionarioDTO> GetQuestionariosAtivosByEmpresaAndStartWithNome(int idEmpresa, string startWithNome);

        /// <summary>
        /// Insere um questionário.
        /// </summary>
        /// <param name="questionarioToInsert">QuestionarioDTO</param>
        void Insert(QuestionarioDTO questionarioToInsert);

        /// <summary>
        /// Exclui logicamente o questionário.
        /// </summary>
        /// <param name="questionarioToDelete">Id do questionario a ser deletado.</param>
        void Delete(int questionarioToDelete);

        /// <summary>
        /// Atualiza os formulários de um grupo.
        /// </summary>
        /// <param name="grupoDTO">Grupo e os formulários pertencentes a ele.</param>
        void AtualizarGrupo(GrupoDTO grupoDTO);
    }
}
