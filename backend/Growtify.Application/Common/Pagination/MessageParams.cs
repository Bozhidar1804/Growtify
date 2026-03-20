namespace Growtify.Application.Common.Pagination
{
    public class MessageParams : PagingParams
    {
        public string? MemberId { get; set; }
        public string Container { get; set; } = "Inbox";
    }
}
