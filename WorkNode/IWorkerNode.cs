namespace WorkNode.WorkNode
{
    public interface IWorkerNode
    {
        OutputObject Calculate(string jobId, string buffer, int interations);
        void GetItemsFromQueue();
        bool CalculateAndPost(InputObject currentItem);
    }
}
