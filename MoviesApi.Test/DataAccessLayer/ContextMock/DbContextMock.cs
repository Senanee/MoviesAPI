using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Linq.Expressions;

namespace MoviesApi.Test.DataAccessLayer.ContextMock
{
    public class DbContextMock
    {
        public static TContext GetMock<TData, TContext>(List<TData> lstData, Expression<Func<TContext, DbSet<TData>>> dbSetSelectionExpression) where TData : class where TContext : DbContext
        {

            IQueryable<TData> lstDataQueryable = lstData.AsQueryable();
            Mock<DbSet<TData>> dbSetMock = new();
            Mock<TContext> dbContext = new();

            dbSetMock.As<IAsyncEnumerable<TData>>()
                .Setup(m => m.GetAsyncEnumerator(default))
                .Returns(new MockAsyncHelper.MockAsyncEnumerator<TData>(lstDataQueryable.GetEnumerator()));

            dbSetMock.As<IQueryable<TData>>()
                .Setup(m => m.Provider)
                .Returns(new MockAsyncHelper.TestAsyncQueryProvider<TData>(lstDataQueryable.Provider));

            dbSetMock.As<IQueryable<TData>>()
                .Setup(m => m.Expression)
                .Returns(lstDataQueryable.Expression);

            dbSetMock.As<IQueryable<TData>>()
                .Setup(m => m.ElementType)
                .Returns(lstDataQueryable.ElementType);

            dbSetMock.As<IQueryable<TData>>()
                .Setup(m => m.GetEnumerator())
                .Returns(() => lstDataQueryable.GetEnumerator());

            dbContext
                .Setup(dbSetSelectionExpression)
                .Returns(dbSetMock.Object);
            return dbContext.Object;

        }
    }

    internal class MockAsyncHelper
    {
        internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
        {
            private readonly IQueryProvider _innerQueryProvider;

            internal TestAsyncQueryProvider(IQueryProvider inner)
            {
                _innerQueryProvider = inner;
            }

            public IQueryable CreateQuery(Expression expression)
            {
                return new MockAsyncEnumerable<TEntity>(expression);
            }

            public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            {
                return new MockAsyncEnumerable<TElement>(expression);
            }

            public object Execute(Expression expression)
            {
                return _innerQueryProvider.Execute(expression);
            }

            public TResult Execute<TResult>(Expression expression)
            {
                return _innerQueryProvider.Execute<TResult>(expression);
            }

            TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
            {
                Type expectedResultType = typeof(TResult).GetGenericArguments()[0];
                object? executionResult = ((IQueryProvider)this).Execute(expression);

                return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))
                    .MakeGenericMethod(expectedResultType)
                    .Invoke(null, new[] { executionResult });
            }
        }

        internal class MockAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
        {
            public MockAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable)
            { }

            public MockAsyncEnumerable(Expression expression) : base(expression)
            { }

            IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);

            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
            {
                return new MockAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
            }
        }

        internal class MockAsyncEnumerator<T> : IAsyncEnumerator<T>
        {
            private readonly IEnumerator<T> _enumerator;

            public MockAsyncEnumerator(IEnumerator<T> inner)
            {
                _enumerator = inner;
            }

            public T Current => _enumerator.Current;

            public ValueTask DisposeAsync()
            {
                return new(Task.Run(() => _enumerator.Dispose()));
            }

            public ValueTask<bool> MoveNextAsync()
            {
                return new(_enumerator.MoveNext());
            }
        }
    }
}
