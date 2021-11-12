using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiver.WebAPI.DTO
{
    public class UnidadeDTO
    {
        public int Id { get; set; }

        public string Nome { get; set; }
    }
}