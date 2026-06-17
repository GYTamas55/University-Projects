
import java.util.concurrent.*;
import java.util.concurrent.atomic.*;

public class DatabaseWorker implements Runnable{
    private final BlockingQueue<Runnable> task_Queue;
    private final AtomicBoolean is_Running;

    public DatabaseWorker(BlockingQueue<Runnable> task_Queue, AtomicBoolean is_Running) {
        this.task_Queue = task_Queue;
        this.is_Running = is_Running;
    }

    @Override
    public void run() {
        while(is_Running.get() || !task_Queue.isEmpty())
            {
                try
                {
                    Runnable task = null;

                    if(!task_Queue.isEmpty())
                    {
                        task = task_Queue.take();        
                    }

                    if(task != null)
                    {
                        task.run();
                    }
                }
                catch(InterruptedException ex)
                {
                    Thread.currentThread().interrupt();
                    break;
                }
            }

    }
}