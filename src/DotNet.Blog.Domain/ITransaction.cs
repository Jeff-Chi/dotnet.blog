using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Blog.Domain
{
    /// <summary>
    /// efcore 事务
    /// </summary>
    public interface ITransaction
    {
        IDbContextTransaction? GetCurrentTransaction();

        bool HasActiveTransaction { get; }

        Task<IDbContextTransaction>? BeginTransactionAsync();

        Task CommitTransactionAsync(IDbContextTransaction transaction);

        void RollbackTransaction();
    }
}
