<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;

public class Handler : IHttpHandler {

    basicFunctions B_functions = new basicFunctions();



    public void ProcessRequest (HttpContext context) {

        context.Response.ContentType = "text/html";
        context.Response.Write(context.Request.QueryString["bid"]);

        var the_co_name= context.Request.QueryString["cn"];
        var the_new_amt = Convert.ToDecimal(context.Request.QueryString["amt"]);
        var the_new_date = Convert.ToDateTime(context.Request.QueryString["dd"]);

        
       // B_functions.reset_bill(the_co_name,the_new_amt,the_new_date);

        
        try
            {
	   
            B_functions.reset_bill(the_co_name,the_new_amt,the_new_date);
            }
        catch (Exception ex)
            {
	            context.Response.Write(ex.Message);
            }
       
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}


