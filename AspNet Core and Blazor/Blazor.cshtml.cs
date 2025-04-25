using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace feedingprogram.Pages
{
    public class ChurchesModel : PageModel
    {
        [BindProperty]
        public string sUser { get; set; }
        [BindProperty]
        public string sBranch { get; set; }
        [BindProperty]
        public string CodChurch { get; set; } = "";
        [BindProperty]
        public string NameChurch { get; set; } = "";
        [BindProperty]
        public string PastorName { get; set; }
        [BindProperty]
        public string CAddress { get; set; }
        [BindProperty]
        public string CPhone { get; set; }
        [BindProperty]
        public string CNotes { get; set; }
        [BindProperty]
        public bool CActive { get; set; } = true;
        [BindProperty]
        public string SError { get; set; }
        [BindProperty]
        public int NFed { get; set; }
        [BindProperty]
        public bool bMonday { get; set; }
        [BindProperty]
        public bool bTuesday { get; set; }
        [BindProperty]
        public bool bWednesday { get; set; }
        [BindProperty]
        public bool bThursday { get; set; }
        [BindProperty]
        public bool bFriday { get; set; }
        [BindProperty]
        public bool bSaturday { get; set; }
        [BindProperty]
        public bool bSunday { get; set; }
        [BindProperty]
        public string TMonday { get; set; }
        [BindProperty]
        public string TTuesday { get; set; }
        [BindProperty]
        public string TWednesday { get; set; }
        [BindProperty]
        public string TThursday { get; set; }
        [BindProperty]
        public string TFriday { get; set; }
        [BindProperty]
        public string TSaturday { get; set; }
        [BindProperty]
        public string TSunday { get; set; }
        [BindProperty]
        public string iCountry { get; set; } = "0";
        [BindProperty]
        public string iState { get; set; } = "0";
        [BindProperty]
        public string iCity { get; set; } = "0";
        [BindProperty]
        public int NCases { get; set; }
        [BindProperty]
        public int NProjectedMeals { get; set; }
        [BindProperty]
        public double NProjectedMonths { get; set; }
        [BindProperty]
        public int NParamPackages { get; set; }
        [BindProperty]
        public int NParamMeals { get; set; }
        [BindProperty]
        public DateTime DLastVisit { get; set; } = DateTime.Today;
        public void OnGet()
        {
            
        }

        public void OnPost()
        {
            if (Request.Form.Count > 0)
            {
                if (Request.Form["BtnFind"].Count > 0)
                {
                    var contextnav = new Microsoft.AspNetCore.Http.HttpContextAccessor();
                    contextnav.HttpContext.Response.Redirect("/EditChurch/"+CodChurch, true);
                }
                if (Request.Form["BtnSave"].Count > 0)
                {
                    SError = "";
                    using (var context = new Data.SQLContext())
                    {
                        var conn = context.Database.GetDbConnection();
                        conn.Open();
                        var command = conn.CreateCommand();
                        string query = "SELECT id_branch, id_internal_role FROM branches_roles  WHERE email_member = '" + this.User.FindFirst(ClaimTypes.Name).Value + "'";
                        command.CommandText = query;
                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            sBranch = reader.GetValue(0).ToString();

                        }
                        int iExists = 0;
                        command = conn.CreateCommand();
                        query = "SELECT MAX(id_church) + 1 FROM feedp_churches  WHERE id_branch = " + sBranch + "";
                        command.CommandText = query;
                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            CodChurch = reader.GetValue(0).ToString();

                        }
                        command = conn.CreateCommand();
                        query = "SELECT 1 FROM feedp_churches  WHERE id_branch = " + sBranch + "";
                        query = query + " AND id_church = " + CodChurch + "";
                        command.CommandText = query;
                        reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            iExists = 1;

                        }
                        
                        command = conn.CreateCommand();
                        if(iExists == 1)
                        {
                            query = "UPDATE feedp_churches SET ";
                            query = query + "txt_church = UPPER('" + NameChurch + "')";
                            query = query + ", txt_pastor = UPPER('" + PastorName + "')";
                            query = query + ", id_country = 0" + iCountry + "";
                            query = query + ", id_state = 0" + iState + "";
                            query = query + ", id_city = 0" + iCity + "";
                            query = query + ", txt_address = UPPER('" + CAddress + "')";
                            query = query + ", txt_phone = '" + CPhone + "'";
                            query = query + ", txt_notes = UPPER('" + CNotes + "')";
                            if (CActive == true)
                                query = query + ", yn_active = -1";
                            else
                                query = query + ", yn_active = 0";
                            query = query + ", meals_week = 0" + NFed + "";
                            if (bMonday == true)
                                query = query + ", yn_monday = -1";
                            else
                                query = query + ", yn_monday = 0";
                            query = query + ", time_mon = '" + TMonday + "'";
                            if (bTuesday == true)
                                query = query + ", yn_tuesday = -1";
                            else
                                query = query + ", yn_tuesday = 0";
                            query = query + ", time_tue = '" + TTuesday + "'";
                            if (bWednesday == true)
                                query = query + ", yn_wednesday = -1";
                            else
                                query = query + ", yn_wednesday = 0";
                            query = query + ", time_wed = '" + TWednesday + "'";
                            if (bThursday == true)
                                query = query + ", yn_thursday = -1";
                            else
                                query = query + ", yn_thursday = 0";
                            query = query + ", time_thu = '" + TThursday + "'";
                            if (bFriday == true)
                                query = query + ", yn_friday = -1";
                            else
                                query = query + ", yn_friday = 0";
                            query = query + ", time_fri = '" + TFriday + "'";
                            if (bSaturday == true)
                                query = query + ", yn_saturday = -1";
                            else
                                query = query + ", yn_saturday = 0";
                            query = query + ", time_sat = '" + TSaturday + "'";
                            if (bSunday == true)
                                query = query + ", yn_sunday = -1";
                            else
                                query = query + ", yn_sunday = 0";
                            query = query + ", time_sun = '" + TSunday + "'";
                            query = query + ", nb_cases = 0" + NCases + "";
                            query = query + ", param_pack = 0" + NParamPackages + "";
                            query = query + ", param_meals_xpack = 0" + NParamMeals + "";
                            query = query + " WHERE id_branch = 0" + sBranch + "";
                            query = query + " AND id_church = 0" + CodChurch + "";
                            query = query + "           UPDATE feedp_churches_adic SET ";
                            query = query + " last_visit = '" + DLastVisit.ToString("yyyyMMdd") + "'";
                            query = query + " WHERE id_branch = 0" + sBranch + "";
                            query = query + " AND id_church = 0" + CodChurch + "";

                        }
                        else
                        {
                            query = "INSERT INTO feedp_churches  SELECT " + sBranch + "";
                            query = query + ", " + CodChurch + "";
                            query = query + ", UPPER('" + NameChurch + "')";
                            query = query + ", UPPER('" + PastorName + "')";
                            query = query + ", 0" + iCountry + "";
                            query = query + ", 0" + iState + "";
                            query = query + ", 0" + iCity + "";
                            query = query + ", UPPER('" + CAddress + "')";
                            query = query + ", '" + CPhone + "'";
                            query = query + ", UPPER('" + CNotes + "')";
                            if (CActive == true)
                                query = query + ", -1";
                            else
                                query = query + ", 0";
                            query = query + ", 0" + NFed + "";
                            if (bMonday == true)
                                query = query + ", -1";
                            else
                                query = query + ", 0";
                            query = query + ", '" + TMonday + "'";
                            if (bTuesday == true)
                                query = query + ", -1";
                            else
                                query = query + ", 0";
                            query = query + ", '" + TTuesday + "'";
                            if (bWednesday == true)
                                query = query + ", -1";
                            else
                                query = query + ", 0";
                            query = query + ", '" + TWednesday + "'";
                            if (bThursday == true)
                                query = query + ", -1";
                            else
                                query = query + ", 0";
                            query = query + ", '" + TThursday + "'";
                            if (bFriday == true)
                                query = query + ", -1";
                            else
                                query = query + ", 0";
                            query = query + ", '" + TFriday + "'";
                            if (bSaturday == true)
                                query = query + ", -1";
                            else
                                query = query + ", 0";
                            query = query + ", '" + TSaturday + "'";
                            if (bSunday == true)
                                query = query + ", -1";
                            else
                                query = query + ", 0";
                            query = query + ", '" + TSunday + "'";
                            query = query + ", 0" + NCases + "";
                            query = query + ", 0" + NParamPackages + "";
                            query = query + ", 0" + NParamMeals + "";
                            query = query + "            INSERT INTO feedp_churches_adic  SELECT " + sBranch + "";
                            query = query + ", " + CodChurch + "";
                            query = query + ", '" + DLastVisit.ToString("yyyyMMdd") + "'";

                        }

                        command.CommandText = query;
                        command.ExecuteNonQuery();

                        conn.Close();

                    }
                    SError = "Data Saved Successfully: " + NameChurch + ' ' + PastorName;
                    clearform();
                }
                if(Request.Form["BtnCalculate"].Count > 0)
                {
                    //Everything is calculated in the cshtml
                        
                }
                
            }
            
        }

        private void clearform()
        {
            CodChurch = "";
            NameChurch = "";
            PastorName = "";
            CAddress = "";
            CPhone = "";
            CNotes = "";
            CActive = true;
            NFed = 0;
            bMonday = false;
            bTuesday = false;
            bWednesday = false;
            bThursday = false;
            bFriday = false;
            bSaturday = false;
            bSunday = false;
            TMonday = "";
            TTuesday = "";
            TWednesday = "";
            TThursday = "";
            TFriday = "";
            TSaturday = "";
            TSunday = "";
            NCases = 0;
        }
    }
}