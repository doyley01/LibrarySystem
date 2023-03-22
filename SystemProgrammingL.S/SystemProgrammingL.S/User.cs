using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

[Serializable]
class User
{
    protected int id; //declare variables for customer
    protected string firstname;
    protected string lastname;
    protected DateTime dateOfBirth;
    protected string password;
  


    //constructor
    public User(int userid, string firstname, string lastname, DateTime dob, string password)
    {
        this.firstname = firstname;
        this.lastname = lastname;
        this.id = userid;
        this.dateOfBirth = dob;
        this.password = password;
    }

    public User()
    {
        
    }

    Colours colours = new Colours();


    //Getters



    public int GetID()
    {
        return id;
    }

    public DateTime GetDob()
    {
        return dateOfBirth;
    }

    public string GetFirstName()
    {
        return firstname;
    }

    public string GetLastName()
    {
        return lastname;
    }

    public int GetUserID()
    {
        return id;
    }

    public string GetPassword()
    {
        return password;
    }


    //Setters

    public void SetID(int Userid)
    {

        Userid = GetUserID();
        if (Convert.ToBoolean(Userid.ToString().Length != 8)) //convert id to string, check its length
        {
            colours.Error($"ERROR, ID must be 8 digits long, {id} is not");
             
        }
        
        

    }


    public void SetFirstName(string fn)
    {

        if (firstname.Length == 0)
        {
            colours.Error("ERROR, name cannot be null");
           
        }
        else if (Regex.IsMatch(firstname, @"^[a-zA-Z]+$"));
        {
            colours.Error("Error, name cannot contain illegal characters");
           
        }
        
        
       
    }

    public void SetLastName(string ln)
    {
        if (lastname.Length == 0)
        {
            colours.Error("ERROR, name cannot be null");

        }
        else if (Regex.IsMatch(lastname, @"^[a-zA-Z]+$")) ;
        {
            colours.Error("Error, name cannot contain illegal characters");

        }
    }

    public void SetDob(DateTime dateOfBirth)
    {
        string format = "01/01/0001";
        if(DateTime.TryParse(format, out dateOfBirth))
        {

        }
        else
        {
            colours.Error("Error, please follow the asked format");
        }
        



    }

    //display account details for user


    public string Display()
    {
        return ($"The users details are: ID {GetID} , Firstname {GetFirstName}, Lastname {GetLastName}, Date of birth {GetDob}");
    }




}