namespace ProvaPub.Models
{
	public class CustomerList : PagedList<Customer>
    {
		//public List<Customer>? Customers { get; set; }
		//public int? TotalCount { get; set; }
		public bool? HasNext { get; set; }
	}
}
