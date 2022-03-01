using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Helper
{
    public class PaginationModel
    {
        public int CurrentPageNumber { get; set; }
        public int Offset { get; set; }
        public int TotalItemsCount { get; set; }
        public int CurrentItemsCount { get; set; }
        public bool NextPage { get; set; }
        public bool PrevPage { get; set; }
        public string PrevPageUrl { get; set; }
        public string NextPageUrl { get; set; }

        public int PrevPageNumber { get; set; }
        public int NextPageNumber { get; set; }
        public string BasePath { get; set; }
        public int NumberOfPages { get; set; }
        public int RequestedPageSize { get; set; }

        public PaginationModel()
        {
        }

        public PaginationModel(int currentItemsCount, string basePath, int currentPageNumber, int requestedPageSize, int totalItemCount)
        {
            CurrentPageNumber = currentPageNumber;
            CurrentItemsCount = currentItemsCount;
            Offset = (currentPageNumber - 1) * requestedPageSize;
            TotalItemsCount = totalItemCount;
            BasePath = basePath;
            RequestedPageSize = requestedPageSize;

            PrevPageNumber = currentPageNumber;
            NextPageNumber = currentPageNumber;

            var skip = (CurrentPageNumber - 1) * RequestedPageSize;
            var traversedSoFar = skip + CurrentItemsCount;
            var remaining = TotalItemsCount - traversedSoFar;
            NextPage = remaining > requestedPageSize;
            PrevPage = currentPageNumber > 1;
            if (requestedPageSize == 0)
                NumberOfPages = 0;
            else
                NumberOfPages = (int)Math.Ceiling((decimal)(totalItemCount / requestedPageSize));


            if (NextPage)
                NextPageNumber = CurrentPageNumber + 1;

            NextPageUrl = $"{basePath}/?page={NextPageNumber}&pageSize={RequestedPageSize}";

            if (PrevPage)
                PrevPageNumber = CurrentPageNumber - 1;


            PrevPageUrl = $"{basePath}/?page={PrevPageNumber}&pageSize={RequestedPageSize}";
        }
    }
}
