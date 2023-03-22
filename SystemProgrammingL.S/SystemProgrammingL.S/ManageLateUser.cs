class ManageLaterUser
{
    HashSet<string> users = new HashSet<string>();//use hashset so duplicate values can't be added 
    string file = "lateUsers.txt"; //file name

    public ManageLaterUser()
    {
        try
        {
            foreach (string line in File.ReadLines("lateUsers.txt")) //read the lateusers file
            {
                users.Add((line));
            }
        }
        catch (FileNotFoundException) //if file does not exist, offer user to make one
        {
            Console.WriteLine("The file has not been found, would you like to create one instead? [Y/N]");
            string answer = Console.ReadLine();

            if (answer == "Y")
            {
                using (FileStream fs = File.Create(file)) ; //create file if answer yes
                Console.WriteLine("File has been created succesfully");
                return;

            }
            else if (answer == "N")
            {
                Console.WriteLine("No worries");
                return;


            }

        }
        catch (IOException e) //exception if file could not open
        {
            Console.WriteLine($"The file could not be opened:  '{e}'");
        }
    }

    public void showOverdueCustomers()
    {
        foreach (string item in users)
        {

            Console.Write(item);
        }
    }
    public void AddUser(string id)
    {
        try
        {
            users.Add(id.ToLower());
            
        }
        catch(FileNotFoundException ex)
        {
            Console.WriteLine("File not found {0}", ex.Message);
        }
        finally
        {
            Console.WriteLine("User has been added to latefile");
            UpdateFile();
        }
       
    }
    public void RemoveUser(string r)
    {
        try
        {
            users.Remove(r);
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine("File not found {0}", ex.Message);
        }
        finally
        {
            Console.WriteLine("User has been removed");
            UpdateFile();
        }
    }
    public void UpdateFile()
    {
        File.AppendAllLines(file, users);
    }
}