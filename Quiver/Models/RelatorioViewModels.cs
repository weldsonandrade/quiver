using Quiver.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiver.Models
{
    public class RankingGeralVM
    {
        public string Categoria;
        public double Efetividade;
        public string QtdAvaliacoes;
        public string Posicao;
        public string idSelecionado; // Id do usuário, grupo ou unidade
    }

    public class DetalhesPosicaoVM
    {
        public string QtdAvaliacoesNaoConformes;
        public string QtdAvaliacoesAgendadas;
    }


    public class EvolutivoPorUnidadeVM
    {
        public string Unidade;
        public string DataInicial;
        public string DataFinal;
        
    }

    public class EvolutivoGeralVM
    {
        public string DataExecutada;
        public string EfetividadeMedia;
        public string QtdAvaliacoes;
        public List<Avaliacao> avaliacoesDia;


    }


    public class PontoEvolutivoGeralVM
    {
        public long TimeStampDataExecutada;
        public string DataExecutada;
        public string EfetividadeMedia;
        public string QtdAvaliacoes;
        public List<AvaliacaoPontoVM> avaliacoesDia;

        public PontoEvolutivoGeralVM() {
            avaliacoesDia = new List<AvaliacaoPontoVM>();
        }
    }


    public class PontoQuantidadeGeralVM
    {
        public long TimeStampDataExecutada;
        public string DataExecutada;
        public string QtdAvaliacoes;
        public List<AvaliacaoPontoVM> avaliacoesDia;

        public PontoQuantidadeGeralVM()
        {
            avaliacoesDia = new List<AvaliacaoPontoVM>();
        }
    }



    public class AvaliacaoPontoVM { 
        public string efetividade;
        public string rotulo;
        public string nomeUnidade;
        public long TimeStampDataExecutada;
        public string DataExecutada;
        public string nomeGrupo;

    }



   



}