using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Handles all of the server-side operations for the Organizer website.
/// </summary>
public class basicFunctions
{
    OrganizerEntities org_DB = new OrganizerEntities();

    public string bill_list_html { get; set; }

    public string bill_details_html { get; set; }

    public string draw_bill_flags(string co_name, DateTime due_date, decimal amount_due) {

        
        TimeSpan days_til = (due_date - DateTime.Today);
        string display_days_til = days_til.ToString("%d");

        string alert_class = "";

        string flag_table = "";

        if (days_til.TotalDays <= 0)
        {
            alert_class = "late";
        }

        if ((days_til.TotalDays > 0) && (days_til.TotalDays <= 7))
        {
            alert_class = "imminent";
        }

        if ((days_til.TotalDays > 7) && (days_til.TotalDays <= 14))
        {
            alert_class = "twowks";
        }

        if ((days_til.TotalDays > 14) && (days_til.TotalDays <= 30))
        {
            alert_class = "threewks";
        }
        if (days_til.TotalDays > 30)
        {
            alert_class = "future";
        }
      

        flag_table += "<div class=\"bill_flag " + alert_class + "\" id=\"" + co_name + "\" data-billamt=\"" + amount_due + "\">" +
                     "  <div class=\"bill_row\">" +
                     "      <div class=\"bill_cell co_name\" >" + co_name + "</div>" +
                     "      <div class=\"bill_cell due_date\">" + display_days_til + "</div>" +
                     "      <div class=\"bill_cell amount_due\">" + amount_due.ToString("c2") + "</div>" +
                     "  </div>" +
                     "</div>";

    return flag_table;

    }

    public void displayBills() {

        string bill_list_disp = "";
        string details_list = "";
        
        var query = from it in org_DB.bills
                    orderby  it.date_due 
                    select it;

        foreach (bill bill in query)
        {

            string bill_due_date =  ((DateTime) bill.date_due).ToString("m");
            string bill_amt_due = ((decimal)bill.amount_due).ToString("c2");

            bill_list_disp += draw_bill_flags(bill.co_name, (DateTime)bill.date_due, (decimal)bill.amount_due);

            
            details_list += "<div class=\"bill_info\" id=\"" + bill.co_name + "_info\">" +
                                " <div class=\"org_tbl\" style=\"width:100%;\">" +
                                "    <div class=\"org_rw\">" +
                                "         <div class=\"org_cl\">" +
                                "         <p class=\"det_co_name im_a_link\">" +
                                            bill.co_name +
                                "            <input type=\"hidden\" id=\"" + bill.co_name + "_link\" value=\"" + bill.url + "\" /> " +
                                "         </p>" +
                                "         <p>" +
                                           bill.notes +
                                "         </p>" +
                                
                                "         </div>" +
                                "         <div class=\"org_cl det_amt_due\"><p>" +
                                            bill_amt_due +
                                "         </p>" +
                                "         <p>" +
                                           bill_due_date +
                                "         </p>" +
                                "         </div>" +
                                "     </div>" +
                                " </div>" +    
                                "</div> ";
            
        }

        bill_list_html = bill_list_disp;
        bill_details_html = details_list;
        //return bill_list_disp;

    }

    public void reset_bill(string co_name, decimal new_amt, DateTime new_date) {

        var query = (from x in org_DB.bills
                    where x.co_name == co_name
                    select x).First();

        query.amount_due = new_amt;
        query.date_due = new_date;
        org_DB.SaveChanges();
    }
}