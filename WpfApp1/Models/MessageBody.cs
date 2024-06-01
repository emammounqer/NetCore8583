using System.Collections.Generic;

namespace WpfApp1.Models
{
    public class MessageBody
    {
        public string Id { get; set; }
        public string SourceId { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public Dictionary<string, string> Message { get; set; }

        public MessageBody()
        {
            Message = new Dictionary<string, string>();
        }
    }
}
