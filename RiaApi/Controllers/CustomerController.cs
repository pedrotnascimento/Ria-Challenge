using Microsoft.AspNetCore.Mvc;
using RiaApi.Models;
using RiaApi.Repository;

namespace RiaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {


        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerRepository _customerRepository;
        private List<Customer>? customerInternalArray;

        public CustomerController(ILogger<CustomerController> logger, ICustomerRepository customerRepository)
        {
            _logger = logger;
            _customerRepository = customerRepository;
            customerInternalArray = _customerRepository.GetInternalArray();
        }

        [HttpGet()]
        public async Task<IEnumerable<Customer>> Get()
        {
            var customers = await _customerRepository.GetAllAsync();
            if (customers == null)
            {
                return new List<Customer>();
            }
            return customers;
        }

        [HttpPost()]
        public async Task<ActionResult> Post([FromBody] IEnumerable<Customer> customers)
        {

            if (customers.Count() == 0)
            {
                return BadRequest("No customers were sent");
            }

            var hasOneInvalid = customers.Any(x => x.Age == default ||
            String.IsNullOrWhiteSpace(x.FirstName) ||
            String.IsNullOrWhiteSpace(x.LastName) || x.Id == default);
            if (hasOneInvalid)
            {
                return BadRequest("Missing fields");
            }

            var hasAnyUnderAge = customers.Any(x => x.Age <= 18);
            if (hasAnyUnderAge)
            {
                return BadRequest("No under age customer allowed");
            }

            var hasAlreadyExistentId = await _customerRepository.HasExistentIdAsync(customers);
            if (hasAlreadyExistentId)
            {
                return BadRequest("Can't create customer with already existing ID");
            }

            await _customerRepository.CreateManyAsync(customers);

            UpdateCustomersInternalArray(customers);
            return Ok();
        }

        private void UpdateCustomersInternalArray(IEnumerable<Customer> customers)
        {
            customerInternalArray.AddRange(customers);

            // Sort the list using the custom comparer
            customerInternalArray.Sort(new CustomerComparer());

            // customerInternalArray = customerInternalArray
            //     .OrderBy(customer => customer.LastName)
            //     .ThenBy(customer => customer.FirstName).ToList();
        }
    }
}