using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace Lewis_Farmer_GU2.ViewModels
{
    public class BookingReasonPartialViewModel
    {
        [Display(Name = "Booking Type")]
        public string BookingReason { get; set; }

        [Display(Name = "Booking Starting Cost")]
        public string StandardCost { get; set; }

        [Display(Name = "Standard Time To Complete")]
        public string StandardTime { get; set; }

        [Display(Name = "Description")]
        public string ClarityDescription { get; set; }

        public string ImageName { get; set; }

        public BookingReasonPartialViewModel(string type)
        {
            if (type.Equals("MOT"))
            {
                BookingReason = "MOT / CheckUp / Service";
                ClarityDescription = "This option applies if you want your car to be run through any kind of standard service";
                StandardCost = "£50+ based on further actions that may be needed";
                StandardTime = "3-5 Buisness Days, subject to increases";
                ImageName = "MOT.jpg";
            }
            else if (type.Equals("Repair"))
            {
                BookingReason = "Repairs";
                ClarityDescription = "If your car has been in an altercation or has part problems we will fix it up";
                StandardCost = "£30 + The Cost of any parts that are used in the repairs";
                StandardTime = "1-4 Buisness Days, subject to increases";
                ImageName = "Repair.jpg";
            }
            else if (type.Equals("Customisation"))
            {
                BookingReason = "Customisation";
                ClarityDescription = "Perform customisations and personalisations to your Vehicle, Our mods department is growing and evolving by the day";
                StandardCost = "£20 + The cost of the aggreed upon work";
                StandardTime = "7 Buisness Days minimum";
                ImageName = "Customization.jpg";
            }
            else if (type.Equals("Consultation"))
            {
                BookingReason = "Consultation";
                ClarityDescription = "If you have questions, need recommendations or are looking to work with us in an undetermined \n capacity you can talk to one of our recpresentitives";
                StandardCost = "£20 for the consultation, Any bookings that come as a result of this consultation will be registered separately";
                StandardTime = "1 - 4 hours on the day in question";
                ImageName = "Consultation.jpg";
            }
            else if (type.Equals("Other"))
            {
                BookingReason = "non-standard work";
                ClarityDescription = "If you have a hyper specific requirement of us, perhaps an engineering task or non automotive mechanical task we can attempt to accomodate your needs";
                StandardCost = "£20 + A cost decided by the agreed upon work / contact";
                StandardTime = "1 - 4 hour meeting plus time that is aggreed upon during the meeting";
                ImageName = "Other.jpg";
            }
        }
    }
}