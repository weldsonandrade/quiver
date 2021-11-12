using AutoMapper;
using Quiver.Core.Models;
using Quiver.DTO.Alternativa;
using Quiver.DTO.Classificacao;
using Quiver.DTO.Grupo;
using Quiver.DTO.Item;
using Quiver.DTO.Questao;
using Quiver.DTO.Questionario;
using Quiver.DTO.QuestionarioGrupo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Service.Mappers
{
    public class FormularioMapper
    {
        public static Questionario MapQuestionarioDTOToQuestionario(QuestionarioDTO questionarioDTO)
        {
            var questionario = GetConfig().CreateMapper().Map<QuestionarioDTO, Questionario>(questionarioDTO);

            // Garante que todas as questões subjetivas terão itens.
            var questoesSubjetivas = questionario.Questoes.Where(q => q.Tipo == TipoQuestao.Subjetiva);
            foreach(Questao questao in questoesSubjetivas)
            {
                if (questao.Itens == null || questao.Itens.Count == 0)
                {
                    questao.Itens.Add(new Item() { IdQuestao = questao.Id, Questao = questao });
                }
            }
            return questionario;
        }

        public static QuestionarioDTO MapQuestionarioToQuestionarioDTO(Questionario questionario)
        {
            return GetConfig().CreateMapper().Map<Questionario, QuestionarioDTO>(questionario);
        }

        public static List<QuestionarioDTO> MapQuestionarioToQuestionarioDTO(IEnumerable<Questionario> questionarios)
        {
            return GetConfig().CreateMapper().Map<IEnumerable<Questionario>, List<QuestionarioDTO>>(questionarios);
        }

        public static List<QuestionarioGrupoDTO> MapQuestionarioGrupoToQuestionarioGrupoDTO(IEnumerable<QuestionarioGrupo> questionarioGrupos)
        {
            return GetConfig().CreateMapper().Map<IEnumerable<QuestionarioGrupo>, List<QuestionarioGrupoDTO>>(questionarioGrupos);
        }

        private static MapperConfiguration GetConfig()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // Model -> DTO
                cfg.CreateMap<Questionario, QuestionarioDTO>().MaxDepth(1);
                cfg.CreateMap<QuestionarioGrupo, QuestionarioGrupoDTO>()
                    .ForMember(dest => dest.Avaliacoes, opt => opt.Ignore()).MaxDepth(1);
                cfg.CreateMap<Questao, QuestaoDTO>().MaxDepth(1);
                cfg.CreateMap<Item, ItemDTO>().MaxDepth(1);
                cfg.CreateMap<Alternativa, AlternativaDTO>().MaxDepth(1);
                cfg.CreateMap<Classificacao, ClassificacaoDTO>();
                cfg.CreateMap<Grupo, GrupoDTO>().MaxDepth(1);
                // DTO -> Model
                cfg.CreateMap<QuestionarioDTO, Questionario>().MaxDepth(1);
                cfg.CreateMap<QuestionarioGrupoDTO, QuestionarioGrupo>()
                    .ForMember(dest => dest.Avaliacoes, opt => opt.Ignore()).MaxDepth(1);
                cfg.CreateMap<QuestaoDTO, Questao>().MaxDepth(1);
                cfg.CreateMap<ItemDTO, Item>().MaxDepth(1);
                cfg.CreateMap<AlternativaDTO, Alternativa>().MaxDepth(1);
                cfg.CreateMap<ClassificacaoDTO, Classificacao>();
                cfg.CreateMap<GrupoDTO, Grupo>();
            });
            return config;
        }
    }
}
