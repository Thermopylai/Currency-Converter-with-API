using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Net.Http;
using System.Text.Json;
using System.Data;
using System.Text.RegularExpressions;
using System.IO;





namespace Currency_Converter_with_API
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //Store your own API key here:
        //You can get a free API key from http://freecurrencyapi.com
        //just by registering a user acount with a valid email address.
        //If you wan't to add more currencies to the app, you can do so
        //just by modifying the URL that is used in the GetValues() method and by
        //adding the needed row(s) into dtCurrencies DataTable in the BindCurrencies() method.
        public static string APIKey = string.Empty;
        //Add extra currencies here, separated with a comma (%2C).
        public static string currencies = "USD%2CEUR%2CGBP%2CJPY%2CAUD%2CCAD%2CCHF%2CCNY%2CSEK%2CNZD";


        //Create classes to deserialize the JSON response from the API
        //Root class is the main class which contains Rates class object
        //Rates class contains all currency properties which we want to fetch from API
        //The naming of the called class members must match the names of the JSON properties.
        public class Root { public Rates data { get; set; } }
        public class Rates
        {
            public double USD { get; set; }
            public double EUR { get; set; }
            public double GBP { get; set; }
            public double JPY { get; set; }
            public double AUD { get; set; }
            public double CAD { get; set; }
            public double CHF { get; set; }
            public double CNY { get; set; }
            public double SEK { get; set; }
            public double NZD { get; set; }
        }
        //Create an object of Root class
        Root val = new Root();
              
        public MainWindow()
        {
            InitializeComponent();
            ClearControls();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Setup();
            GetValues();
        }

        private void Setup()
        {
            if (File.Exists("setup.ini"))
            {
                APIKey = File.ReadAllText("setup.ini");
                txtCurrency.Focus();
            }
            else if (APIKey == string.Empty)
            {
                Setup setup = new Setup();
                setup.Owner = this;
                setup.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                setup.ShowDialog();

                Dispatcher.InvokeAsync(() =>
                {
                    txtCurrency.Focus();
                });
            }
        }

        //GetValues method is used to call GetDataGetMethod method and pass the API URL
        private async void GetValues()
        {
            //Call GetDataGetMethod method and pass the API URL
            //The method returns a Task<Root> which represents an asynchronous operation that will eventually produce a Root object.
            //The await keyword is used to suspend the execution of the async method until the awaited task completes.
            val = await GetDataGetMethod<Root>($"https://api.freecurrencyapi.com/v1/latest?apikey={APIKey}&currencies={currencies}");
            BindCurrencies();
        }

        //GetDataGetMethod is used to fetch data from API with GET method
        //It is a generic method where T is a placeholder for the type of data that will be returned by the method.
        //In this case, T is specified as Root when calling the method.
        //The async keyword indicates that the method is asynchronous and can use the await keyword to suspend execution until the awaited task completes.
        public static async Task<Root> GetDataGetMethod<T>(string url)
        {
            //Create an object of Root class
            var ss = new Root();
            try
            {
                //HttpClient class provides a base class for sending/receiving the HTTP requests/responses from a URL.
                //It is used to make HTTP requests in an asynchronous way.
                //The using statement ensures that the HttpClient instance is disposed of once it goes out of scope.
                //This is important for releasing unmanaged resources and avoiding potential memory leaks.
                using (var client = new HttpClient())
                {
                    //The timespan to wait before the request times out.
                    //Here we set the time to 1 minute.
                    client.Timeout = TimeSpan.FromMinutes(1);
                    //HttpResponseMessage is a way of returning a message/data from your action.
                    //GetAsync sends a GET request to the specified Uri as an asynchronous operation.
                    //GetAsync is a method of HttpClient class.
                    //It returns a Task<HttpResponseMessage> which represents the ongoing operation.
                    //The async modifier indicates that the method is asynchronous and can use the await operator.
                    //The await operator is used to suspend the execution of the async method until the awaited task completes.
                    HttpResponseMessage response = await client.GetAsync(url);
                    //Check API response status code ok
                    //StatusCode is a property of HttpResponseMessage class which gets the status code of the HTTP response.
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        //Serialize the HTTP content to a string as an asynchronous operation.
                        //ReadAsStringAsync to read the response body and return as a string in an asynchronous operation.
                        var responseString = await response.Content.ReadAsStringAsync();
                        //JsonSerializer.Deserialize to deserialize Json to a C#
                        //The naming of the called class members must match the names of the JSON properties.
                        //If there is a mismatch in the naming, the deserialization process may not work as expected.
                        //Here we deserialize the response string to Root object
                        var responseObject = JsonSerializer.Deserialize<Root>(responseString);
                        return responseObject; //Return the deserialized API response as a Root object
                    }
                    return ss;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching currency data: {ex.Message}", "API Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return ss;
            }
        }

        #region Bind Currency From and To Combobox
        private void BindCurrencies()
        {
            //Create an object DataTable
            DataTable dtCurrencies = new DataTable();
            dtCurrencies.Columns.Add("Name"); //Add display column in DataTable
            dtCurrencies.Columns.Add("Rate"); //Add value column in DataTable
            if (val?.data == null)
            {
                MessageBox.Show("Unable to fetch currency rates. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //Add rows in DataTable with text and value. Set a value which was fetched from API
            dtCurrencies.Rows.Add("--SELECT--", 0);
            dtCurrencies.Rows.Add("USD", val?.data.USD);
            dtCurrencies.Rows.Add("EUR", val?.data.EUR);
            dtCurrencies.Rows.Add("GBP", val?.data.GBP);    
            dtCurrencies.Rows.Add("JPY", val?.data.JPY);
            dtCurrencies.Rows.Add("AUD", val?.data.AUD);
            dtCurrencies.Rows.Add("CAD", val?.data.CAD);
            dtCurrencies.Rows.Add("CHF", val?.data.CHF);
            dtCurrencies.Rows.Add("CNY", val?.data.CNY);
            dtCurrencies.Rows.Add("SEK", val?.data.SEK);
            dtCurrencies.Rows.Add("NZD", val?.data.NZD);
            cmbFromCurrency.ItemsSource = dtCurrencies.DefaultView; //DataTable data assign FromCurrency ComboBox
            cmbToCurrency.ItemsSource = dtCurrencies.DefaultView;
            cmbFromCurrency.DisplayMemberPath = "Name"; //DisplayMemberPath property is used to display data in ComboBox
            cmbFromCurrency.SelectedValuePath = "Rate"; //SelectedValuePath property is used to set value in ComboBox
            cmbToCurrency.DisplayMemberPath = "Name";
            cmbToCurrency.SelectedValuePath = "Rate";
            cmbFromCurrency.SelectedIndex = 0; //SelectedIndex property is used for defining selected item in ComboBox. Default index is 0.
            cmbToCurrency.SelectedIndex = 0;
        }
        #endregion

        #region Extra Events

        //ClearControls used for clear all controls input which user entered
        private void ClearControls()
        {
            txtCurrency.Text = string.Empty;
            if (cmbFromCurrency.Items.Count > 0) cmbFromCurrency.SelectedIndex = 0;
            if (cmbToCurrency.Items.Count > 0) cmbToCurrency.SelectedIndex = 0; 
            lblCurrency.Content = string.Empty;
        }

        //Allow only integers in TextBox
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+"); //Regex that matches disallowed text
            e.Handled = regex.IsMatch(e.Text); //If the input text matches the regex, set Handled to true to prevent the input
        }
        #endregion

        #region Button Click Event

        //Assign convert button click event
        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            //Declare ConvertedValue with double data type to store currency converted value
            double ConvertedValue;

            //Check if Amount TextBox is Null or Blank
            if (txtCurrency.Text == null || txtCurrency.Text.Trim() == "")
            {
                //If Amount TextBox is Null or Blank then show this message box
                MessageBox.Show("Please Enter Currency", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                //After clicking on the MessageBox's OK button set the focus on amount TextBox
                txtCurrency.Focus();
                return;
            }

            //Else if FromCurrency is not selected or default text as --SELECT--
            else if (cmbFromCurrency.SelectedValue == null || cmbFromCurrency.SelectedIndex == 0 || cmbFromCurrency.Text == "--SELECT--")
            {
                //Then show this message box
                MessageBox.Show("Please Select Currency From", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                //Set the focus to FromCurrency ComboBox
                cmbFromCurrency.Focus();
                return;
            }
            //else if ToCurrency is not selected or default text as --SELECT--
            else if (cmbToCurrency.SelectedValue == null || cmbToCurrency.SelectedIndex == 0 || cmbToCurrency.Text == "--SELECT--")
            {
                //Then show this message box
                MessageBox.Show("Please Select Currency To", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                //Set the focus on ToCurrency ComboBox
                cmbToCurrency.Focus();
                return;
            }
            //If From and To ComboBox selects the same value
            if (cmbFromCurrency.Text == cmbToCurrency.Text)
            {
                //Amount TextBox value is set to ConvertedValue. Double.Parse is used to change datatype string to double. TextBox.Text is string and ConvertedValue is double datatype.
                ConvertedValue = double.Parse(txtCurrency.Text);

                //Show the label as converted currency and converted currency name and ToString("N3") is used to placed 000 after the dot(.)
                lblCurrency.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N3");
            }
            else
            {
                //Calculation for currency converter is FromCurrency value multiplied(*) with Amount TextBox value and then that total divided(/) by ToCurrency value.
                ConvertedValue = (double.Parse(cmbToCurrency.SelectedValue.ToString()) * double.Parse(txtCurrency.Text)) / double.Parse(cmbFromCurrency.SelectedValue.ToString());

                //Show the label converted currency and converted currency name.
                lblCurrency.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N3");
            }
        }

        //Assign clear button click event
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ClearControls();
            txtCurrency.Focus();
        }
        #endregion
    }
}