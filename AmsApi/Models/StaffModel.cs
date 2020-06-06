using AmsApi.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AmsApi.Models
{
    public class StaffModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Mobile { get; set; }
      
        public bool Active { get; set; }
        public bool CanBook { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Title { get; set; }
        public string BriefCv { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public string Role { get; set; }
        
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;


            }
        }

        public string RoleName { 
            get {
                var name = "";
                switch (RoleId) {
                    case 1:
                        Role = "Admin";
                        break;
                    case 2:
                        Role =  "Employee";
                        break;
                    case 3:                        
                        name = "Client";
                        break;
                }
                return name;
                } }
        public int RoleId { get; set; }
        public ICollection<StaffWorkingHourModel> StaffWorkingHours { get; set; }
    }
}
