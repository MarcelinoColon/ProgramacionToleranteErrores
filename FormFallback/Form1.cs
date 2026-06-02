namespace FormFallback
{
    public partial class Form1 : Form
    {
        private DollarRate _dollarRate;
        public Form1(DollarRate dollarRate)
        {
            _dollarRate = dollarRate;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                decimal dollar = Convert.ToDecimal(txtDolares.Text);
                decimal rate = await _dollarRate.GetRateAsync();

                decimal pesos = dollar * rate;

                txtPesos.Text = pesos.ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error: "+ ex.Message);
            }
        }
    }
}
