#r "System.ComponentModel.DataAnnotations.dll"
#r "System.Data.dll"
#r "C:\Work\Sklad2\packages\EntityFramework.6.1.3\lib\net45\EntityFramework.dll"
#r "C:\Work\Sklad2\packages\EntityFramework.6.1.3\lib\net45\EntityFramework.SqlServer.dll"

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class DbEntity
{
    [Key]
    public Guid Id { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
}

public class NamedDbEntity : DbEntity
{
    public string Name { get; set; }
}

public class Material : NamedDbEntity
{
}

public class User : NamedDbEntity
{
}

public class Worker : NamedDbEntity
{
}

public class Stage : NamedDbEntity
{
}

public class PipelineRule : DbEntity
{
    public virtual Stage From { get; set; }
    public virtual Stage To { get; set; }
}

public class Order : DbEntity
{
    public virtual Material Material { get; set; }
    public int Kgs { get; set; }
    public int Bags { get; set; }
    public DateTime ActionedAt { get; set; }
    public virtual Worker ActionedBy { get; set; }
    public virtual Stage From { get; set; }
    public virtual Stage To { get; set; }
}

public class OrdersContext : DbContext
{
    public OrdersContext() : base("Sklad")
    {

    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Worker> Workers { get; set; }
    public DbSet<Stage> Stages { get; set; }
    public DbSet<PipelineRule> PipelineRules { get; set; }

}


var systemUser = new User { Name = "System" };

