using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Quiver.Core.Models
{
    public class Usuario : IdentityUser
    {
        public Usuario()
        {
            this.Avaliacoes = new HashSet<Avaliacao>();
            this.Grupos = new HashSet<Grupo>();
        }

        [Required]
        [ForeignKey("Empresa")]
        public int IdEmpresa { get; set; }

        public virtual Empresa Empresa { get; set; }

        [StringLength(100)]
        public string Nome { get; set; }

        public virtual ICollection<Avaliacao> Avaliacoes { get; set; }

        public virtual ICollection<Grupo> Grupos { get; set; }
        
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Usuario> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
