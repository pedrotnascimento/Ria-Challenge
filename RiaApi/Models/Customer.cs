using MongoDB.Bson;

namespace RiaApi.Models
{
    public class Customer
    {
        public int Id { get; set; } = default;
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public int Age { get; set; } = default;


    }

    public class CustomerComparer : IComparer<Customer>
    {
        public int Compare(Customer x, Customer y)
        {
            // Compare by last name first
            int lastNameComparison = string.Compare(x.LastName, y.LastName, StringComparison.OrdinalIgnoreCase);

            if (lastNameComparison != 0)
            {
                return lastNameComparison;
            }

            // If last names are the same, compare by first name
            return string.Compare(x.FirstName, y.FirstName, StringComparison.OrdinalIgnoreCase);
        }
    }
}