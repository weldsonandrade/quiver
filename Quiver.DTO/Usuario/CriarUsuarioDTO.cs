﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.DTO.Usuario
{
    public class CriarUsuarioDTO
    {
        public string Id { get; set; }

        public string IdEmpresa { get; set; }

        public string Email { get; set; }

        public string Nome { get; set; }

        public string Perfil { get; set; }

        public string Senha { get; set; }

        public string PerfilUsuarioLogado { get; set; }

        public int IdEmpresaLogada { get; set; }

        public string Telefone { get; set; }
    }
}
