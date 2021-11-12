using Microsoft.AspNet.Identity;
using Quiver.Core.Models;
using Quiver.Data.Repository;
using Quiver.DTO.Models;
using Quiver.WebAPI.Mailers;
using Quiver.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Quiver.WebAPI.DTO
{
    public class SendSyncContainer
    {
        private UnitOfWork _uow = new UnitOfWork();

        public string Usuario { get; set; }

        public string Senha { get; set; }

        public List<AvaliacaoDTO> Avaliacoes { get; set; }

        public bool Sincronizar()
        {
            Usuario usuario = _uow.UsuarioRepository.Get(u => u.UserName == Usuario).SingleOrDefault();

            if (usuario == null)
            {
                return false;
            }
            else if (new PasswordHasher().VerifyHashedPassword(usuario.PasswordHash, Senha) == PasswordVerificationResult.Failed)
            {
                return false;
            }

            // Obtém todos os usuários gestores.
            var gestorRoleId = _uow.RoleRepository.Get(r => r.Name == "Gestor").FirstOrDefault().Id;
            var gestores = _uow.UsuarioRepository.Get(
                u => u.IdEmpresa == usuario.IdEmpresa &&
                u.Roles.Where(r => r.RoleId == gestorRoleId).Count() == 1);

            if (gestores == null || gestores.Count() == 0)
            {
                return false;
            }

            foreach (AvaliacaoDTO avaliacao in Avaliacoes)
            {
                Unidade unidadeAvaliacao = _uow.UnidadeRepository.GetByID(avaliacao.IdUnidade);
                Grupo grupoAvaliado = _uow.GrupoRepository.GetByID(avaliacao.QuestionariosGrupo.First().IdGrupo);

                if (avaliacao.IsLocal)
                {
                    // Foi criado pelo inspetor
                    Avaliacao nAvaliacao = new Avaliacao()
                    {
                        Agendada = false,
                        Assinatura = avaliacao.Assinatura,
                        CargoResponsavel = avaliacao.CargoResponsavel,
                        NomeResponsavel = avaliacao.NomeResponsavel,
                        DataCriacao = avaliacao.DataCriacao,
                        DataFim = avaliacao.DataFim,
                        DataInicio = avaliacao.DataInicio,
                        DataProgramada = avaliacao.DataProgramada,
                        Dispositivo = avaliacao.Dispositivo,
                        Unidade = unidadeAvaliacao,
                        Observacao = avaliacao.Observacao,
                        Usuario = usuario,
                        Situacao = Core.Models.SituacaoAvaliacao.AVALIADO,
                        LocalizacaoLatitude = avaliacao.Latitude,
                        LocalizacaoLongitude = avaliacao.Longitude,
                        RotuloCalendario = unidadeAvaliacao.Nome + " - " + grupoAvaliado.Nome
                    };

                    nAvaliacao.Conforme = InserirRespostas(avaliacao, nAvaliacao, gestores.ToList());

                    _uow.AvaliacaoRepository.Insert(nAvaliacao);

                    InserirNotificacao(nAvaliacao, gestores.ToList());
                }
                else
                {
                    // Avaliação criada pelo Administrador.
                    Avaliacao uAvaliacao = _uow.AvaliacaoRepository.GetByID(avaliacao.Id);

                    // Se igual a null é por que o gestor excluir a avaliação e assim
                    // ela não precisa ser processada.
                    if (uAvaliacao != null)
                    {
                        // Se ainda não foi enviada ao servidor.
                        if (uAvaliacao.Situacao == Core.Models.SituacaoAvaliacao.NAO_AVALIADO)
                        {
                            uAvaliacao.Assinatura = avaliacao.Assinatura;
                            uAvaliacao.CargoResponsavel = avaliacao.CargoResponsavel;
                            uAvaliacao.DataFim = avaliacao.DataFim;
                            uAvaliacao.DataInicio = avaliacao.DataInicio;
                            uAvaliacao.Dispositivo = avaliacao.Dispositivo;
                            uAvaliacao.LocalizacaoLatitude = avaliacao.Latitude;
                            uAvaliacao.LocalizacaoLongitude = avaliacao.Longitude;
                            uAvaliacao.NomeResponsavel = avaliacao.NomeResponsavel;
                            uAvaliacao.Observacao = avaliacao.Observacao;
                            uAvaliacao.Situacao = Core.Models.SituacaoAvaliacao.AVALIADO;

                            uAvaliacao.Conforme = InserirRespostas(avaliacao, uAvaliacao, gestores.ToList());
                        }

                        _uow.AvaliacaoRepository.Update(uAvaliacao);

                        InserirNotificacao(uAvaliacao, gestores.ToList());
                    }
                }
            }

            _uow.SaveChanges();

            List<Usuario> administradores = _uow.UsuarioRepository.Get(
                u => u.IdEmpresa == usuario.IdEmpresa &&
                u.Roles.Where(r => r.RoleId == gestorRoleId).Count() == 1).ToList();

            foreach (Usuario administradorParaNotificar in administradores)
            {
                var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                foreach (HubUserConnection userConnection in Startup.userConnectionList.Where(u => u.UserID == administradorParaNotificar.Id))
                {
                    string connectionId = userConnection.ConnectionID;
                    context.Clients.Client(connectionId).receberSinalParaUsuario(1);
                }
            }

            return true;
        }

        private void InserirNotificacao(Avaliacao avaliacao, List<Usuario> gestores)
        {
            foreach (var gestor in gestores)
            {
                var data = (DateTime)avaliacao.DataFim;
                Notificacao nNotificacao = new Notificacao()
                {
                    Avaliacao = avaliacao,
                    Lida = false,
                    Tipo = TipoNotificacao.AVALIADO,
                    Data = data,
                    UsuarioNotificado = gestor
                };

                _uow.NotificacaoRepository.Insert(nNotificacao);
            }
        }

        /// <summary>
        /// Calcula a pontuação da avaliação,
        /// Insere ou Atualiza AvaliaçãoQuestionario,
        /// Insere Resposta
        /// Insere fotos na Base64
        /// Insere RespostaItem
        /// </summary>
        private bool InserirRespostas(AvaliacaoDTO avaliacaoDto, Avaliacao avaliacao, List<Usuario> gestores)
        {
            int pontuacaoEfetuada = 0;
            int pontuacaoMaxima = 0;
            bool hasNaoConformidade = false;
            foreach (AvaliacaoQuestionarioGrupoDTO aq in avaliacaoDto.QuestionariosGrupo)
            {
                AvaliacaoQuestionarioGrupo qAvaliacao = null;

                // Se a avaliação foi criada pelo inspetor então é criado o questionário,
                // caso contrário busca o que existe na avaliação.
                if (avaliacao.Id == 0)
                {
                    qAvaliacao = new AvaliacaoQuestionarioGrupo()
                    {
                        QuestionarioGrupo = _uow.QuestionarioGrupoRepository.GetByQuestionarioAndGrupo(aq.IdQuestionario, aq.IdGrupo),
                        Situacao = SituacaoAvaliacao.AVALIADO
                    };

                    avaliacao.QuestionariosAvaliacao.Add(qAvaliacao);
                }
                else
                {
                    qAvaliacao = avaliacao.QuestionariosAvaliacao.SingleOrDefault(av => av.IdAvaliacao == aq.IdAvalicao && av.IdQuestionario == aq.IdQuestionario);
                }

                bool questionarioNaoEstaExcluido = !qAvaliacao.QuestionarioGrupo.Questionario.Excluido;
                if (questionarioNaoEstaExcluido)
                {
                    foreach (Questao questao in qAvaliacao.QuestionarioGrupo.Questionario.Questoes)
                    {
                        switch (questao.Tipo)
                        {
                            // Sem pontuação para item subjetivo.
                            case TipoQuestao.Subjetiva:
                                pontuacaoMaxima += 0;
                                break;

                            // Maior pontuação entre as alternativas.
                            case TipoQuestao.ObjetivaUnicaEscolha:
                                pontuacaoMaxima += questao.Itens.Max(i => i.Alternativa.Peso);
                                break;

                            // Somátorio de todos os pontos de cada alternativa.
                            case TipoQuestao.ObjetivaMultiplaEscolha:
                                pontuacaoMaxima += questao.Itens.Sum(i => i.Alternativa.Peso);
                                break;
                        }
                    }
                }

                if (qAvaliacao != null && !qAvaliacao.QuestionarioGrupo.Questionario.Excluido)
                {
                    qAvaliacao.Situacao = SituacaoAvaliacao.AVALIADO;

                    // Respostas desse questionário.
                    foreach (RespostaDTO resposta in aq.respostas)
                    {
                        Resposta nResp = new Resposta()
                        {
                            Justificativa = resposta.Justificativa
                        };

                        foreach (string foto in resposta.Fotos)
                        {
                            Foto nFoto = new Foto()
                            {
                                Fotografia = foto
                            };
                            nResp.Fotos.Add(nFoto);
                        }

                        foreach (int itemId in resposta.Itens)
                        {
                            Item item = _uow.ItemRepository.GetByID(itemId);

                            if (item != null)
                            {
                                RespostaItem respItem = new RespostaItem();
                                respItem.Item = item;
                                respItem.Resposta = nResp;

                                if (item.Alternativa != null)
                                {
                                    pontuacaoEfetuada += item.Alternativa.Peso;

                                    if (item.Alternativa.NaoConformidade && !hasNaoConformidade)
                                    {
                                        hasNaoConformidade = true;
                                    }
                                }

                                nResp.Itens.Add(respItem);
                            }
                        }

                        if (resposta.Itens != null)
                        {
                            qAvaliacao.Respostas.Add(nResp);
                        }
                    }
                }
            }

            if (hasNaoConformidade)
            {
                try
                {
                    INotificacaoMailer _mailer = new NotificacaoMailer();
                    foreach (var gestor in gestores)
                    {
                        _mailer.Notificar(new NotificacaoVM()
                        {
                            Aplicador = avaliacao.Usuario.UserName,
                            Unidade = avaliacao.Unidade.Nome,
                            Grupo = avaliacao.QuestionariosAvaliacao.First().QuestionarioGrupo.Grupo.Nome,
                            DataProgramada = avaliacao.DataProgramada.ToShortDateString(),
                            Email = gestor.Email
                        }).Send();
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            avaliacao.PontuacaoEfetuada = pontuacaoEfetuada;
            avaliacao.PontuacaoMaxima = pontuacaoMaxima;

            return !hasNaoConformidade;
        }
    }
}