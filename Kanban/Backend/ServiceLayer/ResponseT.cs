using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class Response<T>
    {
        private string ErrorMsg;
        private T ReturnVal;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string ErrorMessage { get { return ErrorMsg; } set { ErrorMsg = value; } }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public T ReturnValue { get { return ReturnVal; } set { ReturnVal = value; } }

        
        public Response(Exception e)
        {
            ErrorMessage= e.Message;
        }
        public Response(T value)
        {
            ReturnVal= value;
        }

        public Response()
        {
        }
    }
}
