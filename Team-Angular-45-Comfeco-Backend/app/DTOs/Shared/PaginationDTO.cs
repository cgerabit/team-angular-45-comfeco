namespace BackendComfeco.DTOs.Shared
{
    public class PaginationDTO
    {
        public int Page { get; set; } = 1;

        private int recordsPerPage { get; set; } = 10;

        private readonly int maxRecordsPerPage = 120;

        public int RecordsPerPage
        {
            get => recordsPerPage;
            set
            {
                recordsPerPage = (value > maxRecordsPerPage) ?
                    maxRecordsPerPage : value;
            }
        }

    }
}
