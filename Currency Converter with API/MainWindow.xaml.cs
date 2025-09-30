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





namespace Currency_Converter_with_API
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
        Root val = new Root();
        public MainWindow()
        {
            InitializeComponent();
            ClearControls();
            GetValues();

        }

        private async void GetValues()
        {
            val = await GetDataGetMethod<Root>("https://api.freecurrencyapi.com/v1/latest?apikey=fca_live_6vWt6N2Il5g0qLzHa7Gp4UwHtyWnRsawNoCY7K0j&currencies=USD%2CEUR%2CGBP%2CJPY%2CAUD%2CCAD%2CCHF%2CCNY%2CSEK%2CNZD");
            BindCurrencies();
        }

        public static async Task<Root> GetDataGetMethod<T>(string url)
        {
            var ss = new Root();
            try
            {
                //HttpClient class provides a base class for sending/receiving the HTTP requests/responses from a URL.
                using (var client = new HttpClient())
                {
                    //The timespan to wait before the request times out.
                    client.Timeout = TimeSpan.FromMinutes(1);
                    //HttpResponseMessage is a way of returning a message/data from your action.
                    HttpResponseMessage response = await client.GetAsync(url);
                    //Check API response status code ok
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        //Serialize the HTTP content to a string as an asynchronous operation.
                        var responseString = await response.Content.ReadAsStringAsync();
                        //JsonConvert.DeserializeObject to deserialize Json to a C#
                        var responseObject = JsonSerializer.Deserialize<Root>(responseString);
                        return responseObject; //Return API responce
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
            //Create an object Datatable
            DataTable dtCurrencies = new DataTable();
            dtCurrencies.Columns.Add("Name"); //Add display column in DataTable
            dtCurrencies.Columns.Add("Rate"); //Add value column in DataTable
            if (val?.data == null)
            {
                MessageBox.Show("Unable to fetch currency rates. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //Add rows in Datatable with text and value. Set a value which fetch from API
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
            cmbFromCurrency.ItemsSource = dtCurrencies.DefaultView; //Datatable data assign From currency Combobox
            cmbToCurrency.ItemsSource = dtCurrencies.DefaultView;
            cmbFromCurrency.DisplayMemberPath = "Name"; //DisplayMemberPath property is used to display data in Combobox
            cmbFromCurrency.SelectedValuePath = "Rate"; //SelectedValuePath property is used to set value in Combobox
            cmbToCurrency.DisplayMemberPath = "Name";
            cmbToCurrency.SelectedValuePath = "Rate";
            cmbFromCurrency.SelectedIndex = 0; //SelectedIndex property is used for when bind Combobox it's default selected item is first
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
            txtCurrency.Focus();
        }

        //Allow only integer in textbox
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            e.Handled = regex.IsMatch(e.Text);
        }
        #endregion

        #region Button Click Event

        //Assign convert button click event
        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            //Declare ConvertedValue with double data type to store currency converted value
            double ConvertedValue;

            //Check if amount textbox is Null or Blank
            if (txtCurrency.Text == null || txtCurrency.Text.Trim() == "")
            {
                //If amount textbox is Null or Blank then show this message box
                MessageBox.Show("Please Enter Currency", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                //After clicking on the Messagebox's OK button set the focus on amount textbox
                txtCurrency.Focus();
                return;
            }

            //Else if currency From is not selected or default text as --SELECT--
            else if (cmbFromCurrency.SelectedValue == null || cmbFromCurrency.SelectedIndex == 0 || cmbFromCurrency.Text == "--SELECT--")
            {
                //Then show this message box
                MessageBox.Show("Please Select Currency From", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                //Set the focus to From Combobox
                cmbFromCurrency.Focus();
                return;
            }
            //else if currency To is not Selected or default text as --SELECT--
            else if (cmbToCurrency.SelectedValue == null || cmbToCurrency.SelectedIndex == 0 || cmbToCurrency.Text == "--SELECT--")
            {
                //Then show this message box
                MessageBox.Show("Please Select Currency To", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                //Set the focus on To Combobox
                cmbToCurrency.Focus();
                return;
            }
            //If From and To Combobox selects the same value
            if (cmbFromCurrency.Text == cmbToCurrency.Text)
            {
                //Amount textbox value is set in ConvertedValue. double.parse is used to change datatype String To Double. Textbox text have string and ConvertedValue is double datatype
                ConvertedValue = double.Parse(txtCurrency.Text);

                //Show the label as converted currency and converted currency name. and ToString("N3") is used to placed 000 after the dot(.)
                lblCurrency.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N3");
            }
            else
            {
                //Calculation for currency converter is From currency value multiplied(*) with amount textbox value and then that total divided(/) with To currency value.
                ConvertedValue = (double.Parse(cmbToCurrency.SelectedValue.ToString()) * double.Parse(txtCurrency.Text)) / double.Parse(cmbFromCurrency.SelectedValue.ToString());

                //Show the label converted currency and converted currency name.
                lblCurrency.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N3");
            }
        }

        //Assign a clear button click event
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ClearControls();
        }
        #endregion
    }
}