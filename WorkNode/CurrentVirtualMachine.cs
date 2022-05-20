namespace WorkNode.WorkNode
{
    public class CurrentVirtualMachine
    {
        public static bool isOn { get; set; } = true;
        WorkerNode workerNodeOneInternal;
        WorkerNode worketNodeTwoInternal;

        public CurrentVirtualMachine(WorkerNode workerNodeOne, WorkerNode worketNodeTwo)
        {
            this.workerNodeOneInternal = workerNodeOne;
            this.worketNodeTwoInternal = worketNodeTwo;
        }
        public CurrentVirtualMachine()
        {

        }
        public void TurnOffMachineBool()
        {
            isOn = false;
            return;
        }
        public bool threadsAreBusy()
        {
            if(workerNodeOneInternal.isRunning || worketNodeTwoInternal.isRunning)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
