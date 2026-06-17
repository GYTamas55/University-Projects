
import java.util.*;
import java.util.concurrent.*;
import java.util.concurrent.atomic.*;
import java.util.concurrent.locks.*;

public class Database {
    private final static int USER_COUNT = 10;

    private final static int MAX_CONNECTION_COUNT = 5;
    private final static int MAX_REQUEST_COUNT = 5;

    private final Map<UUID, String> connections = new HashMap<>();
    private final Map<String, Integer> storage = new HashMap<>();

    private final ReadWriteLock rw_Lock = new ReentrantReadWriteLock();
    private final BlockingQueue<Runnable> task_Queue = new ArrayBlockingQueue<>(MAX_REQUEST_COUNT);
    public static void main(String[] args) throws InterruptedException {
        
        log("Initializee");
        //Szálak indítása
        final Database database = new Database();
        List<Thread> user_Threads = new ArrayList<>();
        AtomicBoolean is_Running = new AtomicBoolean(true);
        for(int i = 0; i < USER_COUNT; i++)
        {
            User us = new User(database, "User" + i, is_Running);
            Thread t = new Thread(us);
            user_Threads.add(t);
            t.start();
        };
        //Worker indítása
        DatabaseWorker worker = new DatabaseWorker(database.task_Queue, is_Running);
        Thread w_Thread = new Thread(worker);
        w_Thread.start();

        //Várakozás, leállítás
        Thread.sleep(5000);
        is_Running.set(false);
        for (Thread t : user_Threads){
            t.join(1000);
        }
        w_Thread.join(1000);
        
        //Vége
        if(database.task_Queue.isEmpty()){
            log("Task Queue is empty");
        }
        log("DatabasWorker terminated");
        log("Shut down");
    }

    public synchronized UUID connect(String username) throws InterruptedException {
        
        log("Connection request from " + username);

        while(connections.size() >= MAX_CONNECTION_COUNT){
            log(username + " waiting -> No room for new user");
            wait();
        }


            UUID token = UUID.randomUUID();
            
            connections.put(token, username);

            log(username + " connected with token: " + token + " connections: " + connections.size());


        return token;
    }

    public synchronized void disconnect(UUID token) {
           
        String username = connections.remove(token);

        if (username != null)
        {
            log(username + " disconnected");
            notifyAll();
        }
    }

    public  Future<Integer> query(UUID token, String key) throws InterruptedException {          
        if (!connections.containsKey(token))
        {
            return null;
        }
        String username = connections.get(token);

        CompletableFuture<Integer> f_int = new CompletableFuture<Integer>();

        Runnable task = () -> 
        {
            rw_Lock.readLock().lock();
            try
            {
                Integer data = storage.get(key);
                if(!(data != null))
                {
                    data = 0;
                }
                
                log(username + " quaried value " + key + " = " + data);
                f_int.complete(data);
            }
            finally
            {
                rw_Lock.readLock().unlock();
            }
        };

        task_Queue.put(task);
        return f_int;
    }

    public  void update(UUID token, String key, int value) throws InterruptedException {

           if (!connections.containsKey(token))
            {
                return;
            }
            String username = connections.get(token);

            Runnable task = () -> 
            {
                rw_Lock.writeLock().lock();
                try
                {
                    storage.put(key, value);
                    log(username + " updated value: " + key + " = " + value);
                }
                finally
                {
                    rw_Lock.writeLock().unlock();
                }
            };

            task_Queue.put(task);
        }
        
    

    private static void log(String message) {
        System.out.println("[system] " + message);
    }
}