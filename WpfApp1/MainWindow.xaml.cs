using System;
using System.Collections.Generic;
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
using NetCore8583;
using WpfApp1.Models;
using Newtonsoft.Json;
using NetCore8583.Parse;
using NetCore8583.Util;
using System.Security.Permissions;
using Newtonsoft.Json.Bson;
using static System.Net.Mime.MediaTypeNames;

namespace WpfApp1
{
    public class IsoGridColumn
    {
        public string Field { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ParseJsonBody(object sender, RoutedEventArgs e)
        {
            //var jsonString = "{\"Id\":\"0400(7)0217112922(11)141275(32)013445(37)004811143710\",\"SourceId\":\"mc.0217112922.141275.\",\"ResponseCode\":null,\"ResponseMessage\":null,\"Message\":{\"100\":null,\"101\":null,\"102\":null,\"103\":null,\"104\":null,\"105\":null,\"106\":null,\"107\":null,\"108\":null,\"109\":null,\"110\":null,\"111\":null,\"112\":null,\"113\":null,\"114\":null,\"115\":null,\"116\":null,\"117\":null,\"118\":null,\"119\":null,\"120\":null,\"121\":null,\"122\":null,\"123\":null,\"124\":null,\"125\":null,\"126\":null,\"127\":null,\"128\":null,\"h.ReplyTo\":\"switch.iso.in.acc-http-mux\",\"d\":null,\"000\":\"0400\",\"002\":\"5558486565175219\",\"003\":\"010000\",\"004\":\"000000000500\",\"005\":null,\"006\":\"000000000500\",\"007\":\"0217133222\",\"008\":null,\"009\":null,\"010\":\"61000000\",\"011\":\"141275\",\"012\":\"121922\",\"013\":\"0217\",\"014\":\"2308\",\"015\":\"0217\",\"016\":\"0216\",\"017\":null,\"018\":\"4121\",\"019\":null,\"020\":null,\"021\":null,\"022\":\"812\",\"023\":null,\"024\":null,\"025\":null,\"026\":null,\"027\":null,\"028\":null,\"029\":null,\"030\":null,\"031\":null,\"032\":\"013445\",\"033\":\"200353\",\"034\":null,\"035\":null,\"036\":null,\"037\":\"40314856121\",\"038\":\"016661\",\"039\":\"00\",\"040\":null,\"041\":null,\"042\":\"526567000022884\",\"043\":\"BOLT.EU /O/2002171119  Tallinn       EST\",\"044\":null,\"045\":null,\"046\":null,\"047\":null,\"048\":\"T6315MRGMXT6LV0217  \",\"049\":\"840\",\"050\":null,\"051\":\"840\",\"052\":null,\"053\":null,\"054\":null,\"055\":null,\"056\":null,\"057\":null,\"058\":null,\"059\":null,\"060\":null,\"061\":\"102410400600023310134\",\"062\":\"1525819383625082\",\"063\":\"MRGM7YVLV\",\"064\":null,\"065\":null,\"066\":null,\"067\":null,\"068\":null,\"069\":null,\"070\":null,\"071\":null,\"072\":null,\"073\":null,\"074\":null,\"075\":null,\"076\":null,\"077\":null,\"078\":null,\"079\":null,\"080\":null,\"081\":null,\"082\":null,\"083\":null,\"084\":null,\"085\":null,\"086\":null,\"087\":null,\"088\":null,\"089\":null,\"090\":\"010014371002171119220000001344500000200353\",\"091\":null,\"092\":null,\"093\":null,\"094\":null,\"095\":null,\"096\":null,\"097\":null,\"098\":null,\"099\":null,\"048.13\":null,\"048.17\":null,\"048.20\":null,\"048.26\":\"103\",\"048.30\":\"Tk1hTA/E5l0Km7cFoK548xSBSFLz19n4OQuPdiwu938=\",\"048.33\":\"0101C0216555848626545435103042702080205061150110030273\",\"048.38\":null,\"048.42\":null,\"048.43\":\"kBMrhCyPqGPJ/g0ZftVBXJeB2mVI\",\"048.61\":\"00001\",\"048.63\":null,\"048.71\":\"50C 51V 18C \",\"048.72\":null,\"048.77\":null,\"048.80\":null,\"048.82\":null,\"048.83\":null,\"048.84\":null,\"048.87\":null,\"048.89\":null,\"048.92\":null,\"048.98\":null,\"048.99\":null}}";
            var jsonString = GetTextFromRichTextBox(txtJsonBody);
            if (TryDeseializeMessage(jsonString, out Dictionary<string,string> message))
            {
                // Set the ItemsSource of the DataGrid to the list of people
                List<IsoGridColumn> rows = new List<IsoGridColumn>();
                foreach (var messageData in message)
                {
                    if (messageData.Value == null)
                        continue;

                    descriptions.TryGetValue(messageData.Key, out string desc);
                    rows.Add(new IsoGridColumn { Field = messageData.Key, Description = desc, Value = messageData.Value });
                }
                FillGrid(rows);
            }
            else
            {
                MessageBox.Show("not valid message", "Error Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void ParseMessage(object sender, RoutedEventArgs e)
        {
            /*
0200F638401F28A0A010000060000400000016421498001022408401200000000000250000000000250006081210248682601024140806601100001000000010000000100000001000000006686868374214980010224084391=1200000000000003255689353700002500029 ATM SIMULATOR  GPACK  TEMENOS  CHENNAI
840840003SML0000000100000000000100000560062
            */
            try
            {
                var text = GetTextFromRichTextBox(txtIsoMessage);
                var isoMessage = ParseIsoMessage(StringUtil.GetSignedBytes(text));
                FillGrid(GetDataSource(isoMessage));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void FillGrid(List<IsoGridColumn> data)
        {
            MyDataGrid.ItemsSource = data;
        }
        
        private static IsoMessage ParseIsoMessage(sbyte[] messageStream)
        {
            // create message factory with data element specification
            MessageFactory<IsoMessage> messageFactory = new MessageFactory<IsoMessage>();
            //MessageFactory<IsoMessage> messageFactory = ConfigParser.CreateDefault();
            messageFactory.SetParseMap(0X200, GetParaseMap());

            // reading ISO8583 message came through TCP/IP connection. So the message is coming as a byte stream
            IsoMessage isoMessage = messageFactory.ParseMessage(messageStream, 0);

            return isoMessage;
        }

        private List<IsoGridColumn> GetDataSource(IsoMessage isoMessage)
        {
            List<IsoGridColumn> output = new List<IsoGridColumn>();
            //Fields
            for (var i = 2; i < 129; i++)
            {
                var v = isoMessage.GetField(i);
                if (v != null)
                {
                    var field = i.ToString().PadLeft(3, '0');
                    var description = descriptions[field];
                    var value = v.Value.ToString();

                    output.Add(new IsoGridColumn { Field = field, Description = description, Value = value });
                }
            }
            return output;
        }

        private static bool TryDeseializeMessage<T>(string jsonString, out T parsedObject)
        {
            parsedObject = default;
            try
            {
                parsedObject = JsonConvert.DeserializeObject<T>(jsonString);
                return parsedObject != null;
            }
            catch (Exception)
            {

                return false;
            }
        }

        private string GetTextFromRichTextBox(RichTextBox richTextBox)
        {
            TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            return textRange.Text;
        }

        private static Dictionary<int, FieldParseInfo> GetParaseMap()
        {
            //0200C00000000000000000000000000000001642149800102240840
            Dictionary<int, FieldParseInfo> parseMap = new Dictionary<int, FieldParseInfo>
            {
                { 2, new LlvarParseInfo() },
            };

            return parseMap;
        }

        public static readonly Dictionary<string, string> descriptions = new Dictionary<string, string> {
            { "000", "Message Type" },
            { "002", "Primary Account Number (PAN)" },
            { "003", "Processing Code" },
            { "004", "Amount, Transaction" },
            { "005", "Amount, Settlement" },
            { "006", "Amount, Cardholder Billing" },
            { "007", "Transmission Date & Time" },
            { "008", "Amount, Cardholder Billing Fee" },
            { "009", "Conversion Rate, Settlement" },
            { "010", "Conversion Rate, Cardholder Billing" },
            { "011", "Systems Trace Audit Number" },
            { "012", "Time, Local Transaction" },
            { "013", "Date, Local Transaction" },
            { "014", "Date, Expiration" },
            { "015", "Date, Settlement" },
            { "016", "Date, Conversion" },
            { "017", "Date, Capture" },
            { "018", "Merchant Type" },
            { "019", "Acquiring Institution Country Code" },
            { "020", "PAN Extended, Country Code" },
            { "021", "Forwarding Institution Country Code" },
            { "022", "Point of Service Entry Mode" },
            { "023", "Card Sequence Number" },
            { "024", "Function Code" },
            { "025", "Point of Service Condition Code" },
            { "026", "Point of Service PIN Capture Code" },
            { "027", "Authorization Identification Response Length" },
            { "028", "Amount, Transaction Fee" },
            { "029", "Amount, Settlement Fee" },
            { "030", "Amount, Transaction Processing Fee" },
            { "031", "Amount, Settlement Processing Fee" },
            { "032", "Acquiring Institution Identification Code" },
            { "033", "Forwarding Institution Identification Code" },
            { "034", "Primary Account Number, Extended" },
            { "035", "Track 2 Data" },
            { "036", "Track 3 Data" },
            { "037", "Retrieval Reference Number" },
            { "038", "Authorization Identification Response" },
            { "039", "Response Code" },
            { "040", "Service Restriction Code" },
            { "041", "Card Acceptor Terminal Identification" },
            { "042", "Card Acceptor Identification Code" },
            { "043", "Card Acceptor Name/Location" },
            { "044", "Additional Response Data" },
            { "045", "Track 1 Data" },
            { "046", "Amounts, Fees" },
            { "047", "Additional Data - National" },
            { "048", "Additional Data - Private" },
            { "049", "Currency Code, Transaction" },
            { "050", "Currency Code, Settlement" },
            { "051", "Currency Code, Cardholder Billing" },
            { "052", "Personal Identification Number (PIN) Data" },
            { "053", "Security Related Control Information" },
            { "054", "Amounts, Additional" },
            { "055", "IC Card System Related Data" },
            { "056", "Original Data Elements" },
            { "057", "Authorization Life Cycle Code" },
            { "058", "Authorizing Agent Institution ID Code" },
            { "059", "Transport Data" },
            { "060", "Reserved for National Use" },
            { "061", "Reserved for National Use" },
            { "062", "Reserved for Private Use" },
            { "063", "Reserved for Private Use" },
            { "064", "Message Authentication Code (MAC)" },
            { "065", "Reserved for ISO Use" },
            { "066", "Amount, Network Management Fee" },
            { "067", "Payee" },
            { "068", "Settlement Institution Identification Code" },
            { "069", "Receiving Institution Identification Code" },
            { "070", "Account Identification 1" },
            { "071", "Account Identification 2" },
            { "072", "Reserved for ISO Use" },
            { "073", "Reserved for ISO Use" },
            { "074", "Reserved for ISO Use" },
            { "075", "Reserved for ISO Use" },
            { "076", "Reserved for ISO Use" },
            { "077", "Reserved for ISO Use" },
            { "078", "Reserved for ISO Use" },
            { "079", "Reserved for ISO Use" },
            { "080", "Reserved for ISO Use" },
            { "081", "Reserved for ISO Use" },
            { "082", "Reserved for ISO Use" },
            { "083", "Reserved for ISO Use" },
            { "084", "Reserved for ISO Use" },
            { "085", "Reserved for ISO Use" },
            { "086", "Reserved for ISO Use" },
            { "087", "Reserved for ISO Use" },
            { "088", "Reserved for ISO Use" },
            { "089", "Reserved for ISO Use" },
            { "090", "Original Data Elements" },
            { "091", "File Update Code" },
            { "092", "File Security Code" },
            { "093", "Response Indicator" },
            { "094", "Service Indicator" },
            { "095", "Replacement Amounts" },
            { "096", "Message Security Code" },
            { "097", "Amount, Net Settlement" },
            { "098", "Payee" },
            { "099", "Settlement Institution Identification Code" },
            { "100", "Receiving Institution Identification Code" },
            { "101", "File Name" },
            { "102", "Account Identification 1" },
            { "103", "Account Identification 2" },
            { "104", "Transaction Description" },
            { "105", "Reserved for ISO Use" },
            { "106", "Reserved for ISO Use" },
            { "107", "Reserved for ISO Use" },
            { "108", "Reserved for ISO Use" },
            { "109", "Reserved for ISO Use" },
            { "110", "Reserved for ISO Use" },
            { "111", "Reserved for ISO Use" },
            { "112", "Reserved for National Use" },
            { "113", "Reserved for National Use" },
            { "114", "Reserved for National Use" },
            { "115", "Reserved for National Use" },
            { "116", "Reserved for National Use" },
            { "117", "Reserved for National Use" },
            { "118", "Reserved for National Use" },
            { "119", "Reserved for National Use" },
            { "120", "Reserved for Private Use" },
            { "121", "Reserved for Private Use" },
            { "122", "Reserved for Private Use" },
            { "123", "Reserved for Private Use" },
            { "124", "Reserved for Private Use" },
            { "125", "Reserved for Private Use" },
            { "126", "Reserved for Private Use" },
            { "127", "Reserved for Private Use" },
            { "128", "Message Authentication Code (MAC)" }
        };

        private void CreateIsoMessage(object sender, RoutedEventArgs e)
        {
            try
            {
                var jsonString = GetTextFromRichTextBox(txtJsonBody);

                MessageFactory<IsoMessage> messageFactory = new MessageFactory<IsoMessage>();
                IsoMessage isoMessage = messageFactory.NewMessage(0x200);



                if (TryDeseializeMessage(jsonString, out MessageBody message))
                {

                }

                txtIsoMessage.Document.Blocks.Clear();
                txtIsoMessage.Document.Blocks.Add(new Paragraph(new Run(isoMessage.DebugString())));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }
    }
}
