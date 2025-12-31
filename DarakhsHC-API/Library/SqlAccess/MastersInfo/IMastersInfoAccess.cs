using DarakhsHC_API.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarakhsHC_API.Library.SqlAccess.MastersInfo
{
    public interface IMastersInfoAccess
    {
        int UpsertReference(ReferencesInfo referencesInfo);

        int UpsertTreatement(TreatmentsInfo treatment);
    }
}
