using System;
using System.Windows.Forms;
using System.Linq;
using MongoDB.Driver;

namespace Coursework_4
{
    public partial class Form1 : Form
    {
        private MongoClient client;
        private IMongoDatabase database;
        private IMongoCollection<Employee> employeeCollection;
        public Form1()
        {
            InitializeComponent();
            var connectionString = "mongodb://localhost:27017";
            var databaseName = "Company";

            client = new MongoClient(connectionString);
            database = client.GetDatabase(databaseName);
            employeeCollection = database.GetCollection<Employee>("employees");
        }
        private bool IsDuplicateEmployee(string name)
        {
            var filter = Builders<Employee>.Filter.Eq("Name", name);
            return employeeCollection.Find(filter).Any();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            var name = txtName.Text;
            var age = int.Parse(txtAge.Text);

            if (ValidateEmployeeData(name, age))
            {

                {

                    var employee = new Employee
                    {
                        Name = name,
                        Age = age
                    };

                    employeeCollection.InsertOne(employee);
                    RefreshEmployeeList();
                    MessageBox.Show("Employee created successfully!");
                }

            }
            if (IsDuplicateEmployee(name))
            {
                MessageBox.Show("Employee with the same name already exists.");
                return;
            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (lstEmployees.SelectedItem is Employee selectedEmployee)
            {
                selectedEmployee.Name = txtName.Text;
                selectedEmployee.Age = int.Parse(txtAge.Text);

                var filter = Builders<Employee>.Filter.Eq("_id", selectedEmployee.Id);
                var update = Builders<Employee>.Update
                    .Set("Name", selectedEmployee.Name)
                    .Set("Age", selectedEmployee.Age);

                employeeCollection.UpdateOne(filter, update);
                RefreshEmployeeList();
            }
            if (lstEmployees.SelectedItem == null)
            {
                MessageBox.Show("Please select an employee to update.");
                return;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstEmployees.SelectedItem is Employee selectedEmployee)
            {
                var filter = Builders<Employee>.Filter.Eq("_id", selectedEmployee.Id);
                employeeCollection.DeleteOne(filter);
                RefreshEmployeeList();
            }
            if (lstEmployees.SelectedItem == null)
            {
                MessageBox.Show("Please select an employee to delete.");
                return;
            }
        }
        private void RefreshEmployeeList()
        {
            lstEmployees.Items.Clear();
            var employees = employeeCollection.Find(_ => true).ToList();
            foreach (var employee in employees)
            {
                lstEmployees.Items.Add(employee);
            }
        }
        private bool ValidateEmployeeData(string name, int age)
        {
            // Validate name and age as per your application's requirements
            if (string.IsNullOrWhiteSpace(name) || age <= 0)
            {
                MessageBox.Show("Please enter a valid name and age.");
                return false;
            }
            return true;
        }
        private void lstEmployees_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            lstEmployees.Items.Clear();
            var searchText = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                RefreshEmployeeList(); // Show all employees since no search criteria is provided
                return;
            }

            var filter = Builders<Employee>.Filter.Where(emp =>
                emp.Name.Contains(searchText) || emp.Age.ToString() == searchText);

            try
            {
                var employees = employeeCollection.Find(filter).ToList();

                if (employees.Count == 0)
                {
                    MessageBox.Show("No matching employees found.");
                    return;
                }
                foreach (var employee in employees)
                {
                    lstEmployees.Items.Add(employee);
                }
            }
            catch (MongoQueryException ex)
            {
                MessageBox.Show($"Error occurred while searching: {ex.Message}");
                // Handle the query exception here (e.g., log the error or display a user-friendly message)
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error occurred: {ex.Message}");
                // Handle other exceptions that may occur during search
            }
        }
    }
    public class Employee
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public override string ToString()
        {
            return $"{Name} (Age: {Age})";
        }
    }
}
