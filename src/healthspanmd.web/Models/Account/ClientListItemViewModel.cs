using healthspanmd.core.CQRS.AuthorizedRoles;
using System;
using System.Collections.Generic;

namespace healthspanmd.web.Models.Account
{
    public class ClientListItemViewModel
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneCountryCode { get; set; }
        public string Phone { get; set; }
        public DateTime ProgramStartDate { get; set; }
        public DateTime ProgramEndDate { get; set; }
        public bool IsActive { get; set; }
        public int ActiveMetricCount { get; set; }
        public string AuthorizedRoles { get; set; }

        public string FullPhoneNumber
        {
            get
            {
                return $"+{PhoneCountryCode} {Phone}";
            }
        }

        public string LastName_comma_FirstName
        {
            get
            {
                return $"{LastName}, {FirstName}";
            }
        }
    }
}
