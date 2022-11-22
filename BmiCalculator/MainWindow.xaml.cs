using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Data;

namespace BmiCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    [XmlRoot("BMI Calc", Namespace = "www.bmicalc.ninja")]
    public partial class MainWindow : Window
    {
        public string FilePath = "C:\\Users\\ali_gulfaam\\Desktop\\C#\\BmiCalculator\\BmiCalculator\\yourBMI.xml";
        public string FileName = "yourBMI.xml";

        #region part 1 of the lab, ClearBtn & Exit Btn
        private void ClearBtn_Click_1(object sender, RoutedEventArgs e)
        {
            xPhoneBox.Text = "";
            xFirstNameBox.Text = "";
            xLastNameBox.Text = "";
            xHeightInchesBox.Text = "";
            xWeightLbsBox.Text = "";
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        #endregion

        public class Customer
        {
            [XmlAttribute("Last Name")]
            public string? lastName { get; set; }

            [XmlAttribute("First Name")]
            public string? firstName { get; set; }

            [XmlAttribute("Phone Number")]
            public string? phoneNumber { get; set; }
            
            [XmlAttribute("Height")]
            public int heightInches { get; set; }

            [XmlAttribute("Weight")]
            public int weightLbs { get; set; }

            [XmlAttribute("Customer BMI")]
            public int custBMI { get; set; }

            [XmlAttribute("Status")]
            public string? statusTitle { get; set; }
        }  
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            Customer customer1 = new Customer();

            customer1.lastName = xLastNameBox.Text;
            customer1.firstName = xFirstNameBox.Text;
            customer1.phoneNumber = xPhoneBox.Text;
            customer1.statusTitle = xBMIMessage.Text;

            int currentWeight = 0;
            int currentHeight = 0;
            Int32.TryParse(xWeightLbsBox.Text, out currentWeight);
            Int32.TryParse(xHeightInchesBox.Text, out currentHeight);
            customer1.weightLbs = currentWeight;
            customer1.heightInches = currentHeight;

            int bmi;
            bmi = 703 * customer1.weightLbs / (customer1.heightInches * customer1.heightInches);
            customer1.custBMI = bmi;

            //MessageBox.Show(Convert.ToString(bmi));
            string yourBMISatus = "NA";
            customer1.statusTitle = yourBMISatus;
            //MessageBox.Show($"The Customer's Name is {customer1.firstName} {customer1.lastName} and they can be reached at {customer1.phoneNumber}. they are {customer1.heightInches} inches tall. their weight is {customer1.weightLbs}.  their Bmi is {bmi}");


            if (bmi < 18.5)
            {
                Console.WriteLine("According to CDC.gov BMi Calculator you are underweight");
                customer1.statusTitle = "Underweight";
            } if (bmi < 24.9)
            {
               Console.WriteLine("According to CDC.gov BMi Calculator you have a normal body weight");
                customer1.statusTitle = "Normal";
            } if (bmi < 29.9)
            {

                Console.WriteLine("According to CDC.gov BMi Calculator you are Overweight");
                customer1.statusTitle = "Overweight";
            }
            else
            {
                Console.WriteLine("According to CDC.gov BMi Calculator you are Obese");
                customer1.statusTitle = "Obese";
            }


            TextWriter writer = new StreamWriter(FilePath + FileName);
            XmlSerializer ser = new XmlSerializer(typeof(Customer));

            ser.Serialize(writer, customer1);
            writer.Close();

        }

        private void OnLoadStats(string custBMI)
        {
            Customer cust = new Customer();

            XmlSerializer des = new XmlSerializer(typeof(Customer));
            using (XmlReader reader = XmlReader.Create(FilePath + FileName))
            {
                cust = (Customer)des.Deserialize(reader);


                xLastNameBox.Text = cust.lastName;
                xFirstNameBox.Text = cust.firstName;
                xPhoneBox.Text = cust.phoneNumber;
                xYourBMIResults.Text = custBMI;
            }

            DataSet xmlData = new DataSet();
            xmlData.ReadXml(FilePath + FileName, XmlReadMode.Auto);
            xDataGrid.ItemsSource = xmlData.Tables[0].DefaultView;
        }
    }
}
