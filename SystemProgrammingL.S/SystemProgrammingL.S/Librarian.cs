using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Linq;


[Serializable]
class Librarian : User//inherit user attributes
{
    //declaring instances of classes
    Book books = new Book();
    Customer customer = new Customer(); 
    Colours colours = new Colours();

    //declaring files 
    string StaffFile = "StaffUsers.txt"; //filepath to add a new user
    string CustomerFile = "Customer.txt"; //filepath to add new customer
    string loginPath = "login.txt";

   
     List<Librarian> StaffLogins = new List<Librarian>(100);
    //declare private dictonary to serialize staff login details to seperate file
    
    List<Librarian> librarians = new(100); //create list of librarian objects
    
    //constructor
    public Librarian(int userid, string firstname, string lastname, DateTime dob, string password) : base(userid, firstname, lastname, dob, password) //derives from userclass
    {
        SetID(userid); //setters with custom validation checks 
        SetDob(dob);
        SetFirstName(firstname);
        SetLastName(lastname);
       
    }


    //empty constructor
    public Librarian()
    {
        
        
    }

    public Librarian(int id, string password)
    {
        this.id = id;
        this.password = password;
    }


    //function to save login to seperate login file
    private void SaveLogin()
    {

        List<Librarian> lib = new List<Librarian>
        {
            new Librarian(id, password)//set password into dict
        }; //create instance of libraria, include password
         //add userid, lib to dictonary
        using(FileStream fs = new FileStream(loginPath, FileMode.Create)) //usefilestream to create file, with loginpath provided
        {
            BinaryFormatter bf = new BinaryFormatter(); //create binary formatter
            bf.Serialize(fs, lib);
            fs.Close();                     //serialize the list to staff login file
        }
    }

    //function to checklogin for librarian
    private void CheckLogin()
    {
        try
        {
            using (FileStream fs = new FileStream(loginPath, FileMode.Open, FileAccess.Read)) //open file with filestream
            {
                BinaryFormatter bf = new BinaryFormatter();
                StaffLogins = bf.Deserialize(fs) as List<Librarian>; //deserialize the data in the file
                fs.Close();//close filestream
            }
        }
        catch (FileLoadException)
        {
            colours.Error("Error, the file could not be loaded");
            
        }
        
        
    }


    private void AddNewStaff()
    {
     //to add new staff member to main file
        List<Librarian> staff = new List<Librarian>
        {
            new Librarian(id, firstname, lastname, dateOfBirth, password)
        }; //create list of libarians, pass in needed values
        
        FileStream fs = File.OpenWrite(StaffFile); //use filesetream to get open/write privledges for the file 
        BinaryFormatter bf = new BinaryFormatter(); //create binary formatter
        bf.Serialize(fs, staff); //serialize the list of librarians
        fs.Close(); //close fs
    }

