using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace FKAssociation
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Database.SetInitializer<SchoolContext>(new SchoolContextInit());

                SchoolContext context = new SchoolContext();

                Address a1 = new Address() { Street = "street1" };
                Address a2 = new Address() { Street = "street2" };

                Student s1 = new Student() { Name = "student1", FirstAddress = a1, SecondAddress = a2 };
                context.Students.Add(s1);

                // Unique Constraint를 추가하면 아래 s2는 저장되지 않는다.
                Student s2 = new Student() { Name = "student2", FirstAddress = a1, SecondAddress = a2 };
                context.Students.Add(s2);

                Teacher t1 = new Teacher() { Name = "teacher1", Address = a1 };
                context.Teachers.Add(t1);

                // Unique Constraint를 추가하면 아래 t2는 저장되지 않는다.
                Teacher t2 = new Teacher() { Name = "teacher2", Address = a1 };
                context.Teachers.Add(t2);

                context.SaveChanges();

                Console.WriteLine("success");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.Read();
        }
    }

    public class SchoolContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                        .HasOptional(s => s.FirstAddress)
                        .WithMany()
                        .HasForeignKey(s => s.FirstAddressId)
                        ;

            modelBuilder.Entity<Student>()
                        .HasOptional(s => s.SecondAddress)
                        .WithMany()
                        .HasForeignKey(s => s.SecondAddressId)
                        ;

            modelBuilder.Entity<Teacher>()
                        .HasOptional(t => t.Address)
                        .WithMany()
                        .HasForeignKey(t => t.AddressId)
                        ;
        }
    }

    public class SchoolContextInit : DropCreateDatabaseAlways<SchoolContext>
    {
        protected override void Seed(SchoolContext context)
        {
            // Unique Constraint가 필요한 관계라면,,,
            //context.Database.ExecuteSqlCommand("ALTER TABLE Students ADD CONSTRAINT uc_FirstAddress UNIQUE(FirstAddressId)");
            //context.Database.ExecuteSqlCommand("ALTER TABLE Students ADD CONSTRAINT uc_SecondAddress UNIQUE(SecondAddressId)");
            //context.Database.ExecuteSqlCommand("ALTER TABLE Teachers ADD CONSTRAINT uc_Address UNIQUE(AddressId)");
        }
    }

    public class Address
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual Address FirstAddress { get; set; }
        public Nullable<int> FirstAddressId { get; set; }

        public virtual Address SecondAddress { get; set; }
        public Nullable<int> SecondAddressId { get; set; }
    }

    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual Address Address { get; set; }
        public Nullable<int> AddressId { get; set; }
    }
}
