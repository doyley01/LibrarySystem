
Librarian lib = new Librarian();
ManageLaterUser late = new ManageLaterUser();


//Args management
if (args.Length == 0)
{
    Console.WriteLine();


}
else if (args[0] == "v")
{
    Console.WriteLine("Version number 23.11.2022");
}
else if (args[0] == "h")
{
    Console.WriteLine("Usage = [Add + Username], [Remove + Username]. [View]");
}
else if (args[0] == "View") late.showOverdueCustomers();
else if (args[0] == "Add") late.AddUser(args[1]);
else if (args[0] == "Remove") late.RemoveUser(args[1]);
else
{
    Console.WriteLine("Unknown command, please try again or h for help");
}

lib.StartSystem();





