namespace BackendComfeco.DTOs.Shared
{
    public class PaginationDTO
    {
        public int Page { get; set; }

        private int recordsPerPage { get; set; }

        private readonly int maxRecordsPerPage = 100;

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
