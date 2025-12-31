using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DarakhsHC_API.Library.Models;

namespace DarakhsHC_API.Library.SqlAccess.MastersInfo
{
    public class MastersInfoAccessWrapper : IMastersInfoAccess
    {
        public int UpsertReference(ReferencesInfo referencesInfo)
        {
            return MastersInfoAccess.UpsertReference(referencesInfo);
        }

        public int UpsertTreatement(TreatmentsInfo treatment)
        {
            return MastersInfoAccess.UpsertTreatement(treatment);
        }
    }
}