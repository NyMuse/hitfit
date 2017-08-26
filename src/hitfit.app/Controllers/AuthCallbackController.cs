using hitfit.google.auth.Mvc;

namespace hitfit.app.Controllers
{
    public class AuthCallbackController : google.auth.Mvc.Controllers.AuthCallbackController
    {
        protected override FlowMetadata FlowData
        {
            get { return new Google.AppFlowMetadata(); }
        }
    }
}