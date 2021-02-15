using System.Collections.Generic;

namespace DotNetConf.Api.Models.BaseModels
{
    public class BaseResponseModel<T> : BaseResponseModel
    where T : class
    {
        public BaseResponseModel(T ResponseModel)
        {
            this.ResponseModel = ResponseModel;
        }
        public T ResponseModel { get; set; }
    }


    public class BaseResponseModel
    {
        public BaseResponseModel()
        {
            DocumentUrl = "http://localhost:5000/swagger/index.html";
            Messages = new List<string>();
            Links = new List<LinkModel>();
        }

        public BaseResponseModel(string message)
        {
            DocumentUrl = "http://localhost:5000/swagger/index.html";
            Messages = new List<string>() { message };
            Links = new List<LinkModel>();
        }
        public List<LinkModel> Links { get; set; }
        public List<string> Messages { get; set; }
        public string DocumentUrl { get; private set; }
    }
}
