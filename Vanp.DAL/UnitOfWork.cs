using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vanp.DAL.Entites;

namespace Vanp.DAL
{
    public class UnitOfWork : IDisposable
    {
        private readonly Vanp_Entities _context;
        public UnitOfWork()
        {
            _context = new Vanp_Entities();
            UserRepository = new UserRepository(_context);
            CategoryRepository = new CategoryRepository(_context);
            UserRoleRepository = new UserRoleRepository(_context);
            ProductRepository = new ProductRepository(_context);
            RequestRepository = new RequestRepository(_context);
            WishlistRepository = new WishlistRepository(_context);
            BidHistoryRepository = new BidHistoryRepository(_context);
            VoteRepository = new VoteRepository(_context);
        }
        #region Properties
        public readonly IUserRepository UserRepository;
        public readonly ICategoryRepository CategoryRepository;
        public readonly IUserRoleRepository UserRoleRepository;
        public readonly IProductRepository ProductRepository;
        public readonly IRequestRepository RequestRepository;
        public readonly IWishlistRepository WishlistRepository;
        public readonly IBidHistoryRepository BidHistoryRepository;
        public readonly IVoteRepository VoteRepository;
        #endregion

        public void Save()
        {
            _context.SaveChanges();
        }
        public void RollBack()
        {
            var changedEntries = _context.ChangeTracker.Entries()
                .Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
