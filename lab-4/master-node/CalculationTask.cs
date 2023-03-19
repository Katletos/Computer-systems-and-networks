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

        if (queue.TryPeek(out var tmp))
        {
            newTask.number = tmp.number + 1;
        }
        else
        {
            newTask.number = 1;
        }
        
        newTask.taskId = Guid.NewGuid();
        newTask.taskState = false;
        newTask.result = false;
        
        return newTask;
    }
    
    void IsPrime()
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