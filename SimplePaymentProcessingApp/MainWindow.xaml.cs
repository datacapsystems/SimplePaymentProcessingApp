using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;
using System.Text.Json.Serialization;
using SimplePaymentProcessingApp.Credit;
using SimplePaymentProcessingApp.General;

namespace SimplePaymentProcessingApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        void OnExecuteButtonPressed(object sender, RoutedEventArgs args)
        {
            CreditTransactionRequest? request = null;
            // Attempt to deserialize the request.
            try
            {
                request = JsonSerializer.Deserialize<CreditTransactionRequest>(InputBox.Text);
            }
            // Catch any exceptions that may occur during deserialization and print it to a popup box (for your convenience).
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                OutputBox.Text = "Oops! An error occurred when parsing the request.";
                return;
            }

            // If the request is successfully deserialized, attempt to process the transaction.
            if (request != null)
            {
                bool checkDuplicate = CheckDuplicateCheckbox.IsChecked.HasValue ? CheckDuplicateCheckbox.IsChecked.Value : false;
                bool validateExpirationDate = ValidateExpirationDateCheckbox.IsChecked.HasValue ? ValidateExpirationDateCheckbox.IsChecked.Value : false;
                bool requireCardholderName = RequireCardholderNameCheckbox.IsChecked.HasValue ? RequireCardholderNameCheckbox.IsChecked.Value : false;
                bool waiveFee = WaiveFeeCheckbox.IsChecked.HasValue ? WaiveFeeCheckbox.IsChecked.Value : false;

                TransactionResponse response = CreditTransactionProcessor.ProcessTransaction(request, checkDuplicate, validateExpirationDate, requireCardholderName, waiveFee);

                // Write response to output text box.
                OutputBox.Text = JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
            }
        }
    }
}
