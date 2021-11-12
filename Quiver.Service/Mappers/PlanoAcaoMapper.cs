using AutoMapper;
using Quiver.Core.Models;
using Quiver.DTO.PlanoAcao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Service.Mappers
{
    public class PlanoAcaoMapper
    {
        public static PlanoAcaoDTO MapPlanoAcaoToPlanoAcaoDTO(PlanoAcao planoAcao)
        {
            PlanoAcaoDTO planoAcaoDTO = GetConfig().CreateMapper().Map<PlanoAcao, PlanoAcaoDTO>(planoAcao);

            planoAcaoDTO.Questao = new QuestaoDTO()
            {
                Id = planoAcao.RespostaItem.Item.IdQuestao,
                Descricao = planoAcao.RespostaItem.Item.Questao.Descricao,
                Formulario = new FormularioDTO()
                {
                    Id = planoAcao.RespostaItem.Resposta.IdQuestionario,
                    Descricao = planoAcao.RespostaItem.Resposta.AvaliacaoQuestionarioGrupo.QuestionarioGrupo.Questionario.Nome,
                    Grupo = planoAcao.RespostaItem.Resposta.AvaliacaoQuestionarioGrupo.QuestionarioGrupo.Grupo.Nome,
                    Avaliacao = new AvaliacaoDTO()
                    {
                        Id = planoAcao.RespostaItem.Resposta.IdAvaliacao,
                        IdEmpresa = planoAcao.RespostaItem.Resposta.AvaliacaoQuestionarioGrupo.Avaliacao.Unidade.IdEmpresa,
                        Rotulo = planoAcao.RespostaItem.Resposta.AvaliacaoQuestionarioGrupo.Avaliacao.RotuloCalendario,
                        Usuario = planoAcao.RespostaItem.Resposta.AvaliacaoQuestionarioGrupo.Avaliacao.Usuario.UserName,
                        Unidade = planoAcao.RespostaItem.Resposta.AvaliacaoQuestionarioGrupo.Avaliacao.Unidade.Nome,
                        DataFim = (DateTime) planoAcao.RespostaItem.Resposta.AvaliacaoQuestionarioGrupo.Avaliacao.DataFim
                    }  
                },
                Alternativas = new List<AlternativaDTO>()
            };

            List<Item> itens = new List<Item>(planoAcao.RespostaItem.Item.Questao.Itens.OrderBy(i => i.Alternativa.Ordem));

            foreach (Item item in itens)
            {
                AlternativaDTO alternativaDTO = new AlternativaDTO()
                {
                    Id = item.Alternativa.Id,
                    Descricao = item.Alternativa.Descricao,
                    NaoConformidade = item.Alternativa.NaoConformidade
                };
                planoAcaoDTO.Questao.Alternativas.Add(alternativaDTO);
            }

            planoAcaoDTO.Questao.Alternativas.Single(a => a.Id == planoAcao.RespostaItem.Item.IdAlternativa).Respondida = true;

            return planoAcaoDTO;
        }

        public static IList<PlanoAcaoDTO> MapPlanoAcaoToPlanoAcaoDTO(IEnumerable<PlanoAcao> planosAcao)
        {
            IList<PlanoAcaoDTO> planos = new List<PlanoAcaoDTO>(); 
            foreach (var planoAcao in planosAcao)
            {
                planos.Add(MapPlanoAcaoToPlanoAcaoDTO(planoAcao));
            }
            return planos;
        }

        private static MapperConfiguration GetConfig()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // Model -> DTO
                cfg.CreateMap<PlanoAcao, PlanoAcaoDTO>();
            });
            return config;
        }
    }
}
