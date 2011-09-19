using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace SharedPKAssociation
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Database.SetInitializer<SchoolContext>(new DropCreateDatabaseAlways<SchoolContext>());

                SchoolContext context = new SchoolContext();
                
                //Address address = new Address { City = "제천"};
                //Student student = new Student { Name = "Scott Hanselman", Address=address };
                //context.Students.Add(student);
                //Teacher teacher = new Teacher { Name = "Scott Gu", Address = address };
                //context.Teachers.Add(teacher);

                Student student = new Student { Name = "Scott Hanselman" };
                Teacher teacher = new Teacher { Name = "Scott Gu" };
                Address address = new Address { City = "제천", Student = student, Teacher=teacher };
                context.Addresses.Add(address);
                
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
                        .HasOptional(s => s.Address)
                        .WithRequired(a => a.Student);

            modelBuilder.Entity<Teacher>()
                .HasOptional(t => t.Address)
                .WithRequired(a => a.Teacher);

        }
    }

    public class Address
    {
        public int Id { get; set; }
        public string City { get; set; }

        public virtual Student Student { get; set; }
        public virtual Teacher Teacher { get; set; }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual Address Address { get; set; }
    }

    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual Address Address { get; set; }
    }


}
