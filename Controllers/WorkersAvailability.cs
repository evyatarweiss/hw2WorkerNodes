using Microsoft.AspNetCore.Mvc;
using WorkNode.WorkNode;

namespace WorkNode.Controllers
{
    [ApiController]
    public class WorkersAvailability : ControllerBase
    {
        CurrentVirtualMachine CurrentVM = new CurrentVirtualMachine();

        [HttpPut]
        [Route("/DeleteMachine")]
        public ContentResult DeleteCurrentMachine()
        {
            CurrentVM.TurnOffMachineBool();
            return new ContentResult() { StatusCode = 200 };
        }

        [HttpGet]
        [Route("/DeleteAvailability")]
        public ContentResult AbleToDeleteMachine()
        {
            if (CurrentVM.threadsAreBusy())
            {
                string False = "false";
                return new ContentResult() {Content = False, StatusCode = 200 };
            }
            else
            {
                string True = "true";
                return new ContentResult() {Content = True, StatusCode = 200 };
            }
        }
    }
}