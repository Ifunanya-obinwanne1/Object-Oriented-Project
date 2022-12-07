namespace PackageManagement;
using System.Data;
using MySql.Data.MySqlClient;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


using Azure;
using Azure.Communication.Email;
using Azure.Communication.Email.Models;

class BusinessLogic
{
   
    static async Task Main(string[] args)
    {
        bool _continue = true;
        User user;
        GuiTier appGUI = new GuiTier();
        DataTier database = new DataTier();

        // start GUI
        user = appGUI.Login();

       
        if (database.LoginCheck(user)){

            while(_continue){
                int option  = appGUI.Dashboard(user);
                switch(option)
                {
                    // Add a New Package
                    case 1:
                    while (_continue)
                    {
                    Console.WriteLine("Input Resident name");
                    string full_name = Console.ReadLine();
                    Console.WriteLine("Input unit number");
                    int unit_number = Convert.ToInt16(Console.ReadLine());
                    if(database.PackageCheck(full_name, unit_number))
                    {
                        Console.WriteLine("--------Search Valid Resident and Add package to Pending Area--------");
                        Console.WriteLine("Input the posting service(Fedex, USPS, UPS, etc).");
                        string posting_agency = Console.ReadLine();
                        database.AddToPendingArea(unit_number, full_name, posting_agency);
                        database.AddToHistory(unit_number, full_name, posting_agency);
                        Console.WriteLine("---------Package Added!--------");
                        DataTable tableAllPending = database.ShowAllPending(user);
                        if (tableAllPending != null)
                        {
                            appGUI.Display(tableAllPending);
                        }
                        // send email notification
                        string serviceConnectionString =  "endpoint=https://ikweek10communication.communication.azure.com/;accesskey=lwjq8YVtecbLzFtNsqyVB06p+KnmNMmpUlKqpxZ0sU4V1YfeTYTr/n+qFpzFxQ4oE6Xm4DawrpEod8DppjzlPg==";
                        EmailClient emailClient = new EmailClient(serviceConnectionString);
                        var subject = "We Have a Delivery For You!";
                        var emailContent = new EmailContent(subject);
        // use Multiline String @ to design html content
        emailContent.Html= @"
                    <html>
                        <body>
                            <h1 style=color:red>Hello Resident</h1>
                            <h4>Your Package(s) have arrived and are ready to be picked up at the leasing office.</h4>
                            <p>See you soon!!</p>
                        </body>
                    </html>";


        // send email from email service on Azure's domain
        var sender = "DoNotReply@5999b487-2c15-4744-a7f9-afaf16d3bb96.azurecomm.net";

        Console.WriteLine("Please input an email address: ");
        string inputEmail = database.ResidentEmail(unit_number, full_name);
        var emailRecipients = new EmailRecipients(new List<EmailAddress> {
            new EmailAddress(inputEmail) { DisplayName = "Testing" },
        });

        var emailMessage = new EmailMessage(sender, emailContent, emailRecipients);

        try
        {
            SendEmailResult sendEmailResult = emailClient.Send(emailMessage);

            string messageId = sendEmailResult.MessageId;
            if (!string.IsNullOrEmpty(messageId))
            {
                Console.WriteLine($"Email sent, MessageId = {messageId}");
            }
            else
            {
                Console.WriteLine($"Failed to send email.");
                return;
            }

            // the wait time for email status. 
            var cancellationToken = new CancellationTokenSource(TimeSpan.FromMinutes(2));
            do
            {
                SendStatusResult sendStatus = emailClient.GetSendStatus(messageId);
                Console.WriteLine($"Send mail status for MessageId : <{messageId}>, Status: [{sendStatus.Status}]");

                if (sendStatus.Status != SendStatus.Queued)
                {
                    break;
                }
                await Task.Delay(TimeSpan.FromSeconds(10));
               
            } while (!cancellationToken.IsCancellationRequested);

            if (cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine($"Looks like we timed out for email");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in sending email, {ex}");
        }
                    }
        else
        {
            Console.WriteLine("---------Owner Not a Resident Add Package to Unknown Area------------");
            Console.WriteLine("Input Owner Name: ");
            string packageOwner = Console.ReadLine();
            Console.WriteLine("Input the posting service");
            string postServiceAgency = Console.ReadLine();
            Console.WriteLine("Input delivery date:");
            string deliveryDate = Console.ReadLine();
            database.AddToUknown(packageOwner, postServiceAgency,deliveryDate);
            DataTable showunknow = database.ShowUnknow(user);
            if(showunknow != null){
                appGUI.DisplayUnknown(showunknow);
            }
        }
        Console.WriteLine("Please input T if you want to add more packages (T or F):");
        string answer = Console.ReadLine();
        if (answer != "T")
        {
            break;
        }

                    }      
         break;
        
        
            // Pickup Package
            case 2:
            while (_continue)
            {
             Console.WriteLine("Input Owner name");
                    string full_name = Console.ReadLine();
                    Console.WriteLine("Input unit number");
                    int unit_number = Convert.ToInt16(Console.ReadLine());
                    if(database.PackageCheck(full_name, unit_number))
                    {
                      Console.WriteLine("---------Pending Package Table before Pick up--------");
                        DataTable tablePending = database.ShowAllPending(user);
                        if (tablePending != null)
                        {
                            appGUI.Display(tablePending);
                        }
                        database.DeleteFromPending(unit_number, full_name);
                        Console.WriteLine("----------Successful Pickup----------------");   
                    }
                    else{
                        Console.WriteLine("Wrong Information, Try again");
                    }
                Console.WriteLine("Please input T if you want to pick up more packages (T or F):");
                string answer = Console.ReadLine();
                if (answer != "T")
                {
                    break;
                }      
            }
            break;
            // Retrieve package history
            case 3:
            Console.WriteLine("----------Package History----------------");

            DataTable tableRecords = database.ShowHistory(user);
            if (tableRecords != null)
            {
                appGUI.Display(tableRecords);
            }


            break;
            case 4:
            // Log Out
            _continue = false;
            Console.WriteLine("Log Out, Goodbye.");
            break;
          
           // default: wrong input
                    default:
                        Console.WriteLine("Wrong Input");
                        break;
                }

            }
        }
        else{
                Console.WriteLine("Login Failed, Goodbye.");
        }        
    }    
}
