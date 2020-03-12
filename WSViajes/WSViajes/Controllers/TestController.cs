using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;
using WSViajes.Comunes;

namespace WSViajes.Controllers
{
    [AllowAnonymous]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/Test")]
    public class TestController : ApiController
    {
        [HttpGet]
        [Route("echoping")]
        public IHttpActionResult EchoPing()
        {
            return Ok(true);
        }


        [HttpGet]
        [Route("echouser")]
        public IHttpActionResult EchoUser()
        {
            var identity = Thread.CurrentPrincipal.Identity;
            return Ok($"IPrincipal-user: { identity.Name} - IsAuthenticated: {identity.IsAuthenticated}");
        }

        [HttpGet]
        [Route("openpay/create-customer")]
        public IHttpActionResult CreateCustomer()
        {
            var open = new OpenPayFunctions();
            open.createCustomer();
            return Ok(true);

        }

        [HttpGet]
        [Route("openpay/create-charge-customer")]
        public IHttpActionResult CreateChargeCustomer()
        {
            var open = new OpenPayFunctions();
            open.createCharge();
            return Ok(true);

        }

        [HttpGet]
        [Route("openpay/create-card-customer")]
        public IHttpActionResult CreateCardCustomer()
        {
            var open = new OpenPayFunctions();
            open.createCard();
            return Ok(true);

        }
        

    }
}
