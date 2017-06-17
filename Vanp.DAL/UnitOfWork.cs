using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vanp.DAL.Entites;
using Vanp.DAL.Repositories;
using Vanp.DAL.Repositories.Interface;

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
            VoteRepository = new VoteRepository(_context);
        }
        #region Properties
        public readonly IUserRepository UserRepository;
        public readonly ICategoryRepository CategoryRepository;
        public readonly IUserRoleRepository UserRoleRepository;
        public readonly IProductRepository ProductRepository;
        public readonly IRequestRepository RequestRepository;
        public readonly IVoteRepository VoteRepository;
        #endregion

        public void Save()
        {
            _context.SaveChanges();
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
