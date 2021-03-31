using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Web.Hosting;
using System.Web.Mvc;
using VeraDemoNet.Models;

namespace VeraDemoNet.Controllers
{
    public enum FortuneType
    {
        Funny,
        Offensive
    }

    public class ToolsController : AuthControllerBase
    {
        protected readonly log4net.ILog logger;

        public ToolsController()
        {
            logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);    
        }

        [HttpGet, ActionName("Tools")]
        public ActionResult GetTools()
        {
            if (IsUserLoggedIn() == false)
            {
                return RedirectToLogin(HttpContext.Request.RawUrl);
            }

            return View(new ToolViewModel());
        }

        [HttpPost]
        public ActionResult Tools(string host, FortuneType fortuneType)
        {
            if (IsUserLoggedIn() == false)
            {
                return RedirectToLogin(HttpContext.Request.RawUrl);
            }

            var viewModel = new ToolViewModel();
            viewModel.Host = host;
            viewModel.PingResult = Ping(host);
            viewModel.FortuneResult = Fortune(fortuneType);

            return View("Tools", viewModel);
        }

        private string Ping(string host)
        {
            if (string.IsNullOrEmpty(host))
            {
                return "";
            }

            var output = new StringBuilder();
            try
            {
                Ping pingSender = new Ping();
                PingOptions options = new PingOptions();

                // Use the default Ttl value which is 128,
                // but change the fragmentation behavior.
                options.DontFragment = true;

                // Create a buffer of 32 bytes of data to be transmitted.
                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 120;
                PingReply reply = pingSender.Send(host, timeout, buffer, options);
                if (reply.Status == IPStatus.Success)
                {
                    output.AppendFormat("Address: {0}\n", reply.Address);
                    output.AppendFormat("RoundTrip time: {0}\n", reply.RoundtripTime);
                    output.AppendFormat("Time to live: {0}\n", reply.Options.Ttl);
                    output.AppendFormat("Don't fragment: {0}\n", reply.Options.DontFragment);
                    output.AppendFormat("Buffer size: {0}\n", reply.Buffer.Length);
                }
                else
                {
                    return "Host is unavailable";
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            return output == null ? "" : output.ToString();
        }

        private string Fortune(FortuneType fortuneType)
        {
            var actualFileName = $"{fortuneType}.txt";
            var output = new StringBuilder();

            try
            {
                var file = HostingEnvironment.MapPath("~/Resources/bin/" + actualFileName);
                return System.IO.File.ReadAllText(file);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return output == null ? "" : output.ToString();
        }
    }
}