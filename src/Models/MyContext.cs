using Microsoft.EntityFrameworkCore;

namespace BuildCalc.Models;
public class MyContext : DbContext
{
    public MyContext(DbContextOptions<MyContext> options)
        : base(options)
    {
    }
}