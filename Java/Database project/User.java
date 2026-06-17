import java.util.*;
import java.util.concurrent.*;
import java.util.concurrent.atomic.*;

public class User implements Runnable{
    private final static long DATABASE_ACTION_MAX_TIMEOUT_MS = 1000;
    private final static float DATABASE_ACTION_DISCONNECT_CHANCE = 0.25f;

    private final Database database;
    private final String username;

    //Leállító flag:
    private final AtomicBoolean is_Running;

    public User(Database database, String username, AtomicBoolean is_Running) {
        this.database = database;
        this.username = username;
        this.is_Running = is_Running;
    }

    @Override
    public void run() {

        //Csatlakozás része
        UUID token = null;
    
        try 
        {
            token = database.connect(username);
        }
        catch(InterruptedException ex)
        {
            Thread.currentThread().interrupt();
            return;
        }

        if(token != null)
            {
                log("Connected");
            }

        while (is_Running.get())
            {

        //Művelet része
                boolean do_Query = Math.random() < 0.5;

                if (do_Query)
                    {
                        try
                        {
                            Future<Integer> f_result = database.query(token, username);
                            if(f_result != null)
                                {
                                    Integer data = f_result.get();
                                    log("Queried value: " + data);
                                }
                            
                        }
                        catch(InterruptedException ex)
                        {
                            Thread.currentThread().interrupt();
                            break;
                        }
                        catch(Exception e)
                        {
                            log("Exception at Database, when doing querry!");
                        }
                    }
                else
                    {
                        try
                        {
                            Integer update_to = ThreadLocalRandom.current().nextInt();
                            database.update(token, username, update_to);
                            log("Updated value: " + update_to);
                        }
                        catch(InterruptedException ex)
                        {
                            Thread.currentThread().interrupt();
                            break;
                        }
                        catch(Exception e)
                        {
                            log("Exception at Database, when doing update!");
                        }
                    }

        //Lecsatlakozás része
                if(Math.random() <= DATABASE_ACTION_DISCONNECT_CHANCE)
                    {
                        break;
                    }
                

                try
                {
                    long sleep_Time = (long)(Math.random() * DATABASE_ACTION_MAX_TIMEOUT_MS);
                    Thread.sleep(sleep_Time);
                }
                catch(InterruptedException ex)
                {
                    Thread.currentThread().interrupt();
                    break;
                }
            }

        database.disconnect(token);
        log(username + " disconnected");
        
    }

    private void log(String message) {
        System.out.println("[" + username + "] " + message);
    }
}