    private void AddNewCustomer()
    {
        List<Customer> customer = new List<Customer>
        {
            new Customer(id, firstname, lastname,  dateOfBirth, password)
        };
        FileStream fs =  File.OpenWrite(CustomerFile);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, customer);
        fs.Close();
    }


    //Functionalities for user management
    public string AddNewUser()
    {

        while (true)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Enter userID");
                 id = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter users first name");
                 firstname = Console.ReadLine();
                Console.WriteLine("Enter users last name");
                 lastname = Console.ReadLine();
                Console.WriteLine("Enter date of birth DD/MM/YYYY");
                dateOfBirth  = DateTime.Parse(Console.ReadLine());
                Console.WriteLine("Please enter password");
                Console.ResetColor();
                password = Console.ReadLine();
                User user = new User(id, firstname, lastname, dateOfBirth, password); //pass in values to the user class
                break;
          
               

            }
            catch (Exception)
            {
                string error = "Error, user could not be added";
                colours.Error(error);
            }
        }
        while (true)
        {
            string select = "Is this a customer or staff account? [C/S]";
            colours.AskQuestions(select);
            select = Console.ReadLine();
            if (select == "C")
            {
                colours.Confirmation("New customer has been added to the system");
         
                AddNewCustomer(); //call the add new customer function
                LibrarianOptions();
                break;
                

            }
            else if (select == "S")
            {
                colours.Confirmation("New Staff member has been added");
        
                AddNewStaff(); //call the add new staff function
                SaveLogin(); //call the savestafflogin function
                LibrarianOptions();
                break;

            }
            else if (select == "Q")
            {
                break;
                LibrarianOptions();

            }
        }

        return Console.ReadLine().ToLower();
    }

   
    //to view staff members
    private void ViewStaffMembers()
    {
        LoadStaffFile(); //load the file
        Console.WriteLine("Which staff member would you like to view [Userid]");
        int s = (int)Convert.ToInt64(Console.ReadLine());
        Parallel.ForEach(librarians, lib =>
        {
            if (lib.GetID() == s) //if id exists then displa the values
            {
                Console.WriteLine("Userid: " + lib.id);
                Console.WriteLine("Firstname: " + lib.firstname);
                Console.WriteLine("Lastname:" + lib.lastname);
                Console.WriteLine("DOB: " + lib.dateOfBirth);
                Console.WriteLine("What would you like to do? [E] to edit this user, [H] to return home");
                string choice = Console.ReadLine();
                if (choice == "E")
                {

                }
                else if (choice == "H")
                {
                    LibrarianOptions();
                }
            }
            else
            {
                Console.WriteLine($"Error, user with id {id} not found"); //if does not exist
            }
        });
        


    }
    
    //load the file
    private void LoadStaffFile()
    {
        FileStream fs = File.OpenRead(StaffFile); //fileestream to open the file 
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            librarians = bf.Deserialize(fs) as List<Librarian>; //deserialize the staff file into list of librarian objects

        }
        catch(FileLoadException)
        {
            Console.WriteLine("Could not load the file"); 
        }
        finally
        {
            fs.Close();
        }
    }

    string RemoveDecision()
    {
        while (true)
        {
            Console.WriteLine("Would you like to remove a user from staff or customer accounts? [S/C]");
            string choice = Console.ReadLine();
            if (choice == "S") RemoveStaff();
            else if(choice == "C") RemoveCustomer();
            break;

        }
        return Console.ReadLine();
    }
   
    //to remove user from the system
    private void RemoveStaff()
    {
        LoadStaffFile(); //load the file
        colours.AskQuestions("Which staff member would you like to remove? [UserID]");
        id = Convert.ToInt32(Console.ReadLine());

        for (int i = 0; i < librarians.Count; i++)
        {
            if (librarians[i].GetUserID() == id)
            {
                librarians.Remove(librarians[i]);
            }
            else
            {
                colours.Error("ERROR, USERID DOES NOT EXIST");

            }
        }
        librarians.Add(this);
        using (FileStream fs = new FileStream(loginPath, FileMode.Create)) //usefilestream to create file, with loginpath provided
        {
            BinaryFormatter bf = new BinaryFormatter(); //create binary formatter
            bf.Serialize(fs, librarians);
            fs.Close();                     //serialize the dictonary to staff login file
        }
        colours.Confirmation("Member has been removed, [H] to return home, [Q] to quit");
        string choice = Console.ReadLine();
        if(choice == "H")
        {
            LibrarianOptions();
        }
        else if(choice == "Q")
        {
            Exit();
        }
        
       
        

      
    }

    public void RemoveCustomer()
    {
        customer.LoadFile();
        List<Customer> custlist = new();
        Console.WriteLine("Which customer would you like to remove? [UserID]");
        id = Convert.ToInt32(Console.ReadLine());

        for (int i = 0; i < custlist.Count; i++) //count number in librarians
        {
            if (custlist[i].GetUserID() == id) //if librarian [i] value is equal to id
            {
                custlist.Remove(custlist[i]); //then remove it 
            }
        }
        custlist.Add(customer);
        using (FileStream fs = new FileStream(CustomerFile, FileMode.Create)) //usefilestream to create file, with loginpath provided
        {
            BinaryFormatter bf = new BinaryFormatter(); //create binary formatter
            bf.Serialize(fs, custlist);
            fs.Close();                     //serialize the dictonary to staff login file
        }



    }


    //function to ask for user decision
    string UserDecision()
    {
        while (true)
        {
            colours.AskQuestions("Would you like to view staff or customer accounts? [S/C]");
            string choice = Console.ReadLine();
            if (choice == "S")
            {
                ViewStaffMembers(); //call view staff member function 
                break;
            }
            else if (choice == "C")
            {
                customer.ViewCustomers(); //call view customer function
                break;
            }
            else
            {
                colours.Error($"Error {choice} is not a recognised command");
                
            }
        }
        return Console.ReadLine();
    }


    
    
    //User interaction messages

     string OpeningMessage()
    {
       
        string welcome = "WELCOME TO THE LIBRARY MANAGEMENT SYSTEM";
        colours.Titles(welcome);
        Console.WriteLine();
        string question = "Are you a staff or customer? [S/C]";
        colours.AskQuestions(question); 
        return Console.ReadLine();
        

    }

    public string LibrarianOptions()
    {

        Console.Clear();
        Console.WriteLine();
        string title = "Librarian System Options";
        colours.Titles(title);
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Option [A] - Manage System Users");
        Console.WriteLine("Option [B] - Manage Book Inventory");
        Console.WriteLine("Option [C] - Logout of system");
        Console.WriteLine();
        Console.ResetColor();
        string response = "What is your response?";
        colours.AskQuestions(response);
        string choice = Console.ReadLine();
        if (choice == "A") ManageUsers(); //call manage user function 
        else if (choice == "B") ManageBooks(); //call manage book function
        else if (choice == "C") Exit();
        
        return Console.ReadLine();
        

    }

     void Exit()
    {
        Console.Clear();
        Console.WriteLine($"Thankyou for using the system");
        Environment.Exit(-1);
    }

    string ManageUsers()
    {
        Console.Clear();
        colours.Titles("--User Management--");

        Console.WriteLine("---------------------------------");
        colours.Answers("[Add] - To Add a new user to system");
        colours.Answers("[Show] - To view current users");
        colours.Answers("[Delete] - To remove user from system");
        Console.WriteLine("---------------------------------");
        Console.WriteLine();
        colours.AskQuestions("What would you like to do?");

        string choice = Console.ReadLine();
        if (choice == "Add") AddNewUser(); //call add user function
        else if (choice == "Show") UserDecision(); //call decision function 
        else if (choice == "Delete") RemoveDecision(); //call remove function
        return Console.ReadLine();
    }

    public void StartSystem()
    {
        while (true)
        {
            string option = OpeningMessage(); //opening messsage function 
            if (option == "Q") break;
            else if (option == "S") StaffMemberOption(); //call staff member options
            else if (option == "C") customer.CheckLogin(); //call customer options

            Console.ReadLine(); //user must press enter


        }
    }

    //Ask for password
    string StaffMemberOption()
    {
        CheckLogin(); //load loginfile
        while (true)
        {
            try
            {
                colours.Answers("Please enter userid");
                id = Convert.ToInt32(Console.ReadLine());
                colours.Answers("Please enter password");
                password = Console.ReadLine();
                
            }
            catch
            {
                colours.Error("Error, ID must be a number");
                continue;  
                
            }
            


            foreach (Librarian user in StaffLogins)
            {
                if (user.GetID() == id  && user.GetPassword() == password)
                {
                    LibrarianOptions();
                    break;
                }
                else
                {
                
                    colours.Error("WARNING, INCORRECT USERNAME OR PASSWORD");
                }
                
            }

           

        }
        return Console.ReadLine();
    }

    string ManageBooks()
    {
        colours.Titles("--Book Management--");
        Console.WriteLine("-------------------");
        colours.Answers("[Add] - To Add a new book to system");
        colours.Answers("[Display] - To view all books");
        colours.Answers("[Remove] - To remove a book from system");
        string choice = Console.ReadLine();
        if (choice == "Add") books.AddBooks(); //addbook function 
        else if (choice == "Display") books.ViewBooks(); //view book function 
        else if (choice == "Remove") books.RemoveBook(); //remove book function 
        return Console.ReadLine();
    }
    


    
}