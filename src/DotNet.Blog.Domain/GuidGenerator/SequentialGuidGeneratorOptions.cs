using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Domain
{
    public class SequentialGuidGeneratorOptions
    {
        /// <summary>
        /// Default value: null (unspecified).
        /// Use <see cref="GetDefaultSequentialGuidType"/> method
        /// to get the value on use, since it fall backs to a default value.
        /// </summary>
        public SequentialGuidType? DefaultSequentialGuidType { get; set; }

        /// <summary>
        /// Get the <see cref="DefaultSequentialGuidType"/> value
        /// or returns <see cref="SequentialGuidType.SequentialAtEnd"/>
        /// if <see cref="DefaultSequentialGuidType"/> was null.
        /// </summary>
        public SequentialGuidType GetDefaultSequentialGuidType()
        {
            return DefaultSequentialGuidType ??
                   SequentialGuidType.SequentialAsString;
        }
    }
}
