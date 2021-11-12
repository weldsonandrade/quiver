using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiver.WebAPI.DTO
{
    public class ItemDTO
    {
        public int Id { get; set; }

        public AlternativaDTO Alternativa { get; set; }
    }
}