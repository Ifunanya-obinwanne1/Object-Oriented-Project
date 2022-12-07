namespace PackageManagement;
using System.Data;
using MySql.Data.MySqlClient;
class GuiTier{
    User user = new User();
    DataTier database = new DataTier();

    // print login page
    public User Login(){
        Console.WriteLine("------Welcome to Package Management System------");
        Console.WriteLine("Please input userName (staff_username): ");
        user.userName = Console.ReadLine();
        Console.WriteLine("Please input password: ");
        user.userPassword = Console.ReadLine();
        return user;
    }
    // print Dashboard after user logs in successfully
    public int Dashboard(User user){
        DateTime localDate = DateTime.Now;
        Console.WriteLine("---------------Dashboard-------------------");
        Console.WriteLine($"Hello: {user.userName}; Date/Time: {localDate.ToString()}");
        Console.WriteLine("Please select an option to continue:");
        Console.WriteLine("1. Add Package");
        Console.WriteLine("2. Package Status");
        Console.WriteLine("3. Check Package History");
        Console.WriteLine("4. Log Out");
        int option = Convert.ToInt16(Console.ReadLine());
        return option;
    }

    // show record history
    public void Display(DataTable tableRecords){
        Console.WriteLine("---------------Table-------------------");
        foreach(DataRow row in tableRecords.Rows){
           Console.WriteLine($"UnitNumber: {row["unit_number"]} \t Resident_Name: {row["full_name"]} \t Agency:{row["posting_agency"]}");
        }
    }
    public void DisplayUnknown(DataTable tableRecords){
        Console.WriteLine("---------------Table-------------------");
        foreach(DataRow row in tableRecords.Rows){
           Console.WriteLine($"owner_name: {row["owner_name"]} \t Posting_agency: {row["posting_agency"]} \t Delivery_date:{row["delivery_date"]}");
}
    }
}
