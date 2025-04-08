namespace CashFlow.Domain.Repositories.Expenses;

public interface IUnitOfWork
{
    void Commit();
}