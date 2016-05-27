using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using myAPI_SAP_RFC_001.Models;

namespace myAPI_SAP_RFC_001.Controllers
{
    public class SAPRFC_RFC_READ_TABLEController : ApiController
    {
        public IEnumerable<SAPRFC_RFC_READ_TABLEModel> GetParameter(string QUERY_TABLE)
        {
            Models.SAPRFC_RFC_READ_TABLEModel ccc = new Models.SAPRFC_RFC_READ_TABLEModel();
            ccc.iQueryTable = QUERY_TABLE;

            System.Threading.Thread s = new System.Threading.Thread(new System.Threading.ThreadStart(ccc.Main));
            s.SetApartmentState(System.Threading.ApartmentState.STA);
            s.Start();
            s.Join();

            yield return ccc;
        }
    }
}
