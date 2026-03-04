using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Growtify.Application.Common.Pagination
{
    public class MemberParams : PagingParams
    {
        public string? Gender { get; set; }
        public string? CurrentMemberId { get; set; }
    }
}
