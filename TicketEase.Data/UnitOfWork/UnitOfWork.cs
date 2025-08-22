using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using TicketEase.Data.Context;

namespace TicketEase.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TicketEaseDbContext _context;

        private IDbContextTransaction _transaction;

        public UnitOfWork(TicketEaseDbContext context)
        {
            _context = context;
        }
        public async Task BeginTransaction()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransaction()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public void Dispose()
        {
            //garbage collector will call this method when the object is no longer in use
            _context.Dispose();
        }

        public async Task RollbackTransaction()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
