using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PortRec.Common.Library.Models
{
    public class APIResponse<T>
    {
        [JsonPropertyName("status")]
        public bool Status { get; set; }

        [JsonPropertyName("data")]
        public T? Data { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("StatusCode")]
        public HttpStatusCode? StatusCode { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("errors")]
        public string[]? Errors { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public APIResponse() { }

        /// <summary>
        /// Set succes status response with data
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="Data"></param>
        public APIResponse(bool Status, T? Data, HttpStatusCode? statusCode = null)
        {
            this.Status = Status;
            this.Data = Data;
            this.StatusCode = statusCode;
        }

        /// <summary>
        /// Set error status response with error data
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="Errors"></param>
        public APIResponse(bool Status, string[]? Errors, HttpStatusCode? statusCode = null)
        {
            this.Status = Status;
            this.Errors = Errors;
            this.StatusCode = statusCode;
        }
    }
}
