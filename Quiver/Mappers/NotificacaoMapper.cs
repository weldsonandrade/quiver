using AutoMapper;
using Quiver.DTO.Notificacao;
using Quiver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiver.Mappers
{
    public class NotificacaoMapper
    {
        public static HeaderViewModel MapVisualizarNotificacoesDTOToHeaderViewModel(VisualizarNotificacoesDTO notificacoesDTO)
        {
            return GetConfig().CreateMapper().Map<VisualizarNotificacoesDTO, HeaderViewModel>(notificacoesDTO);
        }

        private static MapperConfiguration GetConfig()
        {
            // Marca a cor de cada intervalo entre todas disponíveis.
            var config = new MapperConfiguration(cfg =>
            {
                // VisualizarNotificacoesDTO -> HeaderViewModel
                cfg.CreateMap<VisualizarNotificacoesDTO, HeaderViewModel>();
            });
            return config;
        }
    }
}