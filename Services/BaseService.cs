using Domain.Interfaces;
using System;

namespace Services
{
    public class BaseService : IDisposable
    {
        protected readonly IUnitOfWork _unitOfWork;

        public BaseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
