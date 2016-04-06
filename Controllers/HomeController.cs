using Microsoft.PowerBI.Api.Beta;
using Microsoft.PowerBI.Api.Beta.Models;
using Microsoft.PowerBI.Security;
using Microsoft.Rest;
using PBIEmbeddedApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PBIEmbeddedApp.Controllers
{
    public class HomeController : Controller
    {

        private readonly string workspaceCollection;
        private readonly string workspaceId;
        private readonly string accessKey;
        private readonly string apiUrl;

        /// <summary>
        /// Load web.config keys for workspace and access key configs
        /// </summary>
        public HomeController()
        {
            this.workspaceCollection = ConfigurationManager.AppSettings["powerbi:WorkspaceCollection"];
            this.workspaceId = ConfigurationManager.AppSettings["powerbi:WorkspaceId"];
            this.accessKey = ConfigurationManager.AppSettings["powerbi:AccessKey"];
            this.apiUrl = ConfigurationManager.AppSettings["powerbi:ApiUrl"];
        }

        public async Task<ActionResult> Index()
        {
            List<ReportViewModel> reportsList = new List<ReportViewModel>();
            var devToken = PowerBIToken.CreateDevToken(this.workspaceCollection, this.workspaceId);
            using (var client = this.CreatePowerBIClient(devToken))
            {
                var reportsResponse = client.Reports.GetReports(this.workspaceCollection, this.workspaceId);

                for (int i = 0; i < reportsResponse.Value.ToList().Count; i++)
                {
                    reportsList.Add(new ReportViewModel
                    {
                        Id = reportsResponse.Value[i].Id,
                        Name = reportsResponse.Value[i].Id,
                        EmbedUrl = reportsResponse.Value[i].EmbedUrl,
                        WebUrl = reportsResponse.Value[i].WebUrl,
                        Report = reportsResponse.Value[i]
                    }
                    );
                }
            }


            var reportId = reportsList[0].Id;
            using (var client = this.CreatePowerBIClient(devToken))
            {
                var embedToken = PowerBIToken.CreateReportEmbedToken(this.workspaceCollection, this.workspaceId, reportId);

                var viewModel = new ReportViewModel
                {
                    Report = reportsList[0].Report,
                    AccessToken = embedToken.Generate(this.accessKey)
                };

                return View(viewModel);
            }

        }

        /// <summary>
        /// This is a helper method that create a jason web token from dev token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private IPowerBIClient CreatePowerBIClient(PowerBIToken token)
        {
            var jasonWebToken = token.Generate(accessKey);
            var credentials = new TokenCredentials(jasonWebToken, "AppToken");
            var client = new PowerBIClient(credentials)
            {
                BaseUri = new Uri(apiUrl)
            };

            return client;
        }
    }
}