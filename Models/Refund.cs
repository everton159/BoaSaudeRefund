using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json.Serialization;

namespace BoaSaudeRefund
{

    public class RefundCreateModel
    {
        //Refund properties
        public string Reason { get; set; }
        public IFormFile File { get; set; }

    }

    public class Refund
    {
        //Refund properties
        public Int64 Id { get; set; }
        public string User { get; internal set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public byte[] File { get; set; }
    }
    

    public class RefundDto
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
    }



}
