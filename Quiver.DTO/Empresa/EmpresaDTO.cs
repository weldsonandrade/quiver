using Quiver.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiver.DTO.Empresa
{
    public class EmpresaDTO
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Icone { get; set; }

        public int LimiteLicencas { get; set; }

        public string CNPJ { get; set; }

        public SituacaoEmpresa Situacao { get; set; }
    }
}
