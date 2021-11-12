using Quiver.Core.Models;
using System;
using System.Collections.Generic;

namespace Quiver.Data.Interfaces
{
    public interface IPlanoAcaoRepository : IRepository<PlanoAcao>
    {
        void Update(PlanoAcao planoAcaoToUpdate, bool autoHistorico);

        IEnumerable<PlanoAcao> GetByEmpresaAndPeriodo(int idEmpresa, Nullable<DateTime> dataInicial, Nullable<DateTime> dataFinal);

        IEnumerable<PlanoAcao> GetByEmpresaAndResponsavelAndUnidadesAndUsuarios(int idEmpresa, string emailResponsavel, List<int> unidades, List<string> usuarios);

        IEnumerable<PlanoAcao> GetByEmpresa(int idEmpresa);
    }
}
