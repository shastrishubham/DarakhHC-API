using DarakhsHC_API.Library.Models;
using DarakhsHC_API.Library.ServerModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DarakhsHC_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MasterController : ApiController
    {

        #region Reference

        [Route("api/MasterInfo/UpsertReference")]
        [HttpPost]
        public int UpsertReference(ReferencesInfo reference)
        {
            return MasterSetupServer.UpsertReference(reference);
        }

        #endregion

        #region Treatment

        [Route("api/MasterInfo/UpsertTreatment")]
        [HttpPost]
        public int UpsertTreatment(TreatmentsInfo treatment)
        {
            return MasterSetupServer.UpsertTreatement(treatment);
        }

        #endregion
    }
}
