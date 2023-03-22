namespace master_node;

public class CalculationTask
{
    public Guid taskId;
    public bool taskState;
    public ulong number;
    public bool result;

    public static CalculationTask GetNextTask(Queue<CalculationTask> queue)
    {
        CalculationTask newTask = new CalculationTask();
        
        if (queue.Count == 0)
        {
            newTask.number = 1;
        }
        
        newTask.taskId = Guid.NewGuid();
        newTask.taskState = false;
        newTask.number = queue.Peek().number + 1;
        newTask.result = false;
        
        return newTask;
    }
    
    public void IsPrime()
    {
        for (uint i = 0; i < Math.Round(Math.Sqrt(number)); ++i)
        {
            if (number % i == 0)
            {
                result = true;
                taskState = true;
                break;
            }
        }

        result = false;
        taskState = true;
    }
}