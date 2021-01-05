namespace Dropship.Website.Backend.Models.Responses
{
    public class GenericResponse<T> : GenericResponse
    {
        public T Data { get; set; }
    }

    public class GenericResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}