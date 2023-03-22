using Newtonsoft.Json;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Drawing;

[Serializable]
class Book
{
    List<Book> NewBooks = new List<Book>(); //declare list of book objects
    Colours colours = new Colours();
    
    private string ISBN;
    private string Title;
    private string Publisher;
    private float price;
  

    string file = "Books.txt";
    public Book()

   

    {

    }

    public Book(string isbn, string title, string publisher, float price)
    {

        SetISBN(isbn);
        Title = title;
        Publisher = publisher;
        SetPrice(price);    
    }

    public Book(string isbn, string title)
    {
        ISBN = isbn;
        Title = title;
    }

    //function to add a new book to system
    public string AddBooks()
    {
        while (true)
        {
            Librarian lib = new Librarian();
            Console.ForegroundColor = ConsoleColor.Cyan;
            colours.AskQuestions("Please enter ISBN");
            ISBN = Console.ReadLine();
            Console.WriteLine("Please enter Title");
            Title = Console.ReadLine();
            Console.WriteLine("Please enter publisher");
            Publisher = Console.ReadLine();
            Console.WriteLine("Please enter the price");
            price = float.Parse(Console.ReadLine());
            Console.ResetColor();
            Book book = new (ISBN, Title, Publisher, price); //pass in values into book class
            if(ISBN.Length != 10) //if isbn is not equal to 10, do nothing
            {

            }
            else if(ISBN.Length == 10) //if it is equal to 10 then proceed
            {
                AddBookToFile(); //add book to the book file
                colours.Confirmation("Book been added succesfully, would you like to add another? [Y/N]");
                string c = Console.ReadLine();
                if (c == "Y")
                {
                    AddBooks();
                }
                else if (c == "N")
                {
                    lib.LibrarianOptions(); //return home
                    break;
                }
                
            }
           


        }
        
       
        return Console.ReadLine().ToLower();
    }

    //add book items to file using streamwriter
    public void AddBookToFile()
    {
        try
        {
            using (StreamWriter sw = new StreamWriter(file, true)) //writes to file, true so it does not overwrite all existing
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                sw.WriteLine("ISBN:" + ISBN + " " + "Title:" + " " + Title + " " + "Publisher:" + " " + Publisher + " " + "Price:" + " " + "£" + price); //writes the book to file using streamwriter

                sw.Close();
                Console.ResetColor();

            }

        }
        catch (Exception)
        {
            
            colours.Error("ERROR, input type is incorrect");
        }


    }


    //function to view books on system 
    public void ViewBooks()
    {
        Librarian lib = new Librarian();
        colours.AskQuestions("Would you like to view all books or search? [A] for all, [S] for search");
        string c = Console.ReadLine();
        if (c == "A")
        {
            var list = File.ReadLines(file) //load file into list
                 .Select(x => x.Trim()) //select list, trim white
                 .ToList(); // add to list

            foreach (var item in list)
            {
                Console.WriteLine(item.ToLower()); //loop over list, print to screen
            }
            colours.AskQuestions("What would you like to do? [A] to add, [R] to remove, [H] for home");
            string choice = Console.ReadLine();
            if (choice == "A") AddBooks();
            else if (choice == "R") RemoveBookFromFile();
            else if (choice == "H") lib.LibrarianOptions(); 

        }
        else if (c == "S")
        {
            SearchBook(); //call search book function, if book does not exist
        }
    }

    public void SearchBook()
    {

        colours.Answers("Please enter the ISBN number");
         //enter user id to search
        var isbn = Console.ReadLine();
        using (var sr = new StreamReader(file)) //use stream reader to read the file contents
        {
            while (!sr.EndOfStream) //when stream reader is finished reading
            {
                var line = sr.ReadLine();
                if (String.IsNullOrEmpty(line)) continue; //if doesn't exist, continue
                if (line.IndexOf(isbn, StringComparison.CurrentCultureIgnoreCase) >= 0) //determine index of searched string, if exists compare with desired input
                {
                    Console.WriteLine(line); //print the line, if string exists
                    break;
                }


            }
        }



    }

    private void RemoveBookFromFile()
    {
        string line = null;
        colours.AskQuestions("Which book would you like to remove? [ISBN]");
        string remove = Console.ReadLine();
        using (StreamReader read = new StreamReader(file)) //read file contents
        {
            using (StreamWriter writer = new StreamWriter(file)) //write to file
            {
                while ((line == read.ReadLine()) != null) //while line is null, read the line when it is not null
                {
                    if (string.Compare(line, remove) == 0) //compare null line with line containing search
                    {
                        continue;
                        writer.WriteLine(line); //write back to file without line
                    }
                }
            }
        }
    }

    //function to remove book
    public void RemoveBook()
    {

        var bookList = new List<string>();
        using (var sr = new StreamReader(file)) //use stream reader to read the file
        {
            while (!sr.EndOfStream) //once at end of stream, add contents to booklist
            {
                bookList.Add(sr.ReadLine()); //read file into the userlist
                break;
            }

        }
        colours.AskQuestions("Would you like to view all books before removing? [Y/N]");
        string answer = Console.ReadLine();
        if (answer == "Y")
        {
            foreach (var item in bookList)
            {
                Console.WriteLine(item);
            }

            RemoveBookFromFile();
            

        }
        else if (answer == "N")
        {

            RemoveBookFromFile();
           
        }

        

    }
    //setters
    private bool SetISBN(string isbn)
    {
        //checks if isbn is valid by seeing if divisible by 11, isbn is 10 digits long
        ISBN = isbn;
      
        int n = isbn.Length;
        if(n != 10) //ensure length is 10
        {
            colours.Error($"Error, length of ISBN must be 10 {ISBN} is not");
            return false;
        }
        int sum = 0;
        for (int i = 0; i < 9; i++) //check the sum of first 9 digits
        {
            int d = isbn[i] - '0';
            if (0 > d || 9 < d)
                return  false;
            sum += (d * (10 - i));
           
        }
        //check the last digit entered for isbn
        char lastdigit = isbn[9]; //10 in isbn   
        if (lastdigit != 'X' && (lastdigit < '0' || lastdigit > '9')) //check between 0-9
            return  false;

        sum += ((lastdigit == 'X') ? 10 : (lastdigit = '0')); //if last is x, add 10, else add value
        return (sum % 11 == 0); //valid is true if sum is divisible by 11
       
        
    }


   
    //set the price
    private float SetPrice(float p)
    {
        price = p; 
        if(price <= 0)
        {
            colours.Error("Error price must be greater than 0");
         
        }
        return p;
    }

   
}