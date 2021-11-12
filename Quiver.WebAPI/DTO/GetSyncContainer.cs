using Microsoft.AspNet.Identity;
using Quiver.Common.Utils;
using Quiver.Core.Models;
using Quiver.Data.Repository;
using Quiver.DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quiver.WebAPI.DTO
{
    public class GetSyncContainer
    {
        private UnitOfWork _uow = new UnitOfWork();

        public GetSyncContainer(string userName, string password)
        {
            try
            {
                Usuario usuario = _uow.UsuarioRepository.Get(u => u.UserName == userName).SingleOrDefault();

                if (usuario == null || new PasswordHasher().VerifyHashedPassword(usuario.PasswordHash, password) == PasswordVerificationResult.Failed)
                {
                    throw new Exception("Usuário ou senha inválido");
                }

                Avaliacoes = new List<AvaliacaoDTO>();
                Unidades = new List<UnidadeDTO>();
                Grupos = new List<GrupoDTO>();

                Empresa = usuario.Empresa.Nome;

                IdUsuario = usuario.Id;

                Grupos = GetGruposAtivos(usuario);

                Unidades = GetUnidadesAtivas(usuario);

                Avaliacoes = GetAvaliacoes(usuario);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string Empresa { get; set; }

        public string IdUsuario { get; set; }

        public List<GrupoDTO> Grupos { get; set; }

        public List<UnidadeDTO> Unidades { get; set; }

        public List<AvaliacaoDTO> Avaliacoes { get; set; }

        private List<GrupoDTO> GetGruposAtivos(Usuario usuario)
        {
            List<GrupoDTO> grupos = new List<GrupoDTO>();

            try
            {
                grupos = _uow.GrupoRepository.Get(g => g.IdEmpresa == usuario.IdEmpresa && g.Excluido == false && g.Questionarios.Count > 0).Select(g => new GrupoDTO()
                {
                    Id = g.Id,
                    Nome = g.Nome,
                    QuestionariosGrupo = g.Questionarios.Where(qn => !qn.Questionario.Excluido).Select(qng => new QuestionarioGrupoDTO()
                    {
                        IdQuestionario = qng.IdQuestionario,
                        IdGrupo = qng.IdGrupo,
                        Questionario = new QuestionarioDTO()
                        {
                            Id = qng.Questionario.Id,
                            Nome = qng.Questionario.Nome,
                            Ordem = qng.Questionario.Ordem,
                            Questoes = qng.Questionario.Questoes.Select(qs => new QuestaoDTO()
                            {
                                Id = qs.Id,
                                Descricao = qs.Descricao,
                                ExigeJustificativa = qs.ExigeJustificativa,
                                ExigeResposta = qs.ExigeResposta,
                                Ordem = qs.Ordem,
                                Tipo = qs.Tipo,
                                Itens = qs.Itens.Select(i => new ItemDTO()
                                {
                                    Id = i.Id,
                                    Alternativa = i.Alternativa != null ?
                                    new AlternativaDTO()
                                    {
                                        Id = i.Alternativa.Id,
                                        Descricao = i.Alternativa.Descricao,
                                        ExigeJustificativa = i.Alternativa.ExigeJustificativa,
                                        Ordem = i.Alternativa.Ordem
                                    } : null
                                }).ToList()
                            }).ToList()
                        }                        
                    }).ToList()
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return grupos;
        }

        private List<UnidadeDTO> GetUnidadesAtivas(Usuario usuario)
        {
            List<UnidadeDTO> unidades = new List<UnidadeDTO>();

            try
            {
                unidades = usuario.Empresa.Unidades.Where(u => u.Excluido == false).Select(u => new UnidadeDTO()
                {
                    Id = u.Id,
                    Nome = u.Nome
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return unidades;
        }

        private List<AvaliacaoDTO> GetAvaliacoes(Usuario usuario)
        {
            List<AvaliacaoDTO> avaliacoes = new List<AvaliacaoDTO>();

            try
            {
                // Avaliações não avaliadas e até os próximos 15 dias.
                avaliacoes = usuario.Avaliacoes.Where(a => 
                        a.Situacao == Core.Models.SituacaoAvaliacao.NAO_AVALIADO
                     && a.DataProgramada <= TZUtil.GetDataHoraDeBrasilia().AddDays(15))
                .Select(a => new AvaliacaoDTO()
                {
                    Id = a.Id,
                    DataProgramada = a.DataProgramada,
                    IdUnidade = a.IdUnidade,
                    QuestionariosGrupo = a.QuestionariosAvaliacao.Where(aq => aq.QuestionarioGrupo.Questionario.Excluido == false).Select(aq =>
                        new AvaliacaoQuestionarioGrupoDTO()
                        {
                            IdQuestionario = aq.IdQuestionario,
                            IdAvalicao = aq.IdAvaliacao,
                            IdGrupo = aq.IdGrupo
                        }).ToList()
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return avaliacoes;
        }
    }
}