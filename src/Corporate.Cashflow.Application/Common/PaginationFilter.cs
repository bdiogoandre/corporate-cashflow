namespace Corporate.Cashflow.Application.Common
{
    public class PaginationFilter
    {
        private const int MaximumPageSize = 100;
        private int _pageSize = 10;

        public int Page { get; set; } = 1;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaximumPageSize ? MaximumPageSize : value;
        }
    }

}
