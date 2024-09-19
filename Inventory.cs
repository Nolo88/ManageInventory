using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ManageInventory
{
    public partial class Inventory : Form
    {
        private static string connectionString = @"Data Source=LAPTOP-2FO6KVBK\SQLEXPRESS02;Initial Catalog=InventoryManagementsDB;Integrated Security=True";
        public Inventory()
        {
            InitializeComponent();


        }
        private void ViewData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Inventory";

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    dgvDatabse.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
            }
        }


        private void Inventory_Load(object sender, EventArgs e)
        {
            this.inventoryTableAdapter.Fill(this.inventoryManagementsDBDataSet.Inventory);
            ViewData();
            //The data is automatically sorted by ProductID
            txtSerchText.Text = "Enter your text here";
            txtSerchText.ForeColor = Color.Gray;
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            // Getting the last row in the DataGridView
            int lastRowIndex = dgvDatabse.Rows.Count - 2;

            DataGridViewRow row = dgvDatabse.Rows[lastRowIndex];

            string id = row.Cells[0].Value?.ToString();
            string name = row.Cells[1].Value?.ToString();
            string category = row.Cells[2].Value?.ToString();
            string description = row.Cells[3].Value?.ToString();
            int quantity = Convert.ToInt32(row.Cells[4].Value);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Inventory (ProductID, ProductName, Category, ProductDescription, Quantity) VALUES (@id, @name, @category, @description, @quantity)", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@category", category);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@quantity", quantity);


                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Data added successfully!");
            ViewData();
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            if (dgvDatabse.SelectedRows.Count > 0)
            {
                // Getting the selected row
                DataGridViewRow selectedRow = dgvDatabse.SelectedRows[0];
                string id = selectedRow.Cells["productIDDataGridViewTextBoxColumn"].Value?.ToString();

                var confirmResult = MessageBox.Show("Are you sure you want to delete this row?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    dgvDatabse.Rows.Remove(selectedRow);

                    RemoveRecordFromDatabase(id);
                }
                ViewData();
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }

        private void RemoveRecordFromDatabase(string id)
        {
            string query = "DELETE FROM Inventory WHERE ProductID = @id";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.VarChar) { Value = id });
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void btnUpdateProduct_Click(object sender, EventArgs e)
        {
            if (dgvDatabse.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dgvDatabse.SelectedRows[0];
                string id = selectedRow.Cells["productIDDataGridViewTextBoxColumn"].Value?.ToString(); // Use the actual column name

                // Ensure the ID is not null or empty
                if (string.IsNullOrEmpty(id))
                {
                    MessageBox.Show("The selected row does not have a valid ID.");
                    return;
                }

                // Get the new values from the DataGridView or input fields
                string newName = selectedRow.Cells["Name"].Value?.ToString();
                string newCategory = selectedRow.Cells[2].Value?.ToString();
                string newDescription = selectedRow.Cells[3].Value?.ToString();
                int newQuantity = Convert.ToInt32(selectedRow.Cells["Quantity"].Value);
                // Confirm update
                var confirmResult = MessageBox.Show("Are you sure you want to update this row?", "Confirm Update", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    // Update the corresponding record in the database
                    UpdateRecordInDatabase(id, newName, newCategory, newDescription, newQuantity);
                    MessageBox.Show("Record updated successfully.");
                }
            }
            else
            {
                MessageBox.Show("Please select a row to update.");
            }
        }

        private void UpdateRecordInDatabase(string id, string newName, string newCategory, string newDescription, int newQuantity)
        {
            string query = "UPDATE Inventory SET ProductName = @name, Category = @category Quantity = @quantity WHERE ID = @id"; // Adjust table and column names
            //ProductID, ProductName, Category, ProductDescription, Quantity
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.VarChar) { Value =  id}); // Use SqlDbType.VarChar for string IDs
                    cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar) { Value = newName });
                    cmd.Parameters.Add(new SqlParameter("@category", SqlDbType.VarChar) { Value = newCategory});
                    cmd.Parameters.Add(new SqlParameter("@description", SqlDbType.VarChar) { Value = newDescription});
                    cmd.Parameters.Add(new SqlParameter("@quantity", SqlDbType.Int) { Value = newQuantity });

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchText = txtSerchText.Text.Trim(); // Get the search term from the textbox

            if (string.IsNullOrEmpty(searchText))
            {
                MessageBox.Show("Please enter a search term.");
                return;
            }

            foreach (DataGridViewRow row in dgvDatabse.Rows)
            {
                row.DefaultCellStyle.BackColor = Color.White;
            }

            bool found = false;

            foreach (DataGridViewRow row in dgvDatabse.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && cell.Value.ToString().IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        row.DefaultCellStyle.BackColor = Color.Yellow;
                        dgvDatabse.FirstDisplayedScrollingRowIndex = row.Index;// Highlight the row
                        found = true;
                        break; // Stop searching this row's cells
                    }
                }
                if (found)
                {
                    break; // Stop searching other rows once a match is found
                }

            }
            if (!found)
            {
                MessageBox.Show("No matching rows found.");
            }
        }
    }
}
