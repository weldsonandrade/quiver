using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiver.WebAPI.DTO
{
    public class AlternativaDTO
    {
        public int Id { get; set; }

        public string Descricao { get; set; }

        public int Ordem { get; set; }

        public bool ExigeJustificativa { get; set; }
    }
}