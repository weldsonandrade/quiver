using AutoMapper;
using Quiver.Core.Models;
using Quiver.DTO.AvaliacaoQuestionarioGrupo;
using Quiver.DTO.Classificacao;
using Quiver.DTO.Grupo;
using Quiver.DTO.Questionario;
using Quiver.DTO.QuestionarioGrupo;
using Quiver.DTO.Resposta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Service.Mappers
{
    public class GrupoMapper
    {
        public static GrupoDTO MapGrupoToGrupoDTO(Grupo grupo)
        {
            return GetConfig().CreateMapper().Map<Grupo, GrupoDTO>(grupo);
        }

        public static List<GrupoDTO> MapGrupoToGrupoDTO(IEnumerable<Grupo> grupos)
        {
            return GetConfig().CreateMapper().Map<IEnumerable<Grupo>, List<GrupoDTO>>(grupos);
        }

        public static Classificacao MapClassificacaoDTOToClassificacao(ClassificacaoDTO classificacao)
        {
            return GetConfig().CreateMapper().Map<ClassificacaoDTO, Classificacao>(classificacao);
        }

        public static List<Classificacao> MapClassificacaoDTOToClassificacao(List<ClassificacaoDTO> classificacao)
        {
            return GetConfig().CreateMapper().Map<List<ClassificacaoDTO>, List<Classificacao>>(classificacao);
        }

        private static MapperConfiguration GetConfig()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Grupo, GrupoDTO>().MaxDepth(1);
                cfg.CreateMap<Classificacao, ClassificacaoDTO>();
                cfg.CreateMap<QuestionarioGrupo, QuestionarioGrupoDTO>()
                    .ForMember(dest => dest.Avaliacoes, opt => opt.Ignore());
                cfg.CreateMap<Questionario, QuestionarioDTO>()
                    .ForMember(dest => dest.Questoes, opt => opt.Ignore()).MaxDepth(1);
                cfg.CreateMap<ClassificacaoDTO, Classificacao>();
            });
            return config;
        }
    }
}
