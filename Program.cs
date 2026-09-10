using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;

namespace cha90
{
  
    internal class Program
    {
        static bool LoginFunction(SqlConnection con, string username,string password)
        {
            username = username.Trim();
            password = password.Trim();

            if (string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine("Please enter email");
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Please enter password");
                return false;
            }

            string selectcommand = "select count(*) from employees where email=@username and PasswordHash=@password";
        
            using SqlCommand com=new SqlCommand(selectcommand, con);
            com.Parameters.Add("@username", System.Data.SqlDbType.VarChar).Value = username;
            com.Parameters.Add("@password",System.Data.SqlDbType.VarChar).Value=password;
     
           
            int count = (int)com.ExecuteScalar();
            if (count>0)
            {
                Console.WriteLine("Login Success");
                return true;
            }
            else
            {
                Console.WriteLine("Login Failed");
                return false;
            }

           
        }
        internal static void menu(SqlConnection con)
        {
            while (true)
            {
                Console.WriteLine("1. Add Employee");
                Console.WriteLine("2. View Employees");
                Console.WriteLine("3. Search Employee");
                Console.WriteLine("4. Update Employee");
                Console.WriteLine("5. Delete Employee");
               
                Console.WriteLine("6. Exit");
                if (!int.TryParse(Console.ReadLine(), out int choose))
                {
                    Console.WriteLine("enter number");
                    continue;
                }
              
                if (choose == 1)
                {
                    add(con);
                }
                else if (choose == 2)
                {
                    view(con);
                }
                else if (choose == 3)
                {
                    search(con);
                }
                else if (choose == 4)
                {
                    update(con);
                }
                else if (choose == 5)
                {
                    delete(con);
                }
                else if (choose == 6)
                {
                   break; 
                }
              
                else
                {
                    Console.WriteLine("please enter true number");
          
                }
            }
        }
        internal static string GetValidName()
        {
            while (true)
            {
                Console.Write("Full Name: ");
                string fullname = Console.ReadLine() ?? "";
                fullname = fullname.Trim();
                if (string.IsNullOrEmpty(fullname))
                {
                    Console.WriteLine("Name cannot be empty");
                    continue;
                }
                
                if (!fullname.All(c => char.IsLetter(c) || c == ' '))

                {
                    Console.WriteLine("Name cannot contain numbers or special characters");
                    continue;
                }
return fullname;
            }
        }
        internal static int getvalidage()
        {
            while (true) { 
            Console.Write("Age: ");
                if (!int.TryParse(Console.ReadLine(), out int  age))
                {
                    Console.WriteLine("Please enter numbers only");
                    continue;
                }
                if (age < 0 ||age>100)
            {
                Console.WriteLine("enter positive value");
                continue;
            }
           
            return age;
        }}
        internal static string GetValidEmail()
        {
            while (true)
            {
                Console.Write("Email: ");
                string email = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(email))
                {
                    Console.WriteLine("Email cannot be empty");
                    continue;
                }

                if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    Console.WriteLine("Invalid email");
                    continue;
                }

