using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Middlewares
{
    public class DbContextTransactionPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IAppDbContext _context;

        public DbContextTransactionPipeline(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (request is IBaseRequest)
            {
                var transaction = await _context.CreateTransaction();
                try
                {
                    var response = await next();
                    await _context.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    return response;
                }
                catch (DbUpdateException)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);

                    throw;
                }
            }
            else
            {
                var response = await next();
                return response;
            }
        }
    }
}