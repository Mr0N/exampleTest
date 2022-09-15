using System;
using Microsoft.EntityFrameworkCore;

//Example Use
var myDb = new MyDbContext();
using var info = new Info();
myDb.AddInfo(info);


class MyDbContext:DbContext 
{
    public DbSet<Info> information { set; get; }
    public void AddInfo(Info info)
    {
        if(info is IChangeTrackingDispose<Info> change)
        {
            change.SetDbSet(information);
        }
    }
}
public class Info : IChangeTrackingDispose<Info>
{
    public DbSet<Info> _dbSet { set; get; }

    public void Dispose()
    {
        (this as IChangeTrackingDispose<Info>).Close();
    }
}

interface IChangeTrackingDispose<T>:IDisposable where T : class
{

    DbSet<T> _dbSet { set; get; }
    public void SetDbSet(DbSet<T> dbSet)
    {
        _dbSet = dbSet;
    }
    private void RemoveFromChangeTraking()
    {
        ///The logic for removing this instance from Change Tracking
    }
    public void Close()
    {
        RemoveFromChangeTraking();
    }

}