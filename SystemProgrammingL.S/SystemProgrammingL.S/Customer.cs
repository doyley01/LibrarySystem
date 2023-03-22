using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using System.Text;

[Serializable]
class Customer: User
{
    string Password = "Hello";
    string file = "Customer.txt";

    string GetPassword()
    {
        return Password;
    }

    Borrowing borrow = new Borrowing();
    Colours colours = new Colours();
   
    List<Customer> custlist = new();
    List<Customer> customers = new();

    public Customer(int userid, string firstname, string lastname, DateTime dob, string password) : base(userid, firstname, lastname, dob,  password)
    {
        this.firstname = firstname;
        this.lastname = lastname;
        this.dateOfBirth = dob;
        Password = password;
        this.id = userid;
    }

    public Customer()
    {

    }

    public Customer(int id, string password)
    {
        this.id = id;
        Password = password;
    }

    public void ViewCustomers()
    {
        Librarian lib = new Librarian();
        FileStream fs = File.OpenRead(file);
        try
        {

            BinaryFormatter bf = new BinaryFormatter();
            customers = bf.Deserialize(fs) as List<Customer>;



        }
        catch
        {
            colours.Error("Could not load the file");
          
        }
        finally
        {
            fs.Close();
        }
        colours.AskQuestions("Which customer would you like to view [Userid]");
        int s = (int)Convert.ToInt64(Console.ReadLine());
        Parallel.ForEach(customers, c =>
        {
            if (c.GetID() == s)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Userid: " + c.id);
                Console.WriteLine("Firstname: " + c.firstname);
                Console.WriteLine("Lastname:" + c.lastname);
                Console.WriteLine("DOB: " + c.dateOfBirth);
                Console.ResetColor();
            }
            else
            {
                colours.Error("Error, user not found");
                
            }
        });
        colours.AskQuestions("What would you like to do? [R] to remove customer, [H] for home");
        string choice = Console.ReadLine();
        if (choice == "R") lib.RemoveCustomer();
        else if (choice == "H") lib.LibrarianOptions();
        
    }

    public void CheckLogin()
    {
        LoadFile();
        while (true)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Please enter your userID");
                id = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Please enter your password");
                Password = Console.ReadLine();
                Console.ResetColor();
                foreach (Customer cust in custlist)
                {
                    if (cust.GetPassword() == Password && cust.GetID() == id)
                    {
                        colours.AskQuestions($"Welcome {cust.firstname} what would you like to do?");
                        Console.WriteLine("");
                        CustomerOptions();
                        break;
                        
                        
                    }
                    else
                    {
                        colours.Error("Error, userid or password is incorrrect");
                        
                    }
                }


            }
            catch (Exception)
            {
                colours.Error($"Error UserID must be a number");
                continue;
                
            }
            

        }
       









    }
    

    public void LoadFile()
    { 
       
        try
        {
            FileStream fs = File.OpenRead(file);
            BinaryFormatter bf = new BinaryFormatter();
            custlist = bf.Deserialize(fs) as List<Customer>;
            fs.Close();
        }
        catch
        {
            Console.WriteLine("Could not load the file");
        }
    }

   public string CustomerOptions()
    {
        
        Console.WriteLine("");
        Console.WriteLine("Option [A] to browse books");
        Console.WriteLine("Option [B] to return books");
        Console.WriteLine("Option [C] sign out of system");
        string choice = Console.ReadLine();
        if (choice == "A") borrow.Browse();
        else if (choice == "B") ;
        else if (choice == "C") Exit();
        return Console.ReadLine();
    }

    void Exit()
    {
        Console.Clear();
        Console.WriteLine($"Thankyou {firstname} for using the system");
        Environment.Exit(-1);
    }




}