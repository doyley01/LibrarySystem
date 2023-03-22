using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Diagnostics.SymbolStore;
using System.Drawing;
using System.Text;


[Serializable]
class Borrowing:Book
{
    
    private string file = "Books.txt";
    private string BorrowingFile = "Borrowing.txt";
    private string toBorrow;


    private StringBuilder filebuilder;
    private StringBuilder linebuilder;
    private string line;
    private int match;
    private int linetochange;

    Colours colours = new Colours();
    
   
    public Borrowing()
    {

    }

  

    //below are all three attempts I used to try remove/write to a new file, the idea was to remove it from one list, copy new value to new list, then write to new file
    //this function removes all lines from file and appends it to a new one, could not get it to remove just the one line i am searching
    
    public void AddToBorrowingFile()
    {
        Console.WriteLine("Which book would you like to borrow? [ISBN]");
        string borrow = Console.ReadLine();
        filebuilder = new StringBuilder(); //create instance of string builders
        linebuilder = new StringBuilder();
        using(StreamReader reader = new StreamReader(file)) //read the file
        {
            while (reader.Peek() >= 10)
            {
                line = reader.ReadLine(); //set line to reader
                match = line.IndexOf(borrow); //set match to index of isbn

                if (match == -1)
                {
                    filebuilder.AppendLine(line); //append using string builder
                }
                else
                {
                    linetochange++; //increase lineschanged
                    linebuilder.Clear(); //clear the string builder
                    linebuilder.Append(line); //append the line
                    linebuilder.Remove(match, borrow.Length); //remove 
                    linebuilder.Insert(0, borrow); //insert into index 0, insert borrow string
                    linebuilder.AppendLine(linebuilder.ToString());    
                }
            }
            reader.Close(); //close the reader
        }
        if(linetochange > 0) //if greater than 0
        {
            using(StreamWriter sw = new StreamWriter(BorrowingFile)) //write the items to the borrowing file
            {
                sw.Write(filebuilder.ToString()); //write using file stringbuilder
                sw.Close();
            }
        }
        else
        {
            colours.Error("ERROR: Could not add book to your account, please try again");
        }

    }

    
   
    //this function succesfully removes a line from the book file, adds the line to a new list.
    //Writes the new list to a seperate file, it succesfully removes from first file but does not duplicate list
    public void Borrow()
    {
        var list = new List<string>();

  
        string line = null;
        colours.AskQuestions("Which book would you like to borrow? [ISBN]");
        string borrow = Console.ReadLine();
        using(StreamReader sr = new StreamReader(file)) //read file contents
        {
            using(StreamWriter sw = new StreamWriter(file)) //write to file
            {
                while((line == sr.ReadLine()) != null) //while line is null, read the line when it is not null
                {
                    if(string.Compare(line, borrow) == 0) //compare null line with line containing search
                    {
                        continue;
                        list.Add(line); //add the line to new list
                        sw.WriteLine(line); //write back to file without line
                        
                    }
                    sw.Close();
                    break;
                }
            }
            using (StreamWriter sw2 = new StreamWriter(BorrowingFile))
            {
                sw2.Write(list);
            }
            sr.Close();
            
        }
    }

    public void ReturnBook()
    {
        User user = new User();
        List<Book> list = new();
            
        user.GetID();
        Console.WriteLine("Which book would you like to return?");
       

        
    }

    private void BorrowBook()
    {
        List<Borrowing> bookList = new(); //declare booklist
        List<Borrowing> newList = new(); //new list
        
        Console.WriteLine("Which book would you like to borrow? [ISBN]");
        toBorrow = Console.ReadLine();

        for (int i = 0; i < bookList.Count; i++)
        {
            if (bookList[i].GetBorrow() == toBorrow) //if contains to borrow
            {
                bookList.Remove(bookList[i]); //then remove it
            }
            else
            {
                colours.Error("Error, book does not exist");
            }
            newList.Add(bookList[i]); //add the remove to new list
            using(FileStream fs = new FileStream(BorrowingFile, FileMode.Append, FileAccess.Write)) //filestream to open file/provide functionality
            {
                fs.Close();
                StreamWriter sw = new StreamWriter(BorrowingFile); //write to file
                sw.WriteLine(newList.ToString()); //write the new list to borrow file
            
                sw.Close();
            }
        }
        bookList.Add(this); //add this to booklist
        using (FileStream fs = new FileStream(file,  FileMode.Append, FileAccess.Write))
        {
            fs.Close();
            StreamWriter sw = new StreamWriter(file);
            sw.WriteLine(bookList.ToString());
            sw.Close();
        }

    }

   


    //browse the books
    public void Browse()
    {
        Customer customer = new();
        var list = File.ReadLines(file)
            .Select(x => x.Trim()) //trim words
            .Where(x => !string.IsNullOrWhiteSpace(x)) //remove empty space
            .Select(x => x.ToUpper()) //conCCvert to lowercase  
            .ToList(); // add to list

        foreach (var book in list) //loop over
        {
            Console.WriteLine(book.ToLower());
        }
        colours.AskQuestions("Would you like to borrow any books? [Y/N]");
        string choice = Console.ReadLine();
        if(choice == "Y") Borrow();   
        else if(choice == "N")
        {
            customer.CustomerOptions();
        }
        else
        {
            colours.Error("Error, unrecognised command");
            Browse();
            
        }
    }

    public string GetBorrow()
    {
        return toBorrow;
    }
}