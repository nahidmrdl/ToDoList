using System;
using System.Data;
using System.Windows.Forms;

namespace To_Do_List_App
{
    public partial class ToDoList : Form
    {
        public ToDoList()
        {
            InitializeComponent();
        }

        DataTable toDoList = new DataTable();
        bool isEditing = false;

        private void ToDoList_Load(object sender, EventArgs e)
        {
            // Create columns
            toDoList.Columns.Add("Title");
            toDoList.Columns.Add("Description");
            toDoList.Columns.Add("Due Date");
            toDoList.Columns.Add("Priority"); 

            toDoListView.DataSource = toDoList;

            // Add items to the priorityBox
            priorityBox.Items.AddRange(new string[] { "High", "Medium", "Low" });
            priorityBox.DropDownStyle = ComboBoxStyle.DropDownList;

        }

        private void ClearTextBoxes()
        {
            textBoxTitle.Text = "";
            textBoxDescription.Text = "";
            dueDatePicker.Value = DateTime.Now;
            priorityBox.SelectedIndex = -1;
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (toDoListView.CurrentRow != null)
            {
                isEditing = true;

                // Fill text fields with data from table
                textBoxTitle.Text = toDoList.Rows[toDoListView.CurrentRow.Index]["Title"].ToString();
                textBoxDescription.Text = toDoList.Rows[toDoListView.CurrentRow.Index]["Description"].ToString();
                dueDatePicker.Value = DateTime.Parse(toDoList.Rows[toDoListView.CurrentRow.Index]["Due Date"].ToString());
                priorityBox.SelectedItem = toDoList.Rows[toDoListView.CurrentRow.Index]["Priority"].ToString();
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (toDoListView.CurrentRow != null)
            {
                var confirmResult = MessageBox.Show("Are you sure to delete this item?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    toDoList.Rows[toDoListView.CurrentCell.RowIndex].Delete();
                }
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                if (isEditing)
                {
                    toDoList.Rows[toDoListView.CurrentCell.RowIndex]["Title"] = textBoxTitle.Text;
                    toDoList.Rows[toDoListView.CurrentCell.RowIndex]["Description"] = textBoxDescription.Text;
                    toDoList.Rows[toDoListView.CurrentCell.RowIndex]["Due Date"] = dueDatePicker.Value.ToString("yyyy-MM-dd");
                    toDoList.Rows[toDoListView.CurrentCell.RowIndex]["Priority"] = priorityBox.SelectedItem.ToString();
                }
                else
                {
                    toDoList.Rows.Add(textBoxTitle.Text, textBoxDescription.Text, dueDatePicker.Value.ToString("yyyy-MM-dd"), priorityBox.SelectedItem.ToString());
                }
                ClearTextBoxes();
                isEditing = false;
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(textBoxTitle.Text))
            {
                MessageBox.Show("Title cannot be empty.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(textBoxDescription.Text))
            {
                MessageBox.Show("Description cannot be empty.");
                return false;
            }
            if (priorityBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a priority level.");
                return false;
            }
            return true;
        }
    }
}
