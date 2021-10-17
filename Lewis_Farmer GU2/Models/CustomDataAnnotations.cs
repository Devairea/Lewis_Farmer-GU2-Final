using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Lewis_Farmer_GU2.Models
{
    public class CustomDataAnnotations
    {
        public class CurrentDateAttribute : ValidationAttribute
        {
            public CurrentDateAttribute()
            {
            }

            public override bool IsValid(object value)
            {
                if (value == null)
                {
                    return true;
                }
                var dt = (DateTime)value;
                if (dt >= DateTime.Now)
                {
                    return true;
                }
                return false;
            }
        }

        public class RegistrationNumberAttribute : ValidationAttribute
        {
            public RegistrationNumberAttribute()
            {

            }

            public override bool IsValid(object value)
            {
                //try
                //{
                string val = (string)value;
                val = val.Replace(" ", string.Empty);
                val = val.ToUpper();
                if (val.Length != 6)
                {
                    ErrorMessage = "A Valid UK Registation must be 6 Characters long (Excluding Spaces)";
                    return false;
                }
                if (val.IndexOfAny("`¬!£$%^&*()_-+={}[];:'@#~,<.>?/".ToCharArray()) != -1)
                {
                    ErrorMessage = "A Valid UK Registration Must Contain No Punctuation (Excluding Spaces)";
                    return false;
                }
                return true;

                //if (Regex.IsMatch(val, @"^[A-Z]{2}[0-9]{2}\s?[A-Z]{3}$)|(^[A-Z][0-9]{1,3}[A-Z]{3}$)|(^[A-Z]{3}[0-9]{1,3}[A-Z]$)|(^[0-9]{1,4}[A-Z]{1,2}$)|(^[0-9]{1,3}[A-Z]{1,3}$)|(^[A-Z]{1,2}[0-9]{1,4}$)|(^[A-Z]{1,3}[0-9]{1,3}$"))
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}                   

                //catch
                //{
                //    return false;
                //}
            }
        }

        public class UnoccupiedDateAttribute : ValidationAttribute
        {
            public UnoccupiedDateAttribute()
            {

            }

            public override bool IsValid(object value)
            {

                if (value == null)
                {
                    ErrorMessage = "Please Enter a Date";
                    return false;
                }
                DateTime val;
                try
                {
                    val = (DateTime)value;

                    ApplicationDBContext db = new ApplicationDBContext();

                    List<Job> jobs = db.Jobs.Where(p => p.JobName.Equals("Opening Meeting")).Where(p => p.DueDate == val).ToList();

                    if (jobs.Count > 0)
                    {
                        ErrorMessage = "Please Enter A Booking Date Not Currently Occupied (Please Look At The Booked Dates Provided)";
                        return false;
                    }
                    return true;
                }
                catch
                {
                    ErrorMessage = "Please Enter a Date";
                    return false;
                }
            }
        }
    }
}