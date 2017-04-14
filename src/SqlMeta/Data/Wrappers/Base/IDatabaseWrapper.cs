namespace SqlMeta.Data.Wrappers.Base
{
    using System;
    using System.Data;

    public interface IDatabaseWrapper
    {
        IDbConnection GetOpenDbConnection();
        TResult Call<TResult>(Func<IDbConnection, TResult> func);
    }
}