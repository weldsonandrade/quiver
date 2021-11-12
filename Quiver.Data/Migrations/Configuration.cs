namespace Quiver.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Quiver.Core.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Globalization;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Quiver.Data.QuiverDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Quiver.Data.QuiverDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            if (context.Empresas.Count() == 0)
            {
                // Hello World 
                var empresa = new Empresa() { CNPJ = "16872457000126", Nome = "Hello World Software", Icone = "default", Situacao = SituacaoEmpresa.ATIVA, LimiteLicencas = 1000 };
                context.Empresas.Add(empresa);
                var empCliente = new Empresa() { CNPJ = "00000000000001", Nome = "JAILSON", Icone = "default", Situacao = SituacaoEmpresa.ATIVA, LimiteLicencas = 10 };
                context.Empresas.Add(empCliente);

                // Roles
                var roles = new List<IdentityRole>()
                {
                    new IdentityRole() { Id = "1", Name = "Administrador" },
                    new IdentityRole() { Id = "2", Name = "Gestor" },
                    new IdentityRole() { Id = "3", Name = "Inspetor" }
                };
                roles.ForEach(i => context.Roles.AddOrUpdate<IdentityRole>(i));

                // Usuario
                var manager = new UserManager<Usuario>(new UserStore<Usuario>(context));
                var user = new Usuario() { UserName = "admin@helloworldsoft.com", Email = "admin@helloworldsoft.com", Empresa = empresa, EmailConfirmed = true };
                manager.Create(user, "NovaSenha123**");

                manager.AddToRole(user.Id, "Administrador");

                var uJailson = new Usuario() { Nome = "Jalison",  UserName = "jalison@email.com", Email = "jalison@email.com", Empresa = empCliente, EmailConfirmed = true };
                manager.Create(uJailson, "jalison1234");

                manager.AddToRole(uJailson.Id, "Gestor");

                //// Grupos
                //var grupos = new List<Grupo>()
                //{
                //    new Grupo() { Empresa = empresa, Nome = "Frente de Loja", Usuarios = new List<Usuario>() { user } },
                //    new Grupo() { Empresa = empresa, Nome = "Fundo de Loja", Usuarios = new List<Usuario>() { user } }
                //};
                //grupos.ForEach(g => context.Grupos.AddOrUpdate<Grupo>(g));

                //// Unidades
                //var unidades = new List<Unidade>()
                //{
                //    new Unidade() { Empresa = empresa, Nome = "PZ-1234" },
                //    new Unidade() { Empresa = empresa, Nome = "PZ-5555" },
                //    new Unidade() { Empresa = empresa, Nome = "PZ-9999" }
                //};
                //unidades.ForEach(u => context.Unidades.AddOrUpdate<Unidade>(u));

                //// Avaliacoes
                //var avaliacoes = new List<Avaliacao>()
                //{
                //    new Avaliacao() { DataCriacao = new DateTime(2016, 01, 20, 10, 22, 10), DataProgramada = new DateTime(2016, 02, 22), 
                //        RotuloCalendario = "Avaliação Utilizada", Situacao = SituacaoAvaliacao.NAO_AVALIADO,
                //    Unidade = unidades.First(), Usuario = user },
                //    new Avaliacao() { DataCriacao = new DateTime(2016, 02, 24, 12, 25, 30), DataProgramada = new DateTime(2016, 02, 24), 
                //        RotuloCalendario = "Outra Avaliação", Situacao = SituacaoAvaliacao.NAO_AVALIADO,
                //    Unidade = unidades.First(), Usuario = user },
                //    new Avaliacao() { DataCriacao = new DateTime(2016, 02, 22, 09, 09, 30), DataProgramada = new DateTime(2016, 02, 26),
                //    Unidade = unidades[1], Usuario = user}
                //};
                //avaliacoes.ForEach(a => context.Avalicoes.AddOrUpdate<Avaliacao>(a));

                //CreateQuestionario01(context, grupos[0], avaliacoes);

                //CreateQuestionario02(context, grupos[0], avaliacoes);

                //CreateQuestionario03(context, grupos[1], avaliacoes);

                context.SaveChanges();
            }
        }

        private void CreateQuestionario01(QuiverDbContext context, Grupo grupo, List<Avaliacao> avaliacoes)
        {
            var questionario = new Questionario()
            {
                Excluido = false,
                Nome = "Maquinário",
                Ordem = 1,
                Grupos = new List<QuestionarioGrupo>(){
                            new QuestionarioGrupo(){
                                Grupo = grupo,
                                Avaliacoes = new List<AvaliacaoQuestionarioGrupo>() {
                                    new AvaliacaoQuestionarioGrupo()
                                    {
                                        Avaliacao = avaliacoes[0],
                                        Situacao = SituacaoAvaliacao.NAO_AVALIADO
                                    }
                                }                                
                            },
                            new QuestionarioGrupo(){
                                Grupo = grupo,
                                Avaliacoes = new List<AvaliacaoQuestionarioGrupo>() {
                                    new AvaliacaoQuestionarioGrupo()
                                    {
                                        Avaliacao = avaliacoes[1],
                                        Situacao = SituacaoAvaliacao.NAO_AVALIADO
                                    }
                                }
                            }
                },                
                Questoes = new List<Questao>()
                {
                    new Questao(){
                        Descricao = "Comentários do usuário",
                        ExigeJustificativa = false,
                        ExigeResposta = false,
                        Ordem = 1,
                        Tipo = TipoQuestao.Subjetiva,
                        Itens = new List<Item>()
                        {
                            new Item()
                            { }
                        }
                    },
                    new Questao(){
                        Descricao = "O maquinário está funcionado?",
                        ExigeJustificativa = false,
                        ExigeResposta = true,
                        Ordem = 0,
                        Tipo = TipoQuestao.ObjetivaUnicaEscolha,
                        Itens = new List<Item>()
                        {
                            new Item()
                            {
                                Alternativa = new Alternativa()
                                {
                                    Ordem = 1,
                                    Descricao = "Sim",
                                    Peso = 1,
                                    NaoConformidade = false
                                }
                            },
                            new Item()
                            {
                                Alternativa = new Alternativa()
                                {
                                    Ordem = 0,
                                    Descricao = "Não",
                                    Peso = 0,
                                    NaoConformidade = true
                                }
                            }
                        }
                    },
                    new Questao(){
                        Descricao = "Qual o nível de satisfação dos usuários?",
                        ExigeJustificativa = false,
                        ExigeResposta = true,
                        Ordem = 3,
                        Tipo = TipoQuestao.ObjetivaUnicaEscolha,
                        Itens = new List<Item>()
                        {
                            new Item()
                            {
                                Alternativa = new Alternativa()
                                {
                                    Ordem = 0,
                                    Descricao = "Péssimo",
                                    Peso = 0,
                                    NaoConformidade = true
                                }
                            },
                            new Item()
                            {
                                Alternativa = new Alternativa()
                                {
                                    Ordem = 1,
                                    Descricao = "Ruím",
                                    Peso = 1,
                                    NaoConformidade = true
                                }
                            },
                            new Item()
                            {
                                Alternativa = new Alternativa()
                                {
                                    Ordem = 2,
                                    Descricao = "Regular",
                                    Peso = 2,
                                    NaoConformidade = false
                                }
                            },
                            new Item()
                            {
                                Alternativa = new Alternativa()
                                {
                                    Ordem = 3,
                                    Descricao = "Bom",
                                    Peso = 3,
                                    NaoConformidade = false
                                }
                            },
                            new Item()
                            {
                                Alternativa = new Alternativa()
                                {
                                    Ordem = 4,
                                    Descricao = "Ótimo",
                                    Peso = 4,
                                    NaoConformidade = false
                                }
                            }
                        }
                    },
                    new Questao(){
                        Descricao = "O que os usuários acham que precisa melhorar?",
                        ExigeJustificativa = false,
                        ExigeResposta = true,
                        Ordem = 2,
                        Tipo = TipoQuestao.ObjetivaMultiplaEscolha,
                        Itens = new List<Item>()
                        {
                            new Item()
                            {
                                Alternativa = new Alternativa()
                                {
                                    Descricao = "Velocidade da rede",
                                    Peso = 0,
                                    NaoConformidade = false
                                }
                            },
                            new Item()
                            {
                                Alternativa = new Alternativa()
                                {
                                    Descricao = "Velocidade dos PCs",
                                    Peso = 0,
                                    NaoConformidade = false
                                }
                            },
                            new Item()
                            {
                                Alternativa = new Alternativa()
                                {
                                    Descricao = "Temperatura do ar condicionado",
                                    Peso = 0,
                                    NaoConformidade = false
                                }
                            }
                        }
                    }
                }
            };

            context.Questionarios.AddOrUpdate(questionario);
        }

        private void CreateQuestionario02(QuiverDbContext context, Grupo grupo, List<Avaliacao> avaliacoes)
        {
            var questionario = new Questionario()
            {
                Excluido = false,
                Nome = "Fachada",
                Ordem = 1,
                Grupos = new List<QuestionarioGrupo>(){
                            new QuestionarioGrupo(){
                                Grupo = grupo,
                                Avaliacoes = new List<AvaliacaoQuestionarioGrupo>() {
                                    new AvaliacaoQuestionarioGrupo()
                                    {
                                        Avaliacao = avaliacoes[0],
                                        Situacao = SituacaoAvaliacao.NAO_AVALIADO
                                    }
                                }
                            },
                            new QuestionarioGrupo(){
                                Grupo = grupo,
                                Avaliacoes = new List<AvaliacaoQuestionarioGrupo>() {
                                    new AvaliacaoQuestionarioGrupo()
                                    {
                                        Avaliacao = avaliacoes[1],
                                        Situacao = SituacaoAvaliacao.NAO_AVALIADO
                                    }
                                }
                            }
                },
                Questoes = new List<Questao>()
                {
                    new Questao(){
                        Descricao = "O maquinário está funcionado?",
                        ExigeJustificativa = false,
                        ExigeResposta = true,
                        Ordem = 1,
                        Tipo = TipoQuestao.ObjetivaUnicaEscolha,
                        Itens = new List<Item>()
                        {
                            new Item()
                            {
                                Alternativa = new Alternativa()
                                {
                                    Ordem = 1,
                                    Descricao = "Sim",
                                    Peso = 1,
                                    NaoConformidade = false
                                }
                            },
                            new Item()
                            {
                                Alternativa = new Alternativa()
                                {
                                    Ordem = 0,
                                    Descricao = "Não",
                                    Peso = 0,
                                    NaoConformidade = true
                                }
                            }
                        }
                    },
                    new Questao(){
                        Descricao = "A fachada está limpa?",
                        ExigeJustificativa = false,
                        ExigeResposta = true,
                        Ordem = 2,
                        Tipo = TipoQuestao.ObjetivaUnicaEscolha,
                        Itens = new List<Item>()
                        {
                            new Item()
                            {
                                Alternativa = new Alternativa()
                                {
                                    Ordem = 1,
                                    Descricao = "Sim",
                                    Peso = 1,
                                    NaoConformidade = false
                                }
                            },
                            new Item()
                            {
                                Alternativa = new Alternativa()
                                {
                                    Ordem = 0,
                                    Descricao = "Não",
                                    Peso = 0,
                                    NaoConformidade = true
                                }
                            }
                        }
                    }
                }
            };

            context.Questionarios.AddOrUpdate(questionario);
        }

        private void CreateQuestionario03(QuiverDbContext context, Grupo grupo, List<Avaliacao> avaliacoes)
        {
            var questionario = new Questionario()
            {
                Excluido = false,
                Nome = "Comercial",
                Ordem = 1,
                Grupos = new List<QuestionarioGrupo>(){
                            new QuestionarioGrupo(){
                                Grupo = grupo,
                                Avaliacoes = new List<AvaliacaoQuestionarioGrupo>() {
                                    new AvaliacaoQuestionarioGrupo()
                                    {
                                        Avaliacao = avaliacoes[2],
                                        Situacao = SituacaoAvaliacao.NAO_AVALIADO
                                    }
                                }
                            }
                },
                Questoes = new List<Questao>()
                {
                    new Questao(){
                        Descricao = "O estoque foi conferido?",
                        ExigeJustificativa = false,
                        ExigeResposta = true,
                        Ordem = 1,
                        Tipo = TipoQuestao.ObjetivaUnicaEscolha,
                        Itens = new List<Item>()
                        {
                            new Item()
                            {
                                Alternativa = new Alternativa()
                                {
                                    Ordem = 1,
                                    Descricao = "Sim",
                                    Peso = 1,
                                    NaoConformidade = false
                                }
                            },
                            new Item()
                            {
                                Alternativa = new Alternativa()
                                {
                                    Ordem = 2,
                                    Descricao = "Não",
                                    Peso = 0,
                                    NaoConformidade = true
                                }
                            }
                        }
                    }
                }
            };

            context.Questionarios.AddOrUpdate(questionario);
        }
    }
}
