using DarakhsHC_API.Library.Models;
using DarakhsHC_API.Library.SqlAccess.MastersInfo;
using DarakhsHC_API.Library.SqlAccess.PatientsInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DarakhsHC_API.Library.ServerModel
{
    public class MasterSetupServer
    {
        #region Properties Interface

        public static IMastersInfoAccess mMasterssInfoAccessT = new MastersInfoAccessWrapper();
        public static IPatientsInfoAccess mPatientInfoAccessT = new PatientsInfoAccessWrapper();


        #endregion

        public static int UpsertReference(ReferencesInfo reference)
        {
            return mMasterssInfoAccessT.UpsertReference(reference);
        }

        public static int UpsertTreatement(TreatmentsInfo treatment)
        {
            return mMasterssInfoAccessT.UpsertTreatement(treatment);
        }

    }
}