                return email;
            }
        }
        internal static string GetValidPhone()
        {
            while (true)
            {
                Console.Write("Phone: ");
                string phone = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(phone))
                {
                    Console.WriteLine("Phone cannot be empty");
                    continue;
                }

                if (!phone.All(char.IsDigit))
                {
                    Console.WriteLine("Phone must contain numbers only");
                    continue;
                }

                if (phone.Length != 11)
                {
                    Console.WriteLine("Phone must be 11 digits");
                    continue;
                }

                return phone;
            }
        }
        internal static decimal getvalidsalary()
        {
            while (true)
            {
                Console.Write("salary: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal salary))
                {
                    Console.WriteLine("Please enter numbers only");
                    continue;
                }
                if (salary  <= 0 || salary > 100000)
                {
                    Console.WriteLine("enter valid value");
                    continue;
                }

                return salary;
            }
        }
        internal static int GetValidDepartment(SqlConnection con)
        {
            while (true)
            {
                Console.WriteLine("1. Finance");
                Console.WriteLine("2. HR");
                Console.WriteLine("3. IT");
                Console.WriteLine("4. Marketing");
                Console.WriteLine("5. Sales");

                Console.Write("Department ID: ");

                if (!int.TryParse(Console.ReadLine(), out int departmentID))
                {
                    Console.WriteLine("Enter number only");
                    continue;
                }
                if (departmentID < 1 || departmentID > 5)
                {
                    Console.WriteLine("Enter number from 1 to 5");
                    continue;
                }

                string searchdept =
                    "select count(*) from Departments WHERE DepartmentID=@DepartmentID";


                using SqlCommand cmd = new SqlCommand(searchdept, con);

                cmd.Parameters.Add("@DepartmentID", System.Data.SqlDbType.Int).Value = departmentID;


                int count = (int)cmd.ExecuteScalar();


                if (count == 0)
                {
                    Console.WriteLine("Department not found");
                    continue;
                }


                return departmentID;
            }
        }
        internal static void add(SqlConnection con)
        {
            string addemployee =
            "insert into employees(fullname,age,email,phone,salary,hiredate,departmentid) " +
            "values(@fullname,@age,@email,@phone,@salary,@hiredate,@departmentid)";

            using SqlCommand commandsql = new SqlCommand(addemployee, con);



            string fullname = GetValidName();
            commandsql.Parameters.AddWithValue("@fullname", fullname);


            int age = getvalidage();
            commandsql.Parameters.AddWithValue("@age", age);


            string email = GetValidEmail();
            commandsql.Parameters.AddWithValue("@email", email);


            string phone = GetValidPhone();
            commandsql.Parameters.AddWithValue("@phone", phone);



            decimal salary = getvalidsalary();
            commandsql.Parameters.AddWithValue("@salary", salary);


            Console.Write("Hire Date: ");

            DateTime hiredate;

            while (!DateTime.TryParseExact(
                Console.ReadLine(),
                "dd-MM-yyyy",
                null,
                System.Globalization.DateTimeStyles.None,
                out hiredate)
                || hiredate > DateTime.Now)
            {
                Console.WriteLine("Enter valid hire date (dd-MM-yyyy):");
            }
            commandsql.Parameters.AddWithValue("@hiredate", hiredate);


            int departmentID = GetValidDepartment(con);

            commandsql.Parameters.AddWithValue("@departmentid", departmentID);


            int rows = commandsql.ExecuteNonQuery();

            if (rows > 0)
                Console.WriteLine("Employee Added Successfully");
            else
                Console.WriteLine("Failed");
        }



        internal static void view(SqlConnection con)
        {
            string viewemployee =
                "select * from employees ";


            using SqlCommand command = new SqlCommand(viewemployee, con);




            using SqlDataReader reader = command.ExecuteReader();


            while (reader.Read())
            {
               
                    Console.WriteLine("Employee ID: " + reader["EmployeeID"]);
                    Console.WriteLine("Full Name: " + reader["FullName"]);
                    Console.WriteLine("Age: " + reader["Age"]);
                    Console.WriteLine("Email: " + reader["Email"]);
                    Console.WriteLine("Phone: " + reader["Phone"]);
                    Console.WriteLine("Salary: " + reader["Salary"]);
                    Console.WriteLine("Hire Date: " + reader["HireDate"]);
                    Console.WriteLine("Department ID: " + reader["DepartmentID"]);
                    Console.WriteLine("\n");
               
            }
        }

        internal static void search(SqlConnection con)
        {

            string fullname = GetValidName();

            string viewemployee =
   "select * from employees where fullname like @fullname";



            using SqlCommand command = new SqlCommand(viewemployee, con);
            command.Parameters.AddWithValue("@fullname", "%" + fullname + "%");



            using SqlDataReader reader = command.ExecuteReader();





            if (!reader.HasRows)
            {
                Console.WriteLine("Employee Not Found");
            }


            while (reader.Read())
            {
                Console.WriteLine("Employee ID: " + reader["EmployeeID"]);
                Console.WriteLine("Full Name: " + reader["FullName"]);
                Console.WriteLine("Age: " + reader["Age"]);
                Console.WriteLine("Email: " + reader["Email"]);
                Console.WriteLine("Phone: " + reader["Phone"]);
                Console.WriteLine("Salary: " + reader["Salary"]);
                Console.WriteLine("Hire Date: " + reader["HireDate"]);
                Console.WriteLine("Department ID: " + reader["DepartmentID"]);
                Console.WriteLine("\n");
            }
        }
        internal static void delete(SqlConnection con)
        {
            Console.Write("Employee ID: ");

            int id;

            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Enter valid ID");
            }


            string command = "delete from employees where EmployeeID=@id";


            using SqlCommand sqlCommand = new SqlCommand(command, con);

            sqlCommand.Parameters.AddWithValue("@id", id);
            int rows = sqlCommand.ExecuteNonQuery();

            if (rows > 0)
            {
                Console.WriteLine("Employee deleted successfully");
            }
            else
            {
                Console.WriteLine("Employee not found");
            }

        }
        internal static void update(SqlConnection con)
        {
            while (true)
            {
                Console.WriteLine("1. edit name");
                Console.WriteLine("2. edit age");
                Console.WriteLine("3. edit phone");
                Console.WriteLine("4. edit email");
                Console.WriteLine("5. edit password ");
                Console.WriteLine("6. Exit");
                if (!int.TryParse(Console.ReadLine(), out int choose))
                {
                    Console.WriteLine("enter number");
                    continue;
                }

                if (choose == 1)
                {
                    string fullname = GetValidName();
                    string email = GetValidEmail();
                    string updatequery = "update employees set fullname=@fullname where email=@email";
                    using SqlCommand sqlCommand = new SqlCommand(updatequery, con);
                    sqlCommand.Parameters.AddWithValue("@fullname", fullname);
                    sqlCommand.Parameters.AddWithValue("@email", email);

                    int rows = sqlCommand.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        Console.WriteLine("Employee updated successfully");
                    }
                    else
                    {
                        Console.WriteLine("Employee not found");
                    }

                }
                else if (choose == 2)
                {
                    int age = getvalidage();
                    string email = GetValidEmail();
                    string updatequery = "update employees set age=@age where email=@email";
                    using SqlCommand sqlCommand = new SqlCommand(updatequery, con);
                    sqlCommand.Parameters.AddWithValue("@age", age);
                    sqlCommand.Parameters.AddWithValue("@email", email);

                    int rows = sqlCommand.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        Console.WriteLine("Employee updated successfully");
                    }
                    else
                    {
                        Console.WriteLine("Employee not found");
                    }
                }
                else if (choose == 3)
                {
                    string phone = GetValidPhone();
                    string email = GetValidEmail();
                    string updatequery = "update employees set phone=@phone where email=@email";
                    using SqlCommand sqlCommand = new SqlCommand(updatequery, con);
                    sqlCommand.Parameters.AddWithValue("@phone", phone);
                    sqlCommand.Parameters.AddWithValue("@email", email);

                    int rows = sqlCommand.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        Console.WriteLine("Employee updated successfully");
                    }
                    else
                    {
                        Console.WriteLine("Employee not found");
                    }
                }
                else if (choose == 4)
                {
                    Console.Write("Employee ID: ");
                    int id;

                    while (!int.TryParse(Console.ReadLine(), out id))
                    {
                        Console.WriteLine("Enter valid ID");
                    }

                    string email = GetValidEmail();
                    string updatequery =
      "update employees set email=@email where EmployeeID=@id";
                    using SqlCommand sqlCommand = new SqlCommand(updatequery, con);
                    sqlCommand.Parameters.AddWithValue("@id", id);
                    sqlCommand.Parameters.AddWithValue("@email", email);
                    int rows = sqlCommand.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        Console.WriteLine("Employee updated successfully");
                    }
                    else
                    {
                        Console.WriteLine("Employee not found");
                    }
                }
                else if (choose == 5)
                {
                    Console.Write("Password: ");
                    string password = Console.ReadLine() ?? "";

                    while (string.IsNullOrWhiteSpace(password))
                    {
                        Console.WriteLine("Password cannot be empty");
                        Console.Write("Password: ");
                        password = Console.ReadLine() ?? "";
                    }
                    string email = GetValidEmail();
                    string updatequery = "update employees set passwordHash=@password where email=@email";
                    using SqlCommand sqlCommand = new SqlCommand(updatequery, con);
                    sqlCommand.Parameters.AddWithValue("@password", password);
                    sqlCommand.Parameters.AddWithValue("@email", email);

                    int rows = sqlCommand.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        Console.WriteLine("Employee updated successfully");
                    }
                    else
                    {
                        Console.WriteLine("Employee not found");
                    }

                }
                else if (choose == 6)
                {
                    break;
                }
              
                else
                {
                    Console.WriteLine("please enter true number");

                }


            }
        }
        




        static void Main(string[] args)
        {
            string connectiondatabase =
                "Server=.;Database=CompanyManagement;Trusted_Connection=True;TrustServerCertificate=True;";

          using  SqlConnection con = new SqlConnection(connectiondatabase);

            con.Open();
            Console.Write("Email: ");
            string username = Console.ReadLine() ?? "";

            Console.Write("Password: ");
            string password = Console.ReadLine() ?? "";

            bool islogedin = LoginFunction(con, username, password);
            if (islogedin)
            {
                menu(con);
            }
           


        }
    }
}