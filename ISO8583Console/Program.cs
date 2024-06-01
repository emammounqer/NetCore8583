using System;
using System.Windows.Forms;
using NetCore8583;
using Newtonsoft.Json.Linq;


namespace ISO8583Console
{
    class MainForm : Form
    {
        public MainForm()
        {
            // Initialize form
            Text = "Simple Form";
            Width = 300;
            Height = 150;

            // Add controls
            var nameLabel = new Label { Text = "Name:", Left = 20, Top = 20 };
            var nameTextBox = new TextBox { Left = 80, Top = 20, Width = 150 };
            var ageLabel = new Label { Text = "Age:", Left = 20, Top = 50 };
            var ageTextBox = new TextBox { Left = 80, Top = 50, Width = 50 };
            var submitButton = new Button { Text = "Submit", Left = 120, Top = 80 };

            Controls.AddRange(new Control[] { nameLabel, nameTextBox, ageLabel, ageTextBox, submitButton });

            submitButton.Click += (sender, e) =>
            {
                // Handle button click event
                MessageBox.Show($"Name: {nameTextBox.Text}\nAge: {ageTextBox.Text}");
            };
        }
    }

    internal class Program
    {

        //static void Main(string[] args)
        //{
        //    var jsonString = "{\"Id\":\"0400(7)0217112922(11)141275(32)013445(37)004811143710\",\"SourceId\":\"mc.0217112922.141275.\",\"ResponseCode\":null,\"ResponseMessage\":null,\"Message\":{\"100\":null,\"101\":null,\"102\":null,\"103\":null,\"104\":null,\"105\":null,\"106\":null,\"107\":null,\"108\":null,\"109\":null,\"110\":null,\"111\":null,\"112\":null,\"113\":null,\"114\":null,\"115\":null,\"116\":null,\"117\":null,\"118\":null,\"119\":null,\"120\":null,\"121\":null,\"122\":null,\"123\":null,\"124\":null,\"125\":null,\"126\":null,\"127\":null,\"128\":null,\"h.ReplyTo\":\"switch.iso.in.acc-http-mux\",\"d\":null,\"000\":\"0400\",\"002\":\"5558486565175219\",\"003\":\"010000\",\"004\":\"000000000500\",\"005\":null,\"006\":\"000000000500\",\"007\":\"0217133222\",\"008\":null,\"009\":null,\"010\":\"61000000\",\"011\":\"141275\",\"012\":\"121922\",\"013\":\"0217\",\"014\":\"2308\",\"015\":\"0217\",\"016\":\"0216\",\"017\":null,\"018\":\"4121\",\"019\":null,\"020\":null,\"021\":null,\"022\":\"812\",\"023\":null,\"024\":null,\"025\":null,\"026\":null,\"027\":null,\"028\":null,\"029\":null,\"030\":null,\"031\":null,\"032\":\"013445\",\"033\":\"200353\",\"034\":null,\"035\":null,\"036\":null,\"037\":\"40314856121\",\"038\":\"016661\",\"039\":\"00\",\"040\":null,\"041\":null,\"042\":\"526567000022884\",\"043\":\"BOLT.EU /O/2002171119  Tallinn       EST\",\"044\":null,\"045\":null,\"046\":null,\"047\":null,\"048\":\"T6315MRGMXT6LV0217  \",\"049\":\"840\",\"050\":null,\"051\":\"840\",\"052\":null,\"053\":null,\"054\":null,\"055\":null,\"056\":null,\"057\":null,\"058\":null,\"059\":null,\"060\":null,\"061\":\"102410400600023310134\",\"062\":\"1525819383625082\",\"063\":\"MRGM7YVLV\",\"064\":null,\"065\":null,\"066\":null,\"067\":null,\"068\":null,\"069\":null,\"070\":null,\"071\":null,\"072\":null,\"073\":null,\"074\":null,\"075\":null,\"076\":null,\"077\":null,\"078\":null,\"079\":null,\"080\":null,\"081\":null,\"082\":null,\"083\":null,\"084\":null,\"085\":null,\"086\":null,\"087\":null,\"088\":null,\"089\":null,\"090\":\"010014371002171119220000001344500000200353\",\"091\":null,\"092\":null,\"093\":null,\"094\":null,\"095\":null,\"096\":null,\"097\":null,\"098\":null,\"099\":null,\"048.13\":null,\"048.17\":null,\"048.20\":null,\"048.26\":\"103\",\"048.30\":\"Tk1hTA/E5l0Km7cFoK548xSBSFLz19n4OQuPdiwu938=\",\"048.33\":\"0101C0216555848626545435103042702080205061150110030273\",\"048.38\":null,\"048.42\":null,\"048.43\":\"kBMrhCyPqGPJ/g0ZftVBXJeB2mVI\",\"048.61\":\"00001\",\"048.63\":null,\"048.71\":\"50C 51V 18C \",\"048.72\":null,\"048.77\":null,\"048.80\":null,\"048.82\":null,\"048.83\":null,\"048.84\":null,\"048.87\":null,\"048.89\":null,\"048.92\":null,\"048.98\":null,\"048.99\":null}}";
        //    //string jsonString = "{\"name\":\"John\",\"age\":30}";

        //    // Deserialize JSON string to JObject
        //    JObject jsonObject = JObject.Parse(jsonString);

        //    // Serialize JObject with indented formatting
        //    string formattedJson = jsonObject.ToString(Newtonsoft.Json.Formatting.Indented);

        //    // Write formatted JSON string to console
        //    Console.WriteLine(formattedJson);
        //    Console.ReadLine();
        //}
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new MainForm());
        }
    }
}
