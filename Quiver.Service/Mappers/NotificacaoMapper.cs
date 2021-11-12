using AutoMapper;
using Quiver.Core.Models;
using Quiver.DTO.Avaliacao;
using Quiver.DTO.Notificacao;
using Quiver.DTO.Unidade;
using Quiver.DTO.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.Service.Mappers
{
    public class NotificacaoMapper
    {
        public static IList<NotificacaoDTO> MapNotificacaoToNotificacaoDTO(IEnumerable<Notificacao> notificacoes)
        {
            return GetConfig().CreateMapper().Map<IEnumerable<Notificacao>, IList<NotificacaoDTO>>(notificacoes);
        }

        public static VisualizarNotificacoesDTO MapNotificacaoToVisualizarNotificacoesDTO(IEnumerable<Notificacao> notificacoes)
        {
            IList<NotificacaoDTO> notificacoesDTO = GetConfig().CreateMapper().Map<IEnumerable<Notificacao>, IList<NotificacaoDTO>>(notificacoes);
            return new VisualizarNotificacoesDTO() { Notificacoes = notificacoesDTO, QuantidadeNaoLida = notificacoesDTO.Count(n => !n.Lida) };
        }

        private static MapperConfiguration GetConfig()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // Notificacao -> NotificacaoDTO
                cfg.CreateMap<Notificacao, NotificacaoDTO>();
                cfg.CreateMap<Avaliacao, AvaliacaoDTO>()
                .ForMember(dest => dest.NomeUsuario, opt => opt.MapFrom(src => src.Usuario.Nome))
                .ForMember(dest => dest.EmailUsuario, opt => opt.MapFrom(src => src.Usuario.Email));
                cfg.CreateMap<Usuario, UsuarioDTO>();
                cfg.CreateMap<Unidade, UnidadeDTO>();
            });
            return config;
        }
    }
}
