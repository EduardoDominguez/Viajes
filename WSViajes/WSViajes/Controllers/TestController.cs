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
            open.CreateCustomer("Mario Eduardo", "Dominguez Meléndez", "mario.ed.domm@gmail.com");
            return Ok(true);

        }

        [HttpGet]
        [Route("openpay/create-charge-customer")]
        public IHttpActionResult CreateChargeCustomer()
        {
            var open = new OpenPayFunctions();
            open.CreateCharge();
            return Ok(true);

        }

        [HttpGet]
        [Route("openpay/create-card-customer")]
        public IHttpActionResult CreateCardCustomer()
        {
            var open = new OpenPayFunctions();
            open.CreateCard();
            return Ok(true);

        }


        [HttpGet]
        [Route("mail/send")]
        public IHttpActionResult SendMailTest()
        {
            var mailer = new Mailer();
            mailer.Send("mario.ed.domm@gmail.com", "Prueba", "<b>Este es un correo de prueba</b>");
            return Ok(true);
        }


    }
}
