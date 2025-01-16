using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ExpenseCalculator
{
    public partial class MainWindow : Window
    {
        // List to store all expenses
        private readonly List<Expense> _expenses = new List<Expense>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddExpense_Click(object sender, RoutedEventArgs e)
        {
            // Get user input
            string amountText = AmountTextBox.Text;
            string description = DescriptionTextBox.Text;
            DateTime? selectedDate = ExpenseDatePicker.SelectedDate;

            // Validate input
            if (string.IsNullOrWhiteSpace(amountText) || string.IsNullOrWhiteSpace(description) || !selectedDate.HasValue)
            {
                MessageBox.Show("Please enter all fields (amount, description, and date).", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(amountText, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Please enter a valid amount.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Add to the list
            var expense = new Expense
            {
                Amount = amount,
                Description = description,
                Date = selectedDate.Value
            };
            _expenses.Add(expense);

            // Update the ListBox
            ExpenseListBox.Items.Add($"{expense.Date:yyyy-MM-dd}: {expense.Description} - ${expense.Amount:F2}");

            // Clear input fields
            AmountTextBox.Clear();
            DescriptionTextBox.Clear();
            ExpenseDatePicker.SelectedDate = null;
        }

        private void CalculateMonthlyTotal_Click(object sender, RoutedEventArgs e)
        {
            // Prompt the user to select a month and year
            var selectedDate = ExpenseDatePicker.SelectedDate;

            if (!selectedDate.HasValue)
            {
                MessageBox.Show("Please select a date to calculate the monthly total.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int selectedMonth = selectedDate.Value.Month;
            int selectedYear = selectedDate.Value.Year;

            // Calculate the total expenses for the selected month
            var monthlyTotal = _expenses
                .Where(exp => exp.Date.Month == selectedMonth && exp.Date.Year == selectedYear)
                .Sum(exp => exp.Amount);

            // Display the result
            MessageBox.Show($"Total for {selectedDate.Value:MMMM yyyy}: ${monthlyTotal:F2}", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    public class Expense
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
