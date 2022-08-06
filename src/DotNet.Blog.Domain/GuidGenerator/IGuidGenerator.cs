using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Domain
{
    public interface IGuidGenerator
    {
        Guid Create();
        Guid Create(SequentialGuidType guidType);
    }
